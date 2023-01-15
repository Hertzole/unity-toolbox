#if TOOLBOX_MIRAGE
using System;
using Mirage;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	public sealed class NetworkObjects : NetworkBehaviour
	{
		public enum NetworkOwner
		{
			Server = 1,
			OtherClient = 2,
			LocalPlayer = 3
		}

		[SerializeField]
		private NetworkedObject[] objects = default;

		private void Awake()
		{
			Identity.OnStartServer.AddListener(UpdateObjects);
			Identity.OnStopServer.AddListener(UpdateObjects);
			Identity.OnStartClient.AddListener(UpdateObjects);
			Identity.OnStopClient.AddListener(UpdateObjects);
			Identity.OnAuthorityChanged.AddListener(OnAuthorityChanged);
		}

		private void OnDestroy()
		{
			Identity.OnStartServer.RemoveListener(UpdateObjects);
			Identity.OnStopServer.RemoveListener(UpdateObjects);
			Identity.OnStartClient.RemoveListener(UpdateObjects);
			Identity.OnStopClient.RemoveListener(UpdateObjects);
			Identity.OnAuthorityChanged.RemoveListener(OnAuthorityChanged);
		}

		private void OnAuthorityChanged(bool hasAuthority)
		{
			UpdateObjects();
		}

		private void UpdateObjects()
		{
			foreach (NetworkedObject networkedObject in objects)
			{
				if (networkedObject.TargetComponent != null)
				{
					networkedObject.TargetComponent.enabled = ShouldEnable(networkedObject.EnableIf);
				}
				else
				{
					networkedObject.TargetObject.SetActive(ShouldEnable(networkedObject.EnableIf));
				}
			}
		}

		private bool ShouldEnable(NetworkOwner enableIf)
		{
			switch (enableIf)
			{
				case NetworkOwner.Server:
					return IsServer;
				case NetworkOwner.OtherClient:
					return !HasAuthority;
				case NetworkOwner.LocalPlayer:
					return HasAuthority || IsLocalPlayer;
				default:
					throw new ArgumentOutOfRangeException(nameof(enableIf), enableIf, null);
			}
		}

		[Serializable]
		public class NetworkedObject
		{
			[SerializeField]
			private GameObject targetObject = default;
			[SerializeField]
			private Behaviour targetComponent = default;
			[SerializeField]
			private NetworkOwner enableIf = default;

			public GameObject TargetObject { get { return targetObject; } set { targetObject = value; } }
			public Behaviour TargetComponent { get { return targetComponent; } set { targetComponent = value; } }
			public NetworkOwner EnableIf { get { return enableIf; } set { enableIf = value; } }
		}
	}
}
#endif