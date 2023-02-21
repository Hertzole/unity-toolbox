#if FISHNET
using FishNet.Object;

namespace Hertzole.UnityToolbox
{
	public partial class NetworkObjects : NetworkBehaviour
	{
		private void Awake()
		{
			for (int i = 0; i < objects.Length; i++)
			{
				if (objects[i].TargetComponent != null)
				{
					objects[i].TargetComponent.enabled = false;
				}
				else
				{
					objects[i].TargetObject.SetActive(false);
				}
			}
		}

		public override void OnStartClient()
		{
			base.OnStartClient();

			UpdateObjects();
		}

		private partial bool AmIServer()
		{
			return IsServer;
		}

		private partial bool IsMe()
		{
			return IsOwner;
		}
	}
}
#endif