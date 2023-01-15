#if TOOLBOX_INPUT_SYSTEM && TOOLBOX_SCRIPTABLE_VALUES
using Hertzole.UnityToolbox.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hertzole.UnityToolbox
{
#if UNITY_EDITOR
	[RequireComponent(typeof(PlayerInput))]
#endif
	[DefaultExecutionOrder(-1_000_000)]
	public class InputManager : MonoBehaviour
	{
		[SerializeField]
		private ScriptablePlayerInputsList inputsList = default;
		[SerializeField]
		[HideInInspector]
		private PlayerInput playerInput = default;

		[Space]
		[SerializeField]
		private bool enableOnStart = true;
		[SerializeField]
		private bool autoEnableNewInputs = true;
		[SerializeField]
		private bool autoDisableRemovedInputs = true;

		private bool isEnabled;

		private void Start()
		{
			if (enableOnStart)
			{
				EnableInput();
			}
		}

		private void OnEnable()
		{
			inputsList.OnAddedOrInserted += OnInputAdded;
			inputsList.OnRemoved += OnInputRemoved;
		}

		private void OnDisable()
		{
			inputsList.OnAddedOrInserted -= OnInputAdded;
			inputsList.OnRemoved -= OnInputRemoved;
		}

		private void OnDestroy()
		{
			if (isEnabled)
			{
				DisableInput();
			}
		}

		public void EnableInput()
		{
			isEnabled = true;

			for (int i = 0; i < inputsList.Count; i++)
			{
				inputsList[i].EnableInput(playerInput);
			}
		}

		public void DisableInput()
		{
			isEnabled = false;

			for (int i = 0; i < inputsList.Count; i++)
			{
				inputsList[i].DisableInput(playerInput);
			}
		}

		private void OnInputAdded(int arg1, IHasPlayerInput arg2)
		{
			if (autoEnableNewInputs && isEnabled)
			{
				arg2.EnableInput(playerInput);
			}
		}

		private void OnInputRemoved(int arg1, IHasPlayerInput arg2)
		{
			if (autoDisableRemovedInputs && isEnabled)
			{
				arg2.DisableInput(playerInput);
			}
		}

#if UNITY_EDITOR
		private void Reset()
		{
			GetStandardComponents();
		}

		private void OnValidate()
		{
			GetStandardComponents();
		}

		private void GetStandardComponents()
		{
			if (playerInput == null)
			{
				playerInput = GetComponent<PlayerInput>();
			}
		}
#endif
	}
}
#endif