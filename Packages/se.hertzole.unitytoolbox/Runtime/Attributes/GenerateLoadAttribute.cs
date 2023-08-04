#if TOOLBOX_ADDRESSABLES
using System;

namespace Hertzole.UnityToolbox
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class GenerateLoadAttribute : Attribute { }
}
#endif