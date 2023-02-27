#if TOOLBOX_SCRIPTABLE_VALUES
using AuroraPunks.ScriptableValues;
using UnityEngine;
#if TOOLBOX_CECIL_ATTRIBUTES
using Hertzole.CecilAttributes;
#endif

namespace Hertzole.UnityToolbox
{
	[DisallowMultipleComponent]
	public sealed class CursorManager : MonoSingleton<CursorManager>
	{
		[SerializeField]
#if TOOLBOX_CECIL_ATTRIBUTES
		// [Required]
#endif
		private ScriptableValue<bool> lockCursor = default;

		[SerializeReference]
		private IScriptableMatch[] matches = default;

		private void OnDestroy()
		{
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
			for (int i = 0; i < matches.Length; i++)
			{
				matches[i].Initialize();
				matches[i].OnValueChanged += OnValueChanged;
			}
		}

		private void Start()
		{
			OnValueChanged();
		}

		private void OnValueChanged()
		{
			lockCursor.Value = AllMatches();
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