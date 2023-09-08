using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hertzole.UnityToolbox.Generator.Helpers;
using Hertzole.UnityToolbox.Shared;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Hertzole.UnityToolbox.Generator;

[ExportCodeFixProvider(LanguageNames.CSharp)]
[Shared]
public sealed class SubscribeMethodsCodeFixer : CodeFixProvider
{
	private ReferenceSymbols? referenceSymbols = null;

	public override ImmutableArray<string> FixableDiagnosticIds { get; } = ImmutableArray.Create("HUT0002");

	public override async Task RegisterCodeFixesAsync(CodeFixContext context)
	{
		SyntaxNode? root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
		if (root == null)
		{
			return;
		}

		Diagnostic diagnostic = context.Diagnostics[0];
		TextSpan diagnosticsSpan = diagnostic.Location.SourceSpan;

		SyntaxNode? parent = root.FindNode(diagnosticsSpan).Parent;

		TypeDeclarationSyntax? parentClass = parent?.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().FirstOrDefault();
		if (parentClass == null)
		{
			return;
		}

		FieldDeclarationSyntax? field = root.FindNode(diagnosticsSpan).Parent?.AncestorsAndSelf().OfType<FieldDeclarationSyntax>().FirstOrDefault();
		if (field == null)
		{
			return;
		}

		context.RegisterCodeFix(
			CodeAction.Create("Implement subscribe method",
				token => ImplementMethodAsync(context.Document, parentClass, field, token), "Implement subscribe method"),
			diagnostic);
	}

	public override FixAllProvider? GetFixAllProvider()
	{
		return WellKnownFixAllProviders.BatchFixer;
	}

	private async Task<Document> ImplementMethodAsync(Document contextDocument,
		TypeDeclarationSyntax typeDeclarationSyntax,
		FieldDeclarationSyntax fieldDeclarationSyntax,
		CancellationToken cancellationToken)
	{
		SemanticModel? semanticModel = await contextDocument.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
		if (semanticModel == null)
		{
			return contextDocument;
		}

		referenceSymbols ??= new ReferenceSymbols(semanticModel.Compilation);

		SymbolInfo symbolInfo = semanticModel.GetSymbolInfo(fieldDeclarationSyntax.Declaration.Type);
		ISymbol? fieldSymbolInfo = semanticModel.GetDeclaredSymbol(fieldDeclarationSyntax.Declaration.Variables.First());

		if (symbolInfo.Symbol is not INamedTypeSymbol typeSymbol || fieldSymbolInfo is not IFieldSymbol fieldSymbol)
		{
			Log.LogError(
				$"Can't get type symbols. {nameof(symbolInfo)}: {symbolInfo.Symbol}, {nameof(fieldSymbolInfo)}: {fieldSymbolInfo} | {fieldDeclarationSyntax.Declaration.Variables.First()}");

			return contextDocument;
		}

		if (!ScriptableValueHelper.TryGetScriptableType(typeSymbol, out ScriptableType scriptableType, out ITypeSymbol? genericType, referenceSymbols))
		{
			return contextDocument;
		}

		string fieldName = TextUtility.FormatVariableLabel(fieldDeclarationSyntax.Declaration.Variables.First().Identifier.Text);

		Log.LogInfo($"Implement method for {fieldName} with symbol {fieldSymbol}");

		if (referenceSymbols.GenerateLoadAttribute != null && fieldSymbol.TryGetAttribute(referenceSymbols.GenerateLoadAttribute, out _))
		{
			fieldName = TextUtility.FormatAddressableName(fieldName);
			Log.LogInfo($"Field is addressable, changing name to {fieldName}");
		}

		string methodName;
		switch (scriptableType)
		{
			case ScriptableType.Event:
			case ScriptableType.GenericEvent:
				methodName = $"On{fieldName}Invoked";
				break;
			case ScriptableType.Value:
				methodName = $"On{fieldName}Changed";
				break;
			default: // Includes none
				return contextDocument;
		}

		ParameterSyntax[] parameters = new ParameterSyntax[2];
		if (scriptableType == ScriptableType.Event || scriptableType == ScriptableType.GenericEvent)
		{
			parameters[0] = SyntaxFactory.Parameter(SyntaxFactory.Identifier("sender")).WithType(SyntaxFactory.ParseTypeName("object"));

			if (scriptableType == ScriptableType.GenericEvent)
			{
				parameters[1] = SyntaxFactory.Parameter(SyntaxFactory.Identifier("args"))
				                             .WithType(SyntaxFactory.ParseTypeName(genericType?.ToDisplayString() ?? "object"));
			}
			else
			{
				parameters[1] = SyntaxFactory.Parameter(SyntaxFactory.Identifier("e")).WithType(SyntaxFactory.ParseTypeName("System.EventArgs"));
			}
		}
		else
		{
			parameters[0] = SyntaxFactory.Parameter(SyntaxFactory.Identifier("previousValue"))
			                             .WithType(SyntaxFactory.ParseTypeName(genericType?.ToDisplayString() ?? "object"));

			parameters[1] = SyntaxFactory.Parameter(SyntaxFactory.Identifier("newValue"))
			                             .WithType(SyntaxFactory.ParseTypeName(genericType?.ToDisplayString() ?? "object"));
		}

		SyntaxNode? root = await contextDocument.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
		TypeDeclarationSyntax temp = typeDeclarationSyntax.AddMembers(SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), methodName)
		                                                                           .WithModifiers(SyntaxFactory.TokenList(
			                                                                           SyntaxFactory.Token(SyntaxKind.PrivateKeyword),
			                                                                           SyntaxFactory.Token(SyntaxKind.PartialKeyword)))
		                                                                           .WithParameterList(
			                                                                           SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(parameters)))
		                                                                           .WithBody(SyntaxFactory.Block(
			                                                                           SyntaxFactory.ParseStatement(
				                                                                           "throw new System.NotImplementedException();"))));

		if (root == null)
		{
			return contextDocument;
		}

		root = root.ReplaceNode(typeDeclarationSyntax, temp);
		return contextDocument.WithSyntaxRoot(root);
	}
}