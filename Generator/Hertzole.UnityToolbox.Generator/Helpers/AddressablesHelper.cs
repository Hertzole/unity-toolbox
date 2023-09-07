using System;
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
	
	public static INamedTypeSymbol? GetAddressableType(ITypeSymbol type)
	{
		Log.LogInfo($"Getting addressable type {type}.");
		
		if (type is INamedTypeSymbol namedType && namedType.IsGenericType)
		{
			if (namedType.TypeArguments.Length != 1)
			{
				return null;
			}

			return namedType.TypeArguments[0] as INamedTypeSymbol;
		}

		if (type.BaseType != null && string.Equals(type.BaseType.ConstructedFrom.ToDisplayString(), ReferenceSymbols.ASSET_REFERENCE_T, StringComparison.Ordinal))
		{
			return type.BaseType.TypeArguments[0] as INamedTypeSymbol;
		}

		return null;
	}
}