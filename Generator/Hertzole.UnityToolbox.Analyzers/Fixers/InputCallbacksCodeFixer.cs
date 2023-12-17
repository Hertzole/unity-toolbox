using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hertzole.UnityToolbox.Shared;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Hertzole.UnityToolbox.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp)]
[Shared]
internal sealed class InputCallbacksCodeFixer : CodeFixProvider
{
	public override ImmutableArray<string> FixableDiagnosticIds { get; } = ImmutableArray.Create("HUT0004");

	public override async Task RegisterCodeFixesAsync(CodeFixContext context)
	{
		SyntaxNode? root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

		if (root is null)
		{
			return;
		}

		Diagnostic diagnostic = context.Diagnostics[0];
		TextSpan diagnosticSpan = diagnostic.Location.SourceSpan;

		SyntaxNode? parent = root.FindNode(diagnosticSpan).Parent;

		TypeDeclarationSyntax? parentClass = parent?.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().FirstOrDefault();
		if (parentClass is null)
		{
			return;
		}

		MemberDeclarationSyntax? field = root.FindNode(diagnosticSpan).Parent?.AncestorsAndSelf().OfType<MemberDeclarationSyntax>().FirstOrDefault();
		if (field is null)
		{
			return;
		}

		SemanticModel? semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);
		if (semanticModel is null)
		{
			return;
		}

		ISymbol? symbol = null;

		if (field is FieldDeclarationSyntax fieldSyntax)
		{
			symbol = semanticModel.GetDeclaredSymbol(fieldSyntax.Declaration.Variables[0]);
		}
		else if (field is PropertyDeclarationSyntax propertySyntax)
		{
			symbol = semanticModel.GetDeclaredSymbol(propertySyntax);
		}

		if (symbol is null || !symbol.TryGetAttribute(Attributes.GENERATE_INPUT_CALLBACKS, out AttributeData? attribute) || attribute == null)
		{
			return;
		}

		if (!diagnostic.Properties.TryGetValue("startedMethodName", out string? methodName))
		{
			if (!diagnostic.Properties.TryGetValue("performedMethodName", out methodName))
			{
				if (!diagnostic.Properties.TryGetValue("canceledMethodName", out methodName))
				{
					if (!diagnostic.Properties.TryGetValue("allMethodName", out methodName))
					{
						return;
					}
				}
			}
		}

		if (string.IsNullOrEmpty(methodName))
		{
			return;
		}

		context.RegisterCodeFix(
			CodeAction.Create($"Implement input callback '{methodName}'",
				token => ImplementMethodAsync(context.Document, parentClass, field, methodName, token),
				"Implement input callback"), diagnostic);
	}

	private static async Task<Document> ImplementMethodAsync(Document contextDocument,
		TypeDeclarationSyntax parentClass,
		MemberDeclarationSyntax field,
		string methodName,
		CancellationToken token)
	{
		SemanticModel? semanticModel = await contextDocument.GetSemanticModelAsync(token).ConfigureAwait(false);
		if (semanticModel == null)
		{
			return contextDocument;
		}

		SyntaxNode? root = await contextDocument.GetSyntaxRootAsync(token).ConfigureAwait(false);

		if (root is null)
		{
			return contextDocument;
		}

		string? fieldName = field switch
		{
			FieldDeclarationSyntax fieldDeclarationSyntax => fieldDeclarationSyntax.Declaration.Variables[0].Identifier.Text,
			PropertyDeclarationSyntax propertyDeclarationSyntax => propertyDeclarationSyntax.Identifier.Text,
			_ => null
		};

		if (string.IsNullOrEmpty(fieldName))
		{
			return contextDocument;
		}

		ParameterSyntax[] parameters = new ParameterSyntax[1];
		parameters[0] = SyntaxFactory.Parameter(SyntaxFactory.Identifier("context"))
		                             .WithType(SyntaxFactory.ParseTypeName("InputAction.CallbackContext"));

		TypeDeclarationSyntax temp = parentClass.AddMembers(SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), methodName)
		                                                                 .WithModifiers(SyntaxFactory.TokenList(
			                                                                 SyntaxFactory.Token(SyntaxKind.PrivateKeyword),
			                                                                 SyntaxFactory.Token(SyntaxKind.PartialKeyword)))
		                                                                 .WithParameterList(
			                                                                 SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(parameters)))
		                                                                 .WithBody(SyntaxFactory.Block(
			                                                                 SyntaxFactory.ParseStatement("throw new System.NotImplementedException();"))));

		root = root.ReplaceNode(parentClass, temp);

		return contextDocument.WithSyntaxRoot(root);
	}
}