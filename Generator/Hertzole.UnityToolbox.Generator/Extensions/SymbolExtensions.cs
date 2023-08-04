using Microsoft.CodeAnalysis;

namespace Hertzole.UnityToolbox.Generator;

public static class SymbolExtensions
{
	public static bool TryGetAttribute(this ISymbol symbol, INamedTypeSymbol attributeSymbol, out AttributeData? attribute)
	{
		attribute = null;
		foreach (AttributeData attributeData in symbol.GetAttributes())
		{
			if (attributeData.AttributeClass == null)
			{
				continue;
			}
			
			Log.LogInfo($"Checking attribute class {attributeData.AttributeClass} against {attributeSymbol}.");

			if (attributeData.AttributeClass.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == attributeSymbol.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) &&
			    attributeData.AttributeClass.Name == attributeSymbol.Name)
			{
				attribute = attributeData;
				Log.LogInfo("Found attribute.");
				return true;
			}
		}

		return false;
	}
}