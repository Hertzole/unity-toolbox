#if TOOLBOX_PHYSICS_3D
using System;
using UnityEngine;
#if TOOLBOX_CECIL_ATTRIBUTES
using Hertzole.CecilAttributes;
#endif
#if TOOLBOX_SCRIPTABLE_VALUES
using Hertzole.ScriptableValues;
#endif

namespace Hertzole.UnityToolbox
{
	public abstract class ObjectDetector<T> : MonoBehaviour
	{
#if TOOLBOX_SCRIPTABLE_VALUES
		[SerializeField]
		private ScriptableValue<T> currentScriptableTarget = default;
#endif
		[SerializeField]
#if TOOLBOX_CECIL_ATTRIBUTES
		// [Required]
#endif
		private Transform origin = default;
		[SerializeField]
		private float range = default;
		[SerializeField]
		private LayerMask detectMask = default;

		private int? previousColliderId;

		private T currentTarget;

		public T CurrentTarget
		{
			get { return currentTarget; }
			private set
			{
				T previousTarget = currentTarget;
				currentTarget = value;
#if TOOLBOX_SCRIPTABLE_VALUES
				if (currentScriptableTarget != null)
				{
					currentScriptableTarget.SetEqualityCheck = false;
					currentScriptableTarget.Value = value;
					currentScriptableTarget.SetEqualityCheck = true;
				}
#endif
				
				OnTargetChanged?.Invoke(previousTarget, currentTarget);
			}
		}
		
		public event Action<T, T> OnTargetChanged; 

		protected virtual void Update()
		{
			Detect();
		}

		protected void Detect()
		{
			DetectStatus detectStatus = Detector.DetectObject(origin.position, origin.forward, range, detectMask, QueryTriggerInteraction.Ignore, ref previousColliderId, out T target);
			switch (detectStatus)
			{
				case DetectStatus.NoObject:
					CurrentTarget = default;
					break;
				case DetectStatus.NewObject:
					CurrentTarget = target;
					break;
				case DetectStatus.SameObject:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
#endif