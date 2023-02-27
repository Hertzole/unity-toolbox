#if TOOLBOX_PHYSICS_3D && (TOOLBOX_MIRAGE || FISHNET)
#if TOOLBOX_CECIL_ATTRIBUTES
using Hertzole.CecilAttributes;
#endif
#if TOOLBOX_SCRIPTABLE_VALUES
using AuroraPunks.ScriptableValues;
#endif
using System;
using UnityEngine;
#if TOOLBOX_MIRAGE
using Mirage;
#elif FISHNET
using FishNet.Object;
#endif

namespace Hertzole.UnityToolbox
{
	public abstract class NetworkedObjectDetector<T> : NetworkBehaviour
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
			protected set
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

		private bool CanRunUpdate
		{
			get
			{
#if TOOLBOX_MIRAGE
				return HasAuthority;
#elif FISHNET
				return IsOwner;
#else
				return false;
#endif
			}
		}
		
		public event Action<T, T> OnTargetChanged; 

		protected virtual void Update()
		{
			if (!CanRunUpdate)
			{
				return;
			}
			
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