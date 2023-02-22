#if TOOLBOX_SCRIPTABLE_VALUES
using System;
using System.Collections.Generic;
using AuroraPunks.ScriptableValues;
using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine;

namespace Hertzole.UnityToolbox.Matches
{
	[Serializable]
	public abstract class ScriptableValueMatch<T> : IScriptableMatch
	{
		[SerializeField]
		private ScriptableValue<T> target = default;
		[SerializeField]
		private T mustMatchValue = default;

		public event Action OnValueChanged;

		private void OnValueChangedInternal(T previousValue, T newValue)
		{
			OnValueChanged?.Invoke();
		}

		public void Initialize()
		{
			target.OnValueChanged += OnValueChangedInternal;
		}

		public void Dispose()
		{
			target.OnValueChanged -= OnValueChangedInternal;
		}

		public bool Matches()
		{
			return EqualityHelper.Equals(target.Value, mustMatchValue);
		}
	}
}
#endif