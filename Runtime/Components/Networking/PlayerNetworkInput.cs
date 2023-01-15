#if TOOLBOX_SCRIPTABLE_VALUES && TOOLBOX_MIRAGE && TOOLBOX_INPUT_SYSTEM
using Hertzole.UnityToolbox.Interfaces;
using Mirage;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	public sealed class PlayerNetworkInput : NetworkBehaviour
	{
		[SerializeField]
		private ScriptablePlayerInputsList inputsList = default;

		private IHasPlayerInput[] inputs;

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