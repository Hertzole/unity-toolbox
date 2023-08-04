#if TOOLBOX_SCRIPTABLE_VALUES
using System;

namespace Hertzole.UnityToolbox
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class GenerateSubscribeMethodsAttribute : Attribute { }
}
#endif