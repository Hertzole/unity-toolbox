#if TOOLBOX_SCRIPTABLE_VALUES && TOOLBOX_INPUT_SYSTEM && (TOOLBOX_MIRAGE || FISHNET)
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	public sealed partial class PlayerNetworkInput
	{
		[SerializeField]
		private ScriptablePlayerInputsList inputsList = default;

		private IHasPlayerInput[] inputs;
	}
}
#endif