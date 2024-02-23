#if TOOLBOX_SCRIPTABLE_VALUES
using System;

namespace Hertzole.UnityToolbox
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public sealed class GenerateSubscribeMethodsAttribute : Attribute { }
}
#endif