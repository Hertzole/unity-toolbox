#if TOOLBOX_ADDRESSABLES
using System;

namespace Hertzole.UnityToolbox
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public sealed class GenerateLoadAttribute : Attribute { }
}
#endif