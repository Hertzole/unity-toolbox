﻿using UnityEngine;

namespace Hertzole.UnityToolbox
{
	public enum DetectStatus
	{
		NoObject = 0,
		NewObject = 1,
		SameObject = 2
	}

	public static class Detector
	{
		public static DetectStatus DetectObject<T>(Vector3 origin, Vector3 direction, float range, int detectMask, QueryTriggerInteraction triggerInteraction, ref int? previousColliderId, out T target)
		{
			if (Physics.Raycast(origin, direction, out RaycastHit hit, range, detectMask, triggerInteraction))
			{
#if UNITY_EDITOR
				Debug.DrawRay(origin, direction * hit.distance, Color.green, 0);
#endif

				if (hit.colliderInstanceID != previousColliderId)
				{
					previousColliderId = hit.colliderInstanceID;
					if (hit.collider.TryGetComponentInParent(out target))
					{
						return DetectStatus.NewObject;
					}

					target = default;
					return DetectStatus.NoObject;
				}

				target = default;
				return DetectStatus.SameObject;
			}

#if UNITY_EDITOR
			Debug.DrawRay(origin, direction * range, Color.red, 0);
#endif

			if (previousColliderId != null)
			{
				previousColliderId = null;
				target = default;

				return DetectStatus.NoObject;
			}

			target = default;
			return DetectStatus.SameObject;
		}
	}
}