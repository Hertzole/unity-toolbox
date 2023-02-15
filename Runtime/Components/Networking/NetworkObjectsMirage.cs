#if TOOLBOX_MIRAGE
using Mirage;

namespace Hertzole.UnityToolbox
{
	public partial class NetworkObjects : NetworkBehavior
	{
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