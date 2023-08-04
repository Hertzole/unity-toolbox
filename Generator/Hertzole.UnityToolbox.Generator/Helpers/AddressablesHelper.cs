using Microsoft.CodeAnalysis;

namespace Hertzole.UnityToolbox.Generator.Helpers;

public static class AddressablesHelper
{
	public static INamedTypeSymbol? GetAddressableType(IFieldSymbol field, ISymbol assetReferenceT)
	{
		if (field.Type is INamedTypeSymbol namedType && namedType.IsGenericType)
		{
			if (namedType.TypeArguments.Length != 1)
			{
				return null;
			}

			return namedType.TypeArguments[0] as INamedTypeSymbol;
		}

		if (field.Type.BaseType != null && field.Type.BaseType.ConstructedFrom.Equals(assetReferenceT, SymbolEqualityComparer.Default))
		{
			return field.Type.BaseType.TypeArguments[0] as INamedTypeSymbol;
		}

		return null;
	}
}