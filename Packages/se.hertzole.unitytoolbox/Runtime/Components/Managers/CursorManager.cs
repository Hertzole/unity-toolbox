#if TOOLBOX_SCRIPTABLE_VALUES
using Hertzole.ScriptableValues;
using UnityEngine;
#if TOOLBOX_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Hertzole.UnityToolbox
{
	[DisallowMultipleComponent]
	public sealed class CursorManager : MonoSingleton<CursorManager>
	{
		[SerializeField]
		private ScriptableValue<bool> lockCursor = default;
		[SerializeField]
		private bool handleCursorLocking = true;

		[SerializeReference]
		private IScriptableMatch[] matches = default;

		private void Start()
		{
			OnValueChanged();
		}

		private void Update()
		{
			if (!handleCursorLocking)
			{
				return;
			}

#if TOOLBOX_INPUT_SYSTEM
			if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && lockCursor.Value)
#else
			if (Input.GetMouseButtonDown(0) && lockCursor.Value)
#endif
			{
				LockCursor(true);
			}
		}

		private void OnDestroy()
		{
			lockCursor.OnValueChanged -= OnLockCursorChanged;

			for (int i = 0; i < matches.Length; i++)
			{
				matches[i].Dispose();
				matches[i].OnValueChanged -= OnValueChanged;
			}

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		protected override void OnAwake()
		{
			lockCursor.OnValueChanged += OnLockCursorChanged;

			for (int i = 0; i < matches.Length; i++)
			{
				matches[i].Initialize();
				matches[i].OnValueChanged += OnValueChanged;
			}
		}

		private void OnValueChanged()
		{
			lockCursor.Value = AllMatches();
		}

		private void OnLockCursorChanged(bool previousValue, bool newValue)
		{
			if (!handleCursorLocking)
			{
				return;
			}

			LockCursor(newValue);
		}

		private static void LockCursor(bool shouldLock)
		{
			Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = !shouldLock;
		}

		private bool AllMatches()
		{
			for (int i = 0; i < matches.Length; i++)
			{
				if (!matches[i].Matches())
				{
					return false;
				}
			}

			return true;
		}
	}
}
#endif