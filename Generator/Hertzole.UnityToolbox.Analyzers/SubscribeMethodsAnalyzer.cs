using System;
using System.Collections.Immutable;
using Hertzole.UnityToolbox.Shared;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Hertzole.UnityToolbox.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class SubscribeMethodsAnalyzer : DiagnosticAnalyzer
{
	public static readonly DiagnosticDescriptor subscribedMethodNotCalledError = new DiagnosticDescriptor("HUT0002", "Subscribed method not implemented",
		"The subscribed method '{0}' is not implemented", "Unity Toolbox", DiagnosticSeverity.Error, true);

	private static readonly ImmutableArray<SymbolKind> symbolKinds = ImmutableArray.Create(SymbolKind.Field);

	private ReferenceSymbols? referenceSymbols;

	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(subscribedMethodNotCalledError);

	public override void Initialize(AnalysisContext analysisContext)
	{
		analysisContext.EnableConcurrentExecution();
		analysisContext.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);

		analysisContext.RegisterCompilationStartAction(compileContext =>
		{
			referenceSymbols ??= new ReferenceSymbols(compileContext.Compilation);

			// Check if the attribute exists.
			if (compileContext.Compilation.GetTypeByMetadataName("Hertzole.UnityToolbox.GenerateSubscribeMethodsAttribute") is not INamedTypeSymbol
			    generateSubscribeAttribute)
			{
				return;
			}

			// Check if 'object' exists.
			if (compileContext.Compilation.GetTypeByMetadataName("System.Object") is not INamedTypeSymbol objectSymbol)
			{
				return;
			}

			// Check if 'EventArgs' exists.
			if (compileContext.Compilation.GetTypeByMetadataName("System.EventArgs") is not INamedTypeSymbol eventArgsSymbol)
			{
				return;
			}

			INamedTypeSymbol? generateLoadAttribute = compileContext.Compilation.GetTypeByMetadataName("Hertzole.UnityToolbox.GenerateLoadAttribute");

			compileContext.RegisterSymbolAction(context =>
			{
				if (context.Symbol is not IFieldSymbol fieldSymbol)
				{
					return;
				}

				if (!ScriptableValueHelper.TryGetScriptableType(fieldSymbol.Type as INamedTypeSymbol, out ScriptableType scriptableType,
					    out ITypeSymbol? genericType))
				{
					return;
				}

				AttributeData? attributeData = null;

				string fieldName = fieldSymbol.Name;

				bool gotGenerateLoadAttribute = false;

				foreach (AttributeData attribute in context.Symbol.GetAttributes())
				{
					if (attribute.AttributeClass?.Equals(generateSubscribeAttribute, SymbolEqualityComparer.Default) == true)
					{
						attributeData = attribute;
					}

					if (generateLoadAttribute is not null && !gotGenerateLoadAttribute)
					{
						if (attribute.AttributeClass?.Equals(generateLoadAttribute, SymbolEqualityComparer.Default) == true)
						{
							gotGenerateLoadAttribute = true;
							fieldName = TextUtility.FormatAddressableName(fieldName);
						}
					}

					if ((generateLoadAttribute is null && attributeData is not null) ||
					    (generateLoadAttribute is not null && attributeData is not null && gotGenerateLoadAttribute))
					{
						break;
					}
				}

				if (attributeData == null)
				{
					return;
				}

				string methodName;

				switch (scriptableType)
				{
					case ScriptableType.None:
						return;
					case ScriptableType.Event:
					case ScriptableType.GenericEvent:
						methodName = $"On{TextUtility.FormatVariableLabel(fieldName)}Invoked";
						break;
					case ScriptableType.Value:
						methodName = $"On{TextUtility.FormatVariableLabel(fieldName)}Changed";
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				// Check if the correct method exists and is implemented.
				ImmutableArray<ISymbol> members = fieldSymbol.ContainingType.GetMembers(methodName);
				// Loop through all the members with the correct name.
				// There can be multiple members with the same name, but different parameters.
				// We need to check all of them to see if any of them are valid.
				foreach (ISymbol member in members)
				{
					if (member is not IMethodSymbol methodSymbol)
					{
						continue;
					}

					if (IsValidMethod(methodSymbol, scriptableType, genericType, objectSymbol, eventArgsSymbol))
					{
						return;
					}
				}

				// Report diagnostics.
				context.ReportDiagnostic(Diagnostic.Create(subscribedMethodNotCalledError,
					attributeData.ApplicationSyntaxReference?.GetSyntax(context.CancellationToken).GetLocation(), methodName));
			}, symbolKinds);
		});
	}

	private static bool IsValidMethod(IMethodSymbol symbol, ScriptableType type, ISymbol? genericType, ISymbol objectSymbol, ISymbol eventArgsSymbol)
	{
		// If it's a partial method and not implemented, it's not valid.
		if (symbol.PartialDefinitionPart != null || symbol.PartialImplementationPart == null)
		{
			return false;
		}

		// There's always two parameters on subscribed methods.
		if (symbol.Parameters.Length != 2)
		{
			return false;
		}

		// If it's an event, the first parameter must be the object.
		if ((type == ScriptableType.Event || type == ScriptableType.GenericEvent) &&
		    !symbol.Parameters[0].Type.Equals(objectSymbol, SymbolEqualityComparer.Default))
		{
			return false;
		}

		// Check if the method has the required parameters.
		switch (type)
		{
			case ScriptableType.Event:
				return symbol.Parameters[1].Type.Equals(eventArgsSymbol, SymbolEqualityComparer.Default);
			case ScriptableType.GenericEvent:
				return symbol.Parameters[1].Type.Equals(genericType, SymbolEqualityComparer.Default);
			case ScriptableType.Value:
				return symbol.Parameters[0].Type.Equals(genericType, SymbolEqualityComparer.Default) &&
				       symbol.Parameters[1].Type.Equals(genericType, SymbolEqualityComparer.Default);
			default: // Includes none
				return false;
		}
	}
}