#if TOOLBOX_MIRAGE
using Mirage;

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

		private partial bool AmIServer()
		{
			return IsServer;
		}

		private partial bool IsMe()
		{
			return HasAuthority || IsLocalPlayer;
		}
	}
}
#endif