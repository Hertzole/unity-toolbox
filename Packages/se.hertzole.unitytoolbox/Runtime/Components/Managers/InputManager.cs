#if TOOLBOX_INPUT_SYSTEM && TOOLBOX_SCRIPTABLE_VALUES
using System.Collections.Specialized;
using Hertzole.ScriptableValues;
using UnityEngine;
using UnityEngine.InputSystem;
#if TOOLBOX_ADDRESSABLES
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#endif

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

        [SerializeField]
        private bool setInputActions = default;
        [SerializeField]
        private InputActionAsset inputActions = default;

        [SerializeField]
        private bool enableOnStart = true;
        [SerializeField]
        private bool autoEnableNewInputs = true;
        [SerializeField]
        private bool autoDisableRemovedInputs = true;

        private bool isEnabled;
        private bool hasSubscribed;

        private void Start()
        {
#if TOOLBOX_ADDRESSABLES
			if (useAddressables)
			{
				inputsListHandle = inputsListReference.LoadAsync(OnLoadedInputsList);

				if (setInputActions)
				{
					actionsHandle = inputActionsReference.LoadAsync(OnLoadedInputActions);
				}
			}
			else
#endif
            {
                if (setInputActions)
                {
                    playerInput.actions = inputActions;
                }

                if (enableOnStart)
                {
                    EnableInput();
                }
            }
        }

        private void OnEnable()
        {
            if (inputsList != null && !hasSubscribed)
            {
                SubscribeToEvents();
            }
        }

        private void OnDisable()
        {
            if (inputsList != null && hasSubscribed)
            {
#if TOOLBOX_SCRIPTABLE_VALUES_2
                inputsList.OnCollectionChanged -= OnCollectionChanged;
#else
				inputsList.OnAddedOrInserted -= OnInputAdded;
				inputsList.OnRemoved -= OnInputRemoved;
#endif
                hasSubscribed = false;
            }
        }

        private void OnDestroy()
        {
            if (inputsList == null)
            {
                return;
            }

            if (isEnabled)
            {
                DisableInput();
            }

#if TOOLBOX_ADDRESSABLES
			if (inputsListHandle.IsValid())
			{
				inputsListHandle.Release();
			}

			if (actionsHandle.IsValid())
			{
				actionsHandle.Release();
			}
#endif
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

        private void SubscribeToEvents()
        {
#if TOOLBOX_SCRIPTABLE_VALUES_2
            inputsList.OnCollectionChanged += OnCollectionChanged;
#else
			inputsList.OnAddedOrInserted += OnInputAdded;
			inputsList.OnRemoved += OnInputRemoved;
#endif
            hasSubscribed = true;
        }

#if TOOLBOX_SCRIPTABLE_VALUES_2
        private void OnCollectionChanged(CollectionChangedArgs<IHasPlayerInput> e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                for (int i = 0; i < e.NewItems.Length; i++)
                {
                    OnInputAdded(0, e.NewItems.Span[i]);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                for (int i = 0; i < e.OldItems.Length; i++)
                {
                    OnInputRemoved(0, e.OldItems.Span[i]);
                }
            }
        }
#endif

        private void OnInputAdded(int index, IHasPlayerInput input)
        {
            if (autoEnableNewInputs && isEnabled && playerInput.actions != null)
            {
                input.EnableInput(playerInput);
            }
        }

        private void OnInputRemoved(int index, IHasPlayerInput input)
        {
            if (autoDisableRemovedInputs && isEnabled && playerInput.actions != null)
            {
                input.DisableInput(playerInput);
            }
        }
#if TOOLBOX_ADDRESSABLES
		[SerializeField]
		private bool useAddressables = false;
		[SerializeField]
		private AssetReferenceT<ScriptablePlayerInputsList> inputsListReference = default;
		[SerializeField]
		private AssetReferenceT<InputActionAsset> inputActionsReference = default;
#endif

#if TOOLBOX_ADDRESSABLES
		private AsyncOperationHandle<ScriptablePlayerInputsList> inputsListHandle;
		private AsyncOperationHandle<InputActionAsset> actionsHandle;
#endif

#if TOOLBOX_ADDRESSABLES
		private void OnLoadedInputsList(AsyncOperationHandle<ScriptablePlayerInputsList> operation)
		{
			inputsList = operation.Result;

			if (!hasSubscribed)
			{
				SubscribeToEvents();
			}

			if (enableOnStart && playerInput.actions != null && !isEnabled)
			{
				EnableInput();
			}
		}

		private void OnLoadedInputActions(AsyncOperationHandle<InputActionAsset> operation)
		{
			inputActions = operation.Result;
			playerInput.actions = inputActions;

			if (enableOnStart && inputsList != null && !isEnabled)
			{
				EnableInput();
			}
		}
#endif

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

#if TOOLBOX_ADDRESSABLES
			if (useAddressables && !Application.isPlaying)
			{
				inputsList = null;
				inputActions = null;
			}
#endif
        }
#endif
    }
}
#endif