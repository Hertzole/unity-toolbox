﻿using System;
using Microsoft.CodeAnalysis;

namespace Hertzole.UnityToolbox.Shared;

public enum ScriptableType
{
	None = 0,
	Event = 1,
	GenericEvent = 2,
	Value = 3
}

public static class ScriptableValueHelper
{
	public static bool TryGetScriptableType(INamedTypeSymbol? type, out ScriptableType scriptableType, out ITypeSymbol? genericType)
	{
		Log.LogInfo("Get scriptable type. (Type: " + type?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) + ")");
		
		genericType = null;

		if (type != null)
		{
			if (TryGetScriptableTypeFromType(type, out scriptableType, out genericType))
			{
				return true;
			}

			if (TryGetScriptableTypeFromAddressable(type, out scriptableType, out genericType))
			{
				return true;
			}
		}

		scriptableType = ScriptableType.None;
		return false;
	}

	private static bool TryGetScriptableTypeFromType(INamedTypeSymbol type, out ScriptableType scriptableType, out ITypeSymbol? genericType)
	{
		if (type.ConstructedFrom.StringEquals(Types.SCRIPTABLE_VALUE))
		{
			genericType = type.TypeArguments[0];
			scriptableType = ScriptableType.Value;
			return true;
		}

		if (type.ConstructedFrom.StringEquals(Types.SCRIPTABLE_EVENT))
		{
			genericType = null;
			scriptableType = ScriptableType.Event;
			return true;
		}

		if (type.ConstructedFrom.StringEquals(Types.GENERIC_SCRIPTABLE_EVENT))
		{
			genericType = type.TypeArguments[0];
			scriptableType = ScriptableType.GenericEvent;
			return true;
		}

		if (type.BaseType != null)
		{
			if (type.BaseType.ConstructedFrom.StringEquals(Types.SCRIPTABLE_VALUE))
			{
				genericType = type.BaseType.TypeArguments[0];
				scriptableType = ScriptableType.Value;
				return true;
			}

			if (type.BaseType.ConstructedFrom.StringEquals(Types.SCRIPTABLE_EVENT))
			{
				scriptableType = ScriptableType.Event;
				genericType = null;
				return true;
			}

			if (type.BaseType.ConstructedFrom.StringEquals(Types.GENERIC_SCRIPTABLE_EVENT))
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

	private static bool TryGetScriptableTypeFromAddressable(INamedTypeSymbol type, out ScriptableType scriptableType, out ITypeSymbol? genericType)
	{
		Log.LogInfo("Trying to get scriptable type from addressable. (Type: " + type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) + ")");
		
		while (true)
		{
			if (type.ConstructedFrom.StringEquals(Types.ASSET_REFERENCE_T))
			{
				if (type.TypeArguments[0] is not INamedTypeSymbol namedTypeSymbol)
				{
					scriptableType = ScriptableType.None;
					genericType = null;
					return false;
				}

				return TryGetScriptableTypeFromType(namedTypeSymbol, out scriptableType, out genericType);
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