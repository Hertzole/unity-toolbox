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
				RemoveInputs();
			}
		}

		private void OnAuthorityChanged(bool hasAuthority)
		{
			if (hasAuthority)
			{
				inputs ??= GetComponentsInChildren<IHasPlayerInput>();

#if TOOLBOX_ADDRESSABLES
				if (useAddressable)
				{
					LoadInputs();
				}
				else
#endif
				{
					AddInputs();
				}
			}
			else
			{
				RemoveInputs();
			}
		}
	}
}
#endif