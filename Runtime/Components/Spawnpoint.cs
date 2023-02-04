using UnityEngine;

namespace Hertzole.UnityToolbox
{
#if UNITY_EDITOR
	[AddComponentMenu("Toolbox/Spawnpoint")]
#endif
	public class Spawnpoint : MonoBehaviour
#if UNITY_EDITOR
		, ISerializationCallbackReceiver
#endif
	{
		[SerializeField]
		private Vector3[] possibleRotations = default;
		[SerializeField]
		private Vector3 size = new Vector3(0.8f, 2f, 0.8f);
#if TOOLBOX_SCRIPTABLE_VALUES
		[SerializeField]
		private ScriptableSpawnpointsList spawnpointsList = default;
#endif
		[SerializeField]
		[HideInInspector]
		private Vector3[] corners = default;

		public const int CORNERS_COUNT = 8;

		public Vector3 GetRandomRotation()
		{
			if (possibleRotations == null || possibleRotations.Length == 0)
			{
				return Vector3.zero;
			}

			return possibleRotations[Random.Range(0, possibleRotations.Length)];
		}

		public Vector3 GetCorner(int index)
		{
			return corners[index];
		}

#if TOOLBOX_SCRIPTABLE_VALUES
		private void OnEnable()
		{
			if (spawnpointsList != null)
			{
				spawnpointsList.Add(this);
			}
		}

		private void OnDisable()
		{
			if (spawnpointsList != null)
			{
				spawnpointsList.Remove(this);
			}
		}
#endif

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Vector3 position = transform.position;

			Color oldColor = Gizmos.color;
			Gizmos.color = new Color(boxColor.r, boxColor.g, boxColor.b, 0.5f);
			Vector3 center = new Vector3(position.x, position.y + size.y / 2f, position.z);
			Gizmos.DrawCube(center, size);

			Gizmos.color = new Color(boxColor.r, boxColor.g, boxColor.b, 1f);
			Gizmos.DrawWireCube(center, size);

			Gizmos.color = directionColor;
			if (possibleRotations != null)
			{
				for (int i = 0; i < possibleRotations.Length; i++)
				{
					Vector3 direction = Quaternion.Euler(possibleRotations[i]) * Vector3.forward;
					Gizmos.DrawRay(center, direction);
				}
			}

			Gizmos.color = oldColor;
		}

		private void OnDrawGizmosSelected()
		{
			Color oldColor = Gizmos.color;

			// Draw corners
			Gizmos.color = Color.yellow;
			for (int i = 0; i < corners.Length; i++)
			{
				Gizmos.DrawSphere(corners[i], 0.1f);
			}

			Gizmos.color = oldColor;
		}

		[Header("Gizmo Settings")]
		[SerializeField]
		private Color boxColor = Color.white;
		[SerializeField]
		private Color directionColor = Color.blue;

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (corners == null || corners.Length != CORNERS_COUNT)
			{
				corners = new Vector3[CORNERS_COUNT];
			}

			Vector3 position = transform.position;

			Vector3 center = new Vector3(position.x, position.y + size.y / 2f, position.z);
			Vector3 halfSize = size / 2f;
			corners[0] = center + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z);
			corners[1] = center + new Vector3(-halfSize.x, -halfSize.y, halfSize.z);
			corners[2] = center + new Vector3(-halfSize.x, halfSize.y, -halfSize.z);
			corners[3] = center + new Vector3(-halfSize.x, halfSize.y, halfSize.z);
			corners[4] = center + new Vector3(halfSize.x, -halfSize.y, -halfSize.z);
			corners[5] = center + new Vector3(halfSize.x, -halfSize.y, halfSize.z);
			corners[6] = center + new Vector3(halfSize.x, halfSize.y, -halfSize.z);
			corners[7] = center + new Vector3(halfSize.x, halfSize.y, halfSize.z);
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			// Does nothing.
		}
#endif
	}
}