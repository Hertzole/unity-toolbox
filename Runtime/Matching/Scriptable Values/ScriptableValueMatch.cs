#if TOOLBOX_SCRIPTABLE_VALUES
#nullable enable
using System;
using Hertzole.ScriptableValues;
using Hertzole.ScriptableValues.Helpers;
using UnityEngine;
#if TOOLBOX_ADDRESSABLES
using UnityEngine.AddressableAssets;
#endif

namespace Hertzole.UnityToolbox.Matches
{
    [Serializable]
    public abstract partial class ScriptableValueMatch<T> : IMatcher
    {
        [SerializeField]
        private ScriptableValue<T> target = null!;
        [SerializeField]
        private bool invert = false;
        [SerializeField]
        private T? mustMatchValue = default;

        [NonSerialized]
        private bool hasInitialized;

        /// <inheritdoc />
        public event Action? OnMatcherUpdated;

        private void OnValueChangedInternal(T previousValue, T newValue)
        {
            OnMatcherUpdated?.Invoke();
        }

#if TOOLBOX_ADDRESSABLES
        partial void OnTargetLoaded(ScriptableValue<T> value)
        {
            if (hasInitialized)
            {
                value.OnValueChanged += OnValueChangedInternal;
                OnMatcherUpdated?.Invoke();
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
            return invert ^ EqualityHelper.Equals(target.Value, mustMatchValue);
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