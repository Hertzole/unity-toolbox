#if TOOLBOX_ANIMATION
using UnityEditor;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	[CustomPropertyDrawer(typeof(AnimatorFloatParameter))]
	public sealed class AnimatorFloatParameterDrawer : AnimatorParameterDrawer
	{
		public override AnimatorControllerParameterType ParameterType { get { return AnimatorControllerParameterType.Float; } }
	}
}
#endif