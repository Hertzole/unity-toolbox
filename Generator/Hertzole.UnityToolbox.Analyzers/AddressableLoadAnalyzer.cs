using System.Collections.Immutable;
using Hertzole.UnityToolbox.Shared;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Hertzole.UnityToolbox.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class AddressableLoadAnalyzer : DiagnosticAnalyzer
{
	private static readonly LocalizableString title = new LocalizableResourceString(nameof(Resources.HUT0001Title), Resources.ResourceManager, typeof(Resources));
	private static readonly LocalizableString messageFormat = new LocalizableResourceString(nameof(Resources.HUT0001MessageFormat), Resources.ResourceManager, typeof(Resources));
	private static readonly LocalizableString description = new LocalizableResourceString(nameof(Resources.HUT0001Description), Resources.ResourceManager, typeof(Resources));
	
	private static readonly DiagnosticDescriptor rule = new DiagnosticDescriptor(DIAGNOSTIC_ID, title, messageFormat, CATEGORY, DiagnosticSeverity.Error, true, description: description);
    
	private static readonly ImmutableArray<SymbolKind> symbolKinds = ImmutableArray.Create(SymbolKind.Field, SymbolKind.Property);

	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(rule);
	
	public const string DIAGNOSTIC_ID = "HUT0001";
	
	private const string CATEGORY = "Usage";

	public override void Initialize(AnalysisContext context)
	{
		// Don't analyze generated code.
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
		// Enable concurrent execution.
		context.EnableConcurrentExecution();

		context.RegisterSymbolAction(AnalyzeSymbol, symbolKinds);
	}

	private static void AnalyzeSymbol(SymbolAnalysisContext context)
	{
		if (!context.Symbol.TryGetAttribute("Hertzole.UnityToolbox.GenerateLoadAttribute", out AttributeData? attribute) || attribute is null || attribute.AttributeClass is null)
		{
			return;
		}
		
		ITypeSymbol? typeSymbol = context.Symbol switch
		{
			IFieldSymbol fieldSymbol => fieldSymbol.Type,
			IPropertySymbol propertySymbol => propertySymbol.Type,
			_ => null
		};

		if (typeSymbol is null)
		{
			return;
		}

		Log.LogInfo($"Get field addressable type: {context.Symbol.Name}");
		bool isAddressable = AddressablesHelper.TryGetAddressableType(typeSymbol, out _);

		if (!isAddressable)
		{
			context.ReportDiagnostic(Diagnostic.Create(rule, attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation(), context.Symbol.Name));
		}
	}
}