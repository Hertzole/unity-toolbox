#if TOOLBOX_INPUT_SYSTEM
using System;
using UnityEngine.InputSystem;

namespace Hertzole.UnityToolbox
{
	/// <summary>
	///     Extension methods for the PlayerInput class.
	///     <b>NOTE: This class is only available if the Input System package is installed.</b>
	/// </summary>
	public static class PlayerInputExtensions
	{
		/// <summary>
		///     A shortcut for adding a listener to the started event for this action reference on the player input.
		/// </summary>
		/// <param name="input">The player input to get the input from.</param>
		/// <param name="action">The action to listen to.</param>
		/// <param name="onStartedCallback">The event callback that should be invoked when the started event is invoked.</param>
		/// <exception cref="ArgumentNullException">If the player input is null.</exception>
		/// <exception cref="ArgumentNullException">If the action is null.</exception>
		/// <exception cref="ArgumentNullException">If the event event callback is null.</exception>
		/// <example>
		///     <code>
		/// 	void OnEnable()
		///     {
		///    		playerInput.AddStartedListener(action, OnStarted);
		///     }
		///     
		///     void OnStarted(InputAction.CallbackContext context)
		///     {
		///    		Debug.Log("Started!");
		///     }
		///     </code>
		/// </example>
		public static void AddStartedListener(this PlayerInput input, InputActionReference action, Action<InputAction.CallbackContext> onStartedCallback)
		{
			ThrowHelper.ThrowIfNull(input, nameof(input));
			ThrowHelper.ThrowIfNull(action, nameof(action));
			ThrowHelper.ThrowIfNull(onStartedCallback, nameof(onStartedCallback));

			input.actions[action.action.name].started += onStartedCallback;
		}

		/// <summary>
		///     A shortcut for adding a listener to the performed event for this action reference on the player input.
		/// </summary>
		/// <param name="input">The player input to get the input from.</param>
		/// <param name="action">The action to listen to.</param>
		/// <param name="onPerformedCallback">The event callback that should be invoked when the performed event is invoked.</param>
		/// <exception cref="ArgumentNullException">If the player input is null.</exception>
		/// <exception cref="ArgumentNullException">If the action is null.</exception>
		/// <exception cref="ArgumentNullException">If the event event callback is null.</exception>
		/// <example>
		///     <code>
		/// 	void OnEnable()
		///     {
		///    		playerInput.AddPerformedListener(action, OnPerformed);
		///     }
		///     
		///     void OnPerformed(InputAction.CallbackContext context)
		///     {
		///    		Debug.Log("Performed!");
		///     }
		///     </code>
		/// </example>
		public static void AddPerformedListener(this PlayerInput input, InputActionReference action, Action<InputAction.CallbackContext> onPerformedCallback)
		{
			ThrowHelper.ThrowIfNull(input, nameof(input));
			ThrowHelper.ThrowIfNull(action, nameof(action));
			ThrowHelper.ThrowIfNull(onPerformedCallback, nameof(onPerformedCallback));

			input.actions[action.action.name].performed += onPerformedCallback;
		}

		/// <summary>
		///     A shortcut for adding a listener to the canceled event for this action reference on the player input.
		/// </summary>
		/// <param name="input">The player input to get the input from.</param>
		/// <param name="action">The action to listen to.</param>
		/// <param name="onCanceledCallback">The event callback that should be invoked when the canceled event is invoked.</param>
		/// <exception cref="ArgumentNullException">If the player input is null.</exception>
		/// <exception cref="ArgumentNullException">If the action is null.</exception>
		/// <exception cref="ArgumentNullException">If the event event callback is null.</exception>
		/// <example>
		///     <code>
		/// 	void OnEnable()
		///     {
		///    		playerInput.AddCanceledListener(action, OnCanceled);
		///     }
		///     
		///     void OnCanceled(InputAction.CallbackContext context)
		///     {
		///    		Debug.Log("Canceled!");
		///     }
		///     </code>
		/// </example>
		public static void AddCanceledListener(this PlayerInput input, InputActionReference action, Action<InputAction.CallbackContext> onCanceledCallback)
		{
			ThrowHelper.ThrowIfNull(input, nameof(input));
			ThrowHelper.ThrowIfNull(action, nameof(action));
			ThrowHelper.ThrowIfNull(onCanceledCallback, nameof(onCanceledCallback));

			input.actions[action.action.name].canceled += onCanceledCallback;
		}

