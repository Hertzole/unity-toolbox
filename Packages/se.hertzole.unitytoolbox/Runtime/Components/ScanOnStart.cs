#if TOOLBOX_NAVIGATION
using Unity.AI.Navigation;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
#if UNITY_EDITOR
	[RequireComponent(typeof(NavMeshSurface))]
#endif
	public sealed class ScanOnStart : MonoBehaviour
#if UNITY_EDITOR
		, ISerializationCallbackReceiver
#endif
	{
		[SerializeField]
		[HideInInspector]
		private NavMeshSurface surface = null;

		private void Start()
		{
			surface.BuildNavMesh();
		}

#if UNITY_EDITOR
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (surface == null)
			{
				surface = GetComponent<NavMeshSurface>();
			}
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize() { }
#endif
	}
}
#endif