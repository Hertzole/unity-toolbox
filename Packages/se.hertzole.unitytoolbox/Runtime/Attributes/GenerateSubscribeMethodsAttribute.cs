#if TOOLBOX_SCRIPTABLE_VALUES || (TOOLBOX_SCRIPTABLE_VALUES_2 && UNITY_EDITOR)
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
#endif