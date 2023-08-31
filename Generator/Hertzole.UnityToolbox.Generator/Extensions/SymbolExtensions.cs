using System.Collections.Immutable;
using System.Text;
using Hertzole.UnityToolbox.Generator.Pooling;
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

			if (attributeData.AttributeClass.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) ==
			    attributeSymbol.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) &&
			    attributeData.AttributeClass.Name == attributeSymbol.Name)
			{
				attribute = attributeData;
				Log.LogInfo("Found attribute.");
				return true;
			}
		}

		return false;
	}

	public static string GetGenericFriendlyName(this INamedTypeSymbol symbol)
	{
		using (ObjectPool<StringBuilder>.Get(out StringBuilder? nameBuilder))
		{
			nameBuilder.Clear();

			nameBuilder.Append(symbol.Name);

			if (symbol.IsGenericType)
			{
				nameBuilder.Append('<');

				ImmutableArray<ITypeSymbol> typeArguments = symbol.TypeArguments;

				for (int i = 0; i < typeArguments.Length; i++)
				{
					if (i > 0)
					{
						nameBuilder.Append(", ");
					}

					nameBuilder.Append(typeArguments[i].Name);
				}

				nameBuilder.Append('>');
			}

			return nameBuilder.ToString();
		}
	}
}