#if TOOLBOX_ANIMATION
using UnityEditor;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	[CustomPropertyDrawer(typeof(AnimatorBoolParameter))]
	public sealed class AnimatorBoolParameterDrawer : AnimatorParameterDrawer
	{
		public override AnimatorControllerParameterType ParameterType { get { return AnimatorControllerParameterType.Bool; } }
	}
}
#endif