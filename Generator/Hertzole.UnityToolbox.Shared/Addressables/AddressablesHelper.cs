using System;
using Microsoft.CodeAnalysis;

namespace Hertzole.UnityToolbox.Shared;

public static class AddressablesHelper
{
	//TODO: Mark as obsolete and use TryGetAddressableType instead.
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

	//TODO: Mark as obsolete and use TryGetAddressableType instead.
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

		if (type.BaseType != null &&
		    string.Equals(type.BaseType.ConstructedFrom.ToDisplayString(), "UnityEngine.AddressableAssets.AssetReferenceT`1", StringComparison.Ordinal))
		{
			return type.BaseType.TypeArguments[0] as INamedTypeSymbol;
		}

		return null;
	}

	public static bool TryGetAddressableType(ITypeSymbol type, out INamedTypeSymbol? addressableType)
	{
		// Used to check if it's just a AssetReference type.
		// Not entirely sure what to do about the addressable type.
		if (type is INamedTypeSymbol namedType && IsAddressableType(namedType))
		{
			addressableType = null;
			return true;
		}

		if (type is INamedTypeSymbol genericNamedType && genericNamedType.IsGenericType && genericNamedType.TypeArguments.Length == 1 &&
		    IsAddressableType(genericNamedType) && genericNamedType.TypeArguments[0] is INamedTypeSymbol namedTypeSymbol)
		{
			addressableType = namedTypeSymbol;
			return true;
		}

		INamedTypeSymbol? baseType = type as INamedTypeSymbol;

		while (baseType != null)
		{
			if (baseType.IsGenericType && baseType.TypeArguments.Length == 1 && baseType.TypeArguments[0] is INamedTypeSymbol baseTypeSymbol &&
			    IsAddressableType(baseType))
			{
				addressableType = baseTypeSymbol;
				return true;
			}

			baseType = baseType.BaseType;
		}

		addressableType = null;
		return false;
	}

	private static bool IsAddressableType(INamedTypeSymbol symbol)
	{
		Log.LogInfo("IsAddressableType: " + symbol.ConstructedFrom.ToDisplayString());

		return string.Equals(symbol.ConstructedFrom.ToDisplayString(), "UnityEngine.AddressableAssets.AssetReferenceT<TObject>", StringComparison.Ordinal) ||
		       string.Equals(symbol.ConstructedFrom.ToDisplayString(), "UnityEngine.AddressableAssets.AssetReference", StringComparison.Ordinal);
	}
}