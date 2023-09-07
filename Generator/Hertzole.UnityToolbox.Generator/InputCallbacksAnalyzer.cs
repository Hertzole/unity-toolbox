using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Hertzole.UnityToolbox.Generator.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Hertzole.UnityToolbox.Generator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class InputCallbacksAnalyzer : DiagnosticAnalyzer
{
	public static readonly DiagnosticDescriptor inputCallbackNotImplementedError = new DiagnosticDescriptor("HUT0004", "Input callback not implemented",
		"The input callback '{0}' is not implemented", "Unity Toolbox", DiagnosticSeverity.Error, true);

	private static readonly ImmutableArray<SymbolKind> symbolKinds = ImmutableArray.Create(SymbolKind.Field, SymbolKind.Property);

	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(inputCallbackNotImplementedError);

	public override void Initialize(AnalysisContext context)
	{
		context.EnableConcurrentExecution();
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);

		context.RegisterCompilationStartAction(compileContext =>
		{
			compileContext.RegisterSymbolAction(symbolContext =>
			{
				if (!symbolContext.Symbol.TryGetAttribute(InputCallbacksGenerator.ATTRIBUTE_NAME, out AttributeData? attribute) || attribute is null ||
				    attribute.AttributeClass is null)
				{
					return;
				}

				InputCallbackType callbackType = InputCallbackType.None;
				string startedMethodName = string.Empty;
				string performedMethodName = string.Empty;
				string canceledMethodName = string.Empty;
				string allMethodName = string.Empty;

				ImmutableArray<KeyValuePair<string, TypedConstant>> namedArguments = attribute.NamedArguments;
				foreach (KeyValuePair<string, TypedConstant> argumentPair in namedArguments)
				{
					if (argumentPair.Value.Value is bool value && value)
					{
						switch (argumentPair.Key)
						{
							case "GenerateStarted":
								callbackType |= InputCallbackType.Started;
								startedMethodName = $"OnInput{TextUtility.FormatVariableLabel(TextUtility.FormatAddressableName(symbolContext.Symbol.Name))}Started";
								break;
							case "GeneratePerformed":
								callbackType |= InputCallbackType.Performed;
								performedMethodName = $"OnInput{TextUtility.FormatVariableLabel(TextUtility.FormatAddressableName(symbolContext.Symbol.Name))}Performed";
								break;
							case "GenerateCanceled":
								callbackType |= InputCallbackType.Canceled;
								canceledMethodName = $"OnInput{TextUtility.FormatVariableLabel(TextUtility.FormatAddressableName(symbolContext.Symbol.Name))}Canceled";
								break;
							case "GenerateAll":
								callbackType |= InputCallbackType.All;
								allMethodName = $"OnInput{TextUtility.FormatVariableLabel(TextUtility.FormatAddressableName(symbolContext.Symbol.Name))}";
								break;
						}
					}
				}

				bool hasStartedMethod = false;
				bool hasPerformedMethod = false;
				bool hasCanceledMethod = false;
				bool hasAllMethod = false;

				ImmutableArray<ISymbol> members = symbolContext.Symbol.ContainingType.GetMembers();
				foreach (ISymbol member in members)
				{
					if (member is not IMethodSymbol methodSymbol)
					{
						continue;
					}

					if ((callbackType & InputCallbackType.Started) != 0 && IsValidMethod(methodSymbol) && string.Equals(methodSymbol.Name, startedMethodName))
					{
						hasStartedMethod = true;
					}

					if ((callbackType & InputCallbackType.Performed) != 0 && IsValidMethod(methodSymbol) &&
					    string.Equals(methodSymbol.Name, performedMethodName))
					{
						hasPerformedMethod = true;
					}

					if ((callbackType & InputCallbackType.Canceled) != 0 && IsValidMethod(methodSymbol) && string.Equals(methodSymbol.Name, canceledMethodName))
					{
						hasCanceledMethod = true;
					}

					if ((callbackType & InputCallbackType.All) != 0 && IsValidMethod(methodSymbol) && string.Equals(methodSymbol.Name, allMethodName))
					{
						hasAllMethod = true;
					}
				}

				Location? location = attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation();

				if (!hasStartedMethod && (callbackType & InputCallbackType.Started) != 0)
				{
					symbolContext.ReportDiagnostic(Diagnostic.Create(inputCallbackNotImplementedError, location, startedMethodName));
				}

				if (!hasPerformedMethod && (callbackType & InputCallbackType.Performed) != 0)
				{
					symbolContext.ReportDiagnostic(Diagnostic.Create(inputCallbackNotImplementedError, location, performedMethodName));
				}

				if (!hasCanceledMethod && (callbackType & InputCallbackType.Canceled) != 0)
				{
					symbolContext.ReportDiagnostic(Diagnostic.Create(inputCallbackNotImplementedError, location, canceledMethodName));
				}

				if (!hasAllMethod && (callbackType & InputCallbackType.All) != 0)
				{
					symbolContext.ReportDiagnostic(Diagnostic.Create(inputCallbackNotImplementedError, location, allMethodName));
				}
			}, symbolKinds);
		});
	}

	private static bool IsValidMethod(IMethodSymbol symbol)
	{
		if (!symbol.IsPartialDefinition || symbol.PartialImplementationPart == null)
		{
			return false;
		}

		if (symbol.Parameters.Length != 1)
		{
			return false;
		}

		return string.Equals("global::UnityEngine.InputSystem.InputAction.CallbackContext",
			symbol.Parameters[0].Type.ToDisplayString(NullableFlowState.None, SymbolDisplayFormat.FullyQualifiedFormat), StringComparison.Ordinal);
	}
}