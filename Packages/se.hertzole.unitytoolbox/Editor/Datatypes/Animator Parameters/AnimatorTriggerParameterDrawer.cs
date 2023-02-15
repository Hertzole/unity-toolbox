#if TOOLBOX_ANIMATION
using UnityEditor;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	[CustomPropertyDrawer(typeof(AnimatorTriggerParameter))]
	public sealed class AnimatorTriggerParameterDrawer : AnimatorParameterDrawer
	{
		public override AnimatorControllerParameterType ParameterType { get { return AnimatorControllerParameterType.Trigger; } }
	}
}
#endif