		/// <summary>
		///     A shortcut for remove a listener from the started event for this action reference on the player input.
		/// </summary>
		/// <param name="input">The player input to get the input from.</param>
		/// <param name="action">The action to listen to.</param>
		/// <param name="onStartedCallback">The event callback that was invoked when the started event was invoked.</param>
		/// <exception cref="ArgumentNullException">If the player input is null.</exception>
		/// <exception cref="ArgumentNullException">If the action is null.</exception>
		/// <exception cref="ArgumentNullException">If the event event callback is null.</exception>
		/// <example>
		///     <code>
		/// 	void OnDisable()
		///     {
		///    		playerInput.RemoveStartedListener(action, OnStarted);
		///     }
		///     
		///     void OnStarted(InputAction.CallbackContext context)
		///     {
		///    		Debug.Log("Started!");
		///     }
		///     </code>
		/// </example>
		public static void RemoveStartedListener(this PlayerInput input, InputActionReference action, Action<InputAction.CallbackContext> onStartedCallback)
		{
			ThrowHelper.ThrowIfNull(input, nameof(input));
			ThrowHelper.ThrowIfNull(action, nameof(action));
			ThrowHelper.ThrowIfNull(onStartedCallback, nameof(onStartedCallback));

			input.actions[action.action.name].started -= onStartedCallback;
		}

		/// <summary>
		///     A shortcut for remove a listener from the performed event for this action reference on the player input.
		/// </summary>
		/// <param name="input">The player input to get the input from.</param>
		/// <param name="action">The action to listen to.</param>
		/// <param name="onPerformedCallback">The event callback that was invoked when the performed event was invoked.</param>
		/// <exception cref="ArgumentNullException">If the player input is null.</exception>
		/// <exception cref="ArgumentNullException">If the action is null.</exception>
		/// <exception cref="ArgumentNullException">If the event event callback is null.</exception>
		/// <example>
		///     <code>
		/// 	void OnDisable()
		///     {
		///    		playerInput.RemovePerformedListener(action, OnPerformed);
		///     }
		///     
		///     void OnPerformed(InputAction.CallbackContext context)
		///     {
		///    		Debug.Log("Performed!");
		///     }
		///     </code>
		/// </example>
		public static void RemovePerformedListener(this PlayerInput input, InputActionReference action, Action<InputAction.CallbackContext> onPerformedCallback)
		{
			ThrowHelper.ThrowIfNull(input, nameof(input));
			ThrowHelper.ThrowIfNull(action, nameof(action));
			ThrowHelper.ThrowIfNull(onPerformedCallback, nameof(onPerformedCallback));

			input.actions[action.action.name].performed -= onPerformedCallback;
		}

		/// <summary>
		///     A shortcut for remove a listener from the canceled event for this action reference on the player input.
		/// </summary>
		/// <param name="input">The player input to get the input from.</param>
		/// <param name="action">The action to listen to.</param>
		/// <param name="onCanceledCallback">The event callback that was invoked when the canceled event was invoked.</param>
		/// <exception cref="ArgumentNullException">If the player input is null.</exception>
		/// <exception cref="ArgumentNullException">If the action is null.</exception>
		/// <exception cref="ArgumentNullException">If the event event callback is null.</exception>
		/// <example>
		///     <code>
		/// 	void OnDisable()
		///     {
		///    		playerInput.RemoveCanceledListener(action, OnCanceled);
		///     }
		///     
		///     void OnCanceled(InputAction.CallbackContext context)
		///     {
		///    		Debug.Log("Canceled!");
		///     }
		///     </code>
		/// </example>
		public static void RemoveCanceledListener(this PlayerInput input, InputActionReference action, Action<InputAction.CallbackContext> onCanceledCallback)
		{
			ThrowHelper.ThrowIfNull(input, nameof(input));
			ThrowHelper.ThrowIfNull(action, nameof(action));
			ThrowHelper.ThrowIfNull(onCanceledCallback, nameof(onCanceledCallback));

			input.actions[action.action.name].canceled -= onCanceledCallback;
		}
	}
}
#endif // TOOLBOX_INPUT_SYSTEM