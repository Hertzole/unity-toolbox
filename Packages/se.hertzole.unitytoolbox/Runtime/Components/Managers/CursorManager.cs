#if TOOLBOX_SCRIPTABLE_VALUES
using Hertzole.ScriptableValues;
using UnityEngine;
#if TOOLBOX_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

#if TOOLBOX_ADDRESSABLES
using UnityEngine.AddressableAssets;
#endif

namespace Hertzole.UnityToolbox
{
    [DisallowMultipleComponent]
    public sealed partial class CursorManager : MonoSingleton<CursorManager>
    {
        [SerializeField]
        private ScriptableValue<bool> lockCursor = default;
        [SerializeField]
        private bool handleCursorLocking = true;

        [SerializeReference]
        private MatchGroup matches = new MatchGroup();

        private void Update()
        {
            if (!handleCursorLocking)
            {
                return;
            }

#if TOOLBOX_INPUT_SYSTEM
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && lockCursor != null && lockCursor.Value)
#else
            if (Input.GetMouseButtonDown(0) && lockCursor != null && lockCursor.Value)
#endif
            {
                LockCursor(true);
            }
        }

        private void OnDestroy()
        {
#if TOOLBOX_ADDRESSABLES
            ReleaseAssets();
#endif

            if (Instance == this)
            {
                if (lockCursor != null)
                {
                    lockCursor.OnValueChanged -= OnLockCursorChanged;
                }

                matches.OnIsMatchingChanged -= OnValueChanged;
                matches.Dispose();

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        protected override void OnAwake()
        {
#if TOOLBOX_ADDRESSABLES
            if (useAddressables)
            {
                LoadAssets();
            }
            else
#endif
            {
                if (lockCursor != null)
                {
                    lockCursor.OnValueChanged += OnLockCursorChanged;
                }

                matches.OnIsMatchingChanged += OnValueChanged;
                matches.Initialize();
            }
        }

#if TOOLBOX_ADDRESSABLES
        partial void OnLockCursorLoaded(ScriptableValue<bool> value)
        {
            lockCursor.OnValueChanged += OnLockCursorChanged;
            OnValueChanged(matches.IsMatching);
        }
#endif

        private void OnValueChanged(bool isMatching)
        {
            if (lockCursor == null)
            {
                return;
            }

            lockCursor.Value = isMatching;
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

#if UNITY_EDITOR && TOOLBOX_ADDRESSABLES
        private void OnValidate()
        {
            // We don't want to reset the value while playing.
            if (Application.isPlaying)
            {
                return;
            }

            if (useAddressables)
            {
                lockCursor = null;
            }
        }
#endif
#if TOOLBOX_ADDRESSABLES
        [SerializeField]
        private bool useAddressables = default;
        [SerializeField]
        [GenerateLoad]
        private AssetReferenceT<ScriptableValue<bool>> lockCursorReference = default;
#endif
    }
}
#endif