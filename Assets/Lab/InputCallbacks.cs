using UnityEngine.InputSystem;

namespace Hertzole.UnityToolbox.Lab
{
	public partial class InputCallbacks
	{
		[GenerateInputCallbacks(nameof(playerInput), GenerateStarted = true, GeneratePerformed = true, GenerateCanceled = true, GenerateAll = true)]
		public InputActionReference move;
		[GenerateInputCallbacks(nameof(playerInput), GeneratePerformed = true)]
		public InputActionReference look;
		[GenerateInputCallbacks(nameof(playerInput), GenerateStarted = true)]
		public InputActionReference Move { get; set; }

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

		private partial void OnInputMove_1Started(InputAction.CallbackContext context)
		{
			throw new System.NotImplementedException();
		}

		private partial void OnInputLookPerformed(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}