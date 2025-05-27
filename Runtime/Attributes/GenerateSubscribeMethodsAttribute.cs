// This mess is to ensure that the attribute is only included when the ScriptableValues < v2.0 package is used.
// If it's ScriptableValues 2.0 or higher, the attribute is obsolete and should not be used. Only include this file if it's in the unity editor.
// Else if it's ScriptableValues < v2.0, include the attribute whatsoever.
#if TOOLBOX_SCRIPTABLE_VALUES_2
#if UNITY_EDITOR
#define TOOLBOX_INCLUDE_THIS_FILE
#endif // UNITY_EDITOR
#elif TOOLBOX_SCRIPTABLE_VALUES
#define TOOLBOX_INCLUDE_THIS_FILE
#endif // TOOLBOX_SCRIPTABLE_VALUES_2

#if TOOLBOX_INCLUDE_THIS_FILE
using System;

namespace Hertzole.UnityToolbox
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
#if TOOLBOX_SCRIPTABLE_VALUES_2
	[Obsolete("Use built-in attributes from ScriptableValues 2.0+ instead.", true)]
#endif
	public sealed class GenerateSubscribeMethodsAttribute : Attribute
	{
		public GenerateSubscribeMethodsAttribute() { }

		public GenerateSubscribeMethodsAttribute(bool subscribeToChanging) { }
	}
}
#endif // TOOLBOX_INCLUDE_THIS_FILE