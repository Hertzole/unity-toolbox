using UnityEngine.InputSystem;

namespace Hertzole.UnityToolbox.Lab
{
	public partial class InputCallbacks
	{
		[GenerateInputCallbacks(nameof(playerInput), GenerateStarted = true, GeneratePerformed = true, GenerateCanceled = true, GenerateAll = true)]
		public InputActionReference move;

		private PlayerInput playerInput;

		private partial void OnInputMoveStarted(InputAction.CallbackContext context)
		{
			throw new System.NotImplementedException();
		}

		private partial void OnInputMovePerformed(InputAction.CallbackContext context)
		{
			throw new System.NotImplementedException();
		}

		private partial void OnInputMoveCanceled(InputAction.CallbackContext context)
		{
			throw new System.NotImplementedException();
		}

		private partial void OnInputMove(InputAction.CallbackContext context)
		{
			throw new System.NotImplementedException();
		}
	}
}