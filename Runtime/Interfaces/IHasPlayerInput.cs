#if TOOLBOX_INPUT_SYSTEM
using UnityEngine.InputSystem;

namespace Hertzole.UnityToolbox.Interfaces
{
	public interface IHasPlayerInput
	{
		void EnableInput(PlayerInput input);

		void DisableInput(PlayerInput input);
	}
}
#endif