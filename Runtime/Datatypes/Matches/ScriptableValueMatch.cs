#if TOOLBOX_SCRIPTABLE_VALUES
using System;
using Hertzole.ScriptableValues;
using Hertzole.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Hertzole.UnityToolbox.Matches
{
	[Serializable]
	public abstract partial class ScriptableValueMatch<T> : IScriptableMatch
	{
		[SerializeField]
		private ScriptableValue<T> target = default;
		[SerializeField]
		private T mustMatchValue = default;

		[NonSerialized]
		private bool hasInitialized;

		public event Action OnValueChanged;

		private void OnValueChangedInternal(T previousValue, T newValue)
		{
			OnValueChanged?.Invoke();
		}

#if TOOLBOX_ADDRESSABLES
		partial void OnTargetLoaded(ScriptableValue<T> value)
		{
			if (hasInitialized)
			{
				value.OnValueChanged += OnValueChangedInternal;
			}
		}
#endif

		public void Initialize()
		{
			if (hasInitialized)
			{
				return;
			}

#if TOOLBOX_ADDRESSABLES
			if (useAddressables)
			{
				LoadAssets();
			}
#endif
			if (target != null)
			{
				target.OnValueChanged += OnValueChangedInternal;
			}

			hasInitialized = true;
		}

		public void Dispose()
		{
			if (target != null)
			{
				target.OnValueChanged -= OnValueChangedInternal;
			}

#if TOOLBOX_ADDRESSABLES
			ReleaseAssets();
			target = null;
#endif

			hasInitialized = false;
		}

		public bool Matches()
		{
			return EqualityHelper.Equals(target.Value, mustMatchValue);
		}
#if TOOLBOX_ADDRESSABLES
		[SerializeField]
		private bool useAddressables = default;
		[SerializeField]
		[GenerateLoad]
		private AssetReferenceT<ScriptableValue<T>> targetReference = default;
#endif
	}
}
#endif