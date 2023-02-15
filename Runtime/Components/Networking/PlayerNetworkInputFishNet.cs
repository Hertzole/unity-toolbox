#if TOOLBOX_INPUT_SYSTEM && TOOLBOX_SCRIPTABLE_VALUES && FISHNET
using FishNet.Object;

namespace Hertzole.UnityToolbox
{
	public partial class PlayerNetworkInput : NetworkBehaviour
	{
		private void OnDisable()
		{
			if (IsOwner)
			{
				foreach (IHasPlayerInput input in inputs)
				{
					inputsList.Remove(input);
				}
			}
		}

		public override void OnStartClient()
		{
			base.OnStartClient();

			inputs ??= GetComponentsInChildren<IHasPlayerInput>();

			foreach (IHasPlayerInput input in inputs)
			{
				inputsList.Add(input);
			}
		}

		public override void OnStopClient()
		{
			base.OnStopClient();

			if (IsOwner)
			{
				foreach (IHasPlayerInput input in inputs)
				{
					inputsList.Remove(input);
				}
			}
		}
	}
}
#endif