#if FISHNET
using FishNet.Object;

namespace Hertzole.UnityToolbox
{
	public partial class NetworkObjects : NetworkBehaviour
	{
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