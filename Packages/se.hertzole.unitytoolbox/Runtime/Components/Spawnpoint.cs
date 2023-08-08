using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = System.Random;
#if TOOLBOX_SCRIPTABLE_VALUES && TOOLBOX_ADDRESSABLES
using UnityEngine.AddressableAssets;
#endif

namespace Hertzole.UnityToolbox
{
	/// <summary>
	///     A spawnpoint that can be used to indicate where objects will spawn.
	/// </summary>
#if UNITY_EDITOR
	[AddComponentMenu("Toolbox/Spawnpoint")]
#endif
	public partial class Spawnpoint : MonoBehaviour
	{
		[SerializeField]
		[Tooltip("The possible rotations the object can have when spawned.")]
		private List<Vector3> possibleRotations = new List<Vector3>();
		[SerializeField]
		[Tooltip("The position the object will spawn at, relative to the spawnpoint.")]
		private Vector3 spawnPosition = default;
		[SerializeField]
		[Tooltip("The size of the spawn boundary box.")]
		private Vector3 size = new Vector3(1f, 2f, 1f);
		[SerializeField]
		[Tooltip("The offset of the spawn boundary box.")]
		private Vector3 offset = new Vector3(0f, 1f, 0f);

		private bool forceUpdateCorners;

#if TOOLBOX_SCRIPTABLE_VALUES
		private bool hasAddedToList;
#endif

		private Vector3 previousPosition;
		private Vector3[] corners;

		/// <summary>
		///     The position the object will spawn at, relative to the spawnpoint.
		/// </summary>
		public Vector3 SpawnPosition
		{
			get { return spawnPosition; }
			set { spawnPosition = value; }
		}

		/// <summary>
		///     The size of the spawn boundary box.
		/// </summary>
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

		/// <summary>
		///     The offset of the spawn boundary box.
		/// </summary>
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

		/// <summary>
		///     The world position the object will spawn at.
		/// </summary>
		public Vector3 Position
		{
			get { return transform.position + spawnPosition; }
		}

		/// <summary>
		///     The possible rotations the object can have when spawned.
		/// </summary>
		public List<Vector3> PossibleRotations
		{
			get { return possibleRotations; }
			set { possibleRotations = value; }
		}

		/// <summary>
		///     The corners of the spawn boundary box.
		/// </summary>
		public IReadOnlyList<Vector3> Corners
		{
			get
			{
				UpdateCornersIfNeeded();

				return corners;
			}
		}

		/// <summary>
		///     The amount of corners the spawn boundary box has.
		/// </summary>
		public const int CORNERS_COUNT = 8;

		/// <summary>
		///     Gets a random spawn rotation from the list of possible rotations.
		/// </summary>
		/// <returns>A random rotation from the list of possible rotations. Returns <c>Vector3.zero</c> if the list is empty.</returns>
		public Vector3 GetRandomRotation()
		{
			return possibleRotations.IsNullOrEmpty() ? Vector3.zero : possibleRotations[UnityEngine.Random.Range(0, possibleRotations.Count)];
		}

		/// <summary>
		///     Gets a random spawn rotation from the list of possible rotations.
		/// </summary>
		/// <param name="random">The random object to use for getting a random value.</param>
		/// <returns>A random rotation from the list of possible rotations. Returns <c>Vector3.zero</c> if the list is empty.</returns>
		public Vector3 GetRandomRotation(Random random)
		{
			return possibleRotations.IsNullOrEmpty() ? Vector3.zero : possibleRotations[random.Next(0, possibleRotations.Count)];
		}

		/// <summary>
		///     Gets a random spawn rotation from the list of possible rotations.
		/// </summary>
		/// <param name="random">The random object to use for getting a random value.</param>
		/// <returns>A random rotation from the list of possible rotations. Returns <c>Vector3.zero</c> if the list is empty.</returns>
		public Vector3 GetRandomRotation(ref Unity.Mathematics.Random random)
		{
			return possibleRotations.IsNullOrEmpty() ? Vector3.zero : possibleRotations[random.NextInt(0, possibleRotations.Count)];
		}

