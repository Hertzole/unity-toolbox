#if TOOLBOX_MIRAGE || FISHNET
using System;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	public sealed partial class NetworkObjects
	{
		public enum NetworkOwner
		{
			Server = 1,
			OtherClient = 2,
			LocalPlayer = 3
		}

		[SerializeField]
		private NetworkedObject[] objects = default;

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
					return AmIServer();
				case NetworkOwner.OtherClient:
					return !IsMe();
				case NetworkOwner.LocalPlayer:
					return IsMe();
				default:
					throw new ArgumentOutOfRangeException(nameof(enableIf), enableIf, null);
			}
		}

		private partial bool AmIServer();

		private partial bool IsMe();

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