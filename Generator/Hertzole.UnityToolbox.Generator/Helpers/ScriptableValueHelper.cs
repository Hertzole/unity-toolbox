using Microsoft.CodeAnalysis;

namespace Hertzole.UnityToolbox.Generator.Helpers;

public enum ScriptableType
{
	None = 0,
	Event = 1,
	GenericEvent = 2,
	Value = 3
}

public static class ScriptableValueHelper
{
	public static bool TryGetScriptableType(INamedTypeSymbol? type,
		out ScriptableType scriptableType,
		out ITypeSymbol? genericType,
		ReferenceSymbols referenceSymbols)
	{
		genericType = null;

		if (type != null)
		{
			if (TryGetScriptableTypeFromType(type, out scriptableType, out genericType, referenceSymbols))
			{
				return true;
			}

			if (TryGetScriptableTypeFromAddressable(type, out scriptableType, out genericType, referenceSymbols))
			{
				return true;
			}
		}

		scriptableType = ScriptableType.None;
		return false;
	}

	private static bool TryGetScriptableTypeFromType(INamedTypeSymbol type,
		out ScriptableType scriptableType,
		out ITypeSymbol? genericType,
		ReferenceSymbols referenceSymbols)
	{
		if (type.ConstructedFrom.Equals(referenceSymbols.ScriptableValue, SymbolEqualityComparer.Default))
		{
			genericType = type.TypeArguments[0];
			scriptableType = ScriptableType.Value;
			return true;
		}

		if (type.ConstructedFrom.Equals(referenceSymbols.ScriptableEvent, SymbolEqualityComparer.Default))
		{
			genericType = referenceSymbols.EventArgs;
			scriptableType = ScriptableType.Event;
			return true;
		}

		if (type.ConstructedFrom.Equals(referenceSymbols.ScriptableGenericEvent, SymbolEqualityComparer.IncludeNullability))
		{
			genericType = type.TypeArguments[0];
			scriptableType = ScriptableType.GenericEvent;
			return true;
		}

		if (type.BaseType != null)
		{
			if (type.BaseType.ConstructedFrom.Equals(referenceSymbols.ScriptableValue, SymbolEqualityComparer.Default))
			{
				genericType = type.BaseType.TypeArguments[0];
				scriptableType = ScriptableType.Value;
				return true;
			}

			if (type.BaseType.ConstructedFrom.Equals(referenceSymbols.ScriptableEvent, SymbolEqualityComparer.Default))
			{
				scriptableType = ScriptableType.Event;
				genericType = null;
				return true;
			}

			if (type.BaseType.ConstructedFrom.Equals(referenceSymbols.ScriptableGenericEvent, SymbolEqualityComparer.IncludeNullability))
			{
				genericType = type.BaseType.TypeArguments[0];
				scriptableType = ScriptableType.GenericEvent;
				return true;
			}
		}

		scriptableType = ScriptableType.None;
		genericType = null;
		return false;
	}

	private static bool TryGetScriptableTypeFromAddressable(INamedTypeSymbol type,
		out ScriptableType scriptableType,
		out ITypeSymbol? genericType,
		ReferenceSymbols referenceSymbols)
	{
		while (true)
		{
			if (type.ConstructedFrom.Equals(referenceSymbols.AssetReferenceT, SymbolEqualityComparer.Default))
			{
				if (type.TypeArguments[0] is not INamedTypeSymbol namedTypeSymbol)
				{
					scriptableType = ScriptableType.None;
					genericType = null;
					return false;
				}

				return TryGetScriptableTypeFromType(namedTypeSymbol, out scriptableType, out genericType, referenceSymbols);
			}

			if (type.BaseType != null)
			{
				type = type.BaseType;
				continue;
			}

			scriptableType = ScriptableType.None;
			genericType = null;
			return false;
		}
	}
}