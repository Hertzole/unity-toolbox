using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
#if TOOLBOX_SCRIPTABLE_VALUES && TOOLBOX_ADDRESSABLES
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#endif

namespace Hertzole.UnityToolbox
{
#if UNITY_EDITOR
	[AddComponentMenu("Toolbox/Spawnpoint")]
#endif
	public class Spawnpoint : MonoBehaviour
	{
		[SerializeField]
		private Vector3[] possibleRotations = default;
		[SerializeField]
		private Vector3 position = default;
		[SerializeField]
		private Vector3 size = new Vector3(1f, 2f, 1f);
		[SerializeField]
		private Vector3 offset = new Vector3(0f, 1f, 0f);

		private bool forceUpdateCorners;

#if TOOLBOX_SCRIPTABLE_VALUES
		private bool hasAddedToList;
#endif

#if TOOLBOX_ADDRESSABLES && TOOLBOX_SCRIPTABLE_VALUES
		private AsyncOperationHandle<ScriptableSpawnpointsList>? spawnpointsListHandle;
#endif

		private Vector3 previousPosition;
		private Vector3[] corners;

		public Vector3 Position
		{
			get { return position; }
			set { position = value; }
		}
		public Vector3 Size
		{
			get { return size; }
			set
			{
				if (size != value)
				{
					size = value;
					forceUpdateCorners = true;
				}
			}
		}
		public Vector3 Offset
		{
			get { return offset; }
			set
			{
				if (offset != value)
				{
					offset = value;
					forceUpdateCorners = true;
				}
			}
		}

		public IReadOnlyList<Vector3> PossibleRotations
		{
			get { return possibleRotations; }
		}
		public IReadOnlyList<Vector3> Corners
		{
			get
			{
				UpdateCornersIfNeeded();

				return corners;
			}
		}

		public const int CORNERS_COUNT = 8;

		public Vector3 GetRandomRotation()
		{
			return possibleRotations.IsNullOrEmpty() ? Vector3.zero : possibleRotations[UnityEngine.Random.Range(0, possibleRotations.Length)];
		}

		public Vector3 GetRandomRotation(Random random)
		{
			return possibleRotations.IsNullOrEmpty() ? Vector3.zero : possibleRotations[random.Next(0, possibleRotations.Length)];
		}

		public Vector3 GetRandomRotation(ref Unity.Mathematics.Random random)
		{
			return possibleRotations.IsNullOrEmpty() ? Vector3.zero : possibleRotations[random.NextInt(0, possibleRotations.Length)];
		}

		private void UpdateCornersIfNeeded()
		{
			if (forceUpdateCorners || corners == null || transform.position != previousPosition)
			{
				UpdateCorners();
				previousPosition = transform.position;
			}
		}

		private void UpdateCorners()
		{
			if (corners == null || corners.Length != CORNERS_COUNT)
			{
				corners = new Vector3[CORNERS_COUNT];
			}

			Vector3 pos = transform.position;

			Vector3 center = new Vector3(pos.x + offset.x, pos.y + offset.y, pos.z + offset.z);
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
#if TOOLBOX_SCRIPTABLE_VALUES
#if TOOLBOX_ADDRESSABLES
		[SerializeField]
		private bool useAddressables = false;
		[SerializeField]
		private AssetReferenceT<ScriptableSpawnpointsList> spawnpointsListReference = default;
#endif
		[SerializeField]
		private ScriptableSpawnpointsList spawnpointsList = default;
#endif

#if TOOLBOX_SCRIPTABLE_VALUES
#if TOOLBOX_ADDRESSABLES
		private void Awake()
		{
			if (useAddressables)
			{
				spawnpointsListHandle = spawnpointsListReference.LoadAsync();
				spawnpointsListHandle.Value.Completed += handle =>
				{
					if (handle.Status == AsyncOperationStatus.Succeeded)
					{
						spawnpointsList = handle.Result;

						if (spawnpointsList != null && !hasAddedToList)
						{
							spawnpointsList.Add(this);
							hasAddedToList = true;
						}
					}
				};
			}
		}

		private void OnDestroy()
		{
			spawnpointsListHandle.Release();
		}
#endif

		private void OnEnable()
		{
			if (spawnpointsList != null && !hasAddedToList)
			{
				spawnpointsList.Add(this);
				hasAddedToList = true;
			}
		}

		private void OnDisable()
		{
			if (spawnpointsList != null && hasAddedToList)
			{
				spawnpointsList.Remove(this);
				hasAddedToList = false;
			}
		}
#endif

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Vector3 pos = transform.position;

			Color oldColor = Gizmos.color;
			Gizmos.color = new Color(boxColor.r, boxColor.g, boxColor.b, 0.5f);
			Vector3 center = new Vector3(pos.x + offset.x, pos.y + offset.y, pos.z + offset.z);
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

			Vector3 origin = pos + position;

			const float arrow_size = 0.2f;

			Gizmos.color = Color.green;
			Gizmos.DrawLine(origin - new Vector3(0, arrow_size, 0), origin + new Vector3(0, arrow_size, 0));
			Gizmos.color = Color.red;
			Gizmos.DrawLine(origin - new Vector3(arrow_size, 0, 0), origin + new Vector3(arrow_size, 0, 0));
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(origin - new Vector3(0, 0, arrow_size), origin + new Vector3(0, 0, arrow_size));

			Gizmos.color = oldColor;
		}

		[Header("Gizmo Settings")]
		[SerializeField]
		private Color boxColor = Color.white;
		[SerializeField]
		private Color directionColor = Color.blue;

#if TOOLBOX_ADDRESSABLES && TOOLBOX_SCRIPTABLE_VALUES
		private void OnValidate()
		{
			if (useAddressables)
			{
				spawnpointsList = null;
			}

			forceUpdateCorners = true;
		}
#endif
#endif
	}
}