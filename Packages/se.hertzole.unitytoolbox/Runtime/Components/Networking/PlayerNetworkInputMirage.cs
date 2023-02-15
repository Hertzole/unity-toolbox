#if TOOLBOX_SCRIPTABLE_VALUES && TOOLBOX_INPUT_SYSTEM && TOOLBOX_MIRAGE
using Mirage;

namespace Hertzole.UnityToolbox
{
	public partial class PlayerNetworkInput : NetworkBehaviour
	{
		private void Awake()
		{
			Identity.OnAuthorityChanged.AddListener(OnAuthorityChanged);
		}

		private void OnDisable()
		{
			if (HasAuthority)
			{
				foreach (IHasPlayerInput input in inputs)
				{
					inputsList.Remove(input);
				}
			}
		}

		private void OnAuthorityChanged(bool hasAuthority)
		{
			if (hasAuthority)
			{
				inputs ??= GetComponentsInChildren<IHasPlayerInput>();

				foreach (IHasPlayerInput input in inputs)
				{
					inputsList.Add(input);
				}
			}
			else
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