		/// <summary>
		///     Updates the spawn boundary box corners if needed.
		/// </summary>
		private void UpdateCornersIfNeeded()
		{
			if (forceUpdateCorners || corners == null || transform.position != previousPosition)
			{
				UpdateCorners();
				previousPosition = transform.position;
			}
		}

		/// <summary>
		///     Updates the spawn boundary box corners.
		/// </summary>
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
		[Tooltip("If true, the spawnpoints list will be loaded from addressables.")]
		private bool useAddressables = false;
		[SerializeField]
		[GenerateLoad]
		[Tooltip("The addressable reference to the spawnpoints list.")]
		private AssetReferenceT<ScriptableSpawnpointsList> spawnpointsListReference = default;
#endif
		[SerializeField]
		[CanBeNull]
		[Tooltip("The spawnpoints list to add to..")]
		private ScriptableSpawnpointsList spawnpointsList = default;

		/// <summary>
		///     The spawnpoints list to add to.
		/// </summary>
		[CanBeNull]
		public ScriptableSpawnpointsList SpawnpointsList
		{
			get { return spawnpointsList; }
			set
			{
				RemoveFromList();
				spawnpointsList = value;
				AddToList();
			}
		}
#endif

#if TOOLBOX_SCRIPTABLE_VALUES
#if TOOLBOX_ADDRESSABLES
		private void Awake()
		{
			if (useAddressables)
			{
				LoadAssets();
			}
		}

		private void OnDestroy()
		{
			ReleaseAssets();
		}

		/// <summary>
		///     Called when the spawnpoints list is loaded from addressables.
		/// </summary>
		/// <param name="value"></param>
		partial void OnSpawnpointsListLoaded(ScriptableSpawnpointsList value)
		{
			AddToList();
		}
#endif

		private void OnEnable()
		{
			AddToList();
		}

		private void OnDisable()
		{
			RemoveFromList();
		}

		/// <summary>
		///     Adds this spawnpoint to the spawnpoints list.
		/// </summary>
		private void AddToList()
		{
			if (spawnpointsList != null && !hasAddedToList)
			{
				spawnpointsList.Add(this);
				hasAddedToList = true;
			}
		}

		/// <summary>
		///     Removes this spawnpoint from the spawnpoints list.
		/// </summary>
		private void RemoveFromList()
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
			Gizmos.color = new Color(boxColor.r, boxColor.g, boxColor.b, boxColor.a * 0.5f);
			Vector3 center = new Vector3(pos.x + offset.x, pos.y + offset.y, pos.z + offset.z);
			Gizmos.DrawCube(center, size);

			Gizmos.color = new Color(boxColor.r, boxColor.g, boxColor.b, boxColor.a);
			Gizmos.DrawWireCube(center, size);

			Gizmos.color = directionColor;
			if (possibleRotations != null)
			{
				for (int i = 0; i < possibleRotations.Count; i++)
				{
					Vector3 direction = Quaternion.Euler(possibleRotations[i]) * Vector3.forward;
					Vector3 endPosition = center + direction;
					Gizmos.DrawLine(center, endPosition);
					Gizmos.DrawLine(endPosition, endPosition - Quaternion.Euler(possibleRotations[i] + new Vector3(0, 45, 0)) * Vector3.forward * 0.25f);
					Gizmos.DrawLine(endPosition, endPosition - Quaternion.Euler(possibleRotations[i] - new Vector3(0, 45, 0)) * Vector3.forward * 0.25f);
				}
			}

			Vector3 origin = pos + spawnPosition;

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
		[Tooltip("The color of the boundary box.")]
		private Color boxColor = Color.white;
		[SerializeField]
		[Tooltip("The color of the direction arrow.")]
		private Color directionColor = Color.blue;

#if TOOLBOX_ADDRESSABLES && TOOLBOX_SCRIPTABLE_VALUES
		private void OnValidate()
		{
			if (useAddressables)
			{
				spawnpointsList = null;
			}

			// If anything changes, force the corners to update.
			forceUpdateCorners = true;
		}
#endif
#endif
	}
}