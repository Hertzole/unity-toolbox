#if TOOLBOX_ANIMATION
using UnityEditor;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	[CustomPropertyDrawer(typeof(AnimatorIntParameter))]
	public sealed class AnimatorIntParameterDrawer : AnimatorParameterDrawer
	{
		public override AnimatorControllerParameterType ParameterType { get { return AnimatorControllerParameterType.Int; } }
	}
}
#endif