#if TOOLBOX_INPUT_SYSTEM && TOOLBOX_SCRIPTABLE_VALUES
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
#if TOOLBOX_ADDRESSABLES
		[SerializeField]
		private bool useAddressables = false;
		[SerializeField]
		private AssetReferenceT<ScriptablePlayerInputsList> inputsListReference = default;
#endif
		[SerializeField]
		private ScriptablePlayerInputsList inputsList = default;
		[SerializeField]
		[HideInInspector]
		private PlayerInput playerInput = default;
		
		[SerializeField]
		private bool enableOnStart = true;
		[SerializeField]
		private bool autoEnableNewInputs = true;
		[SerializeField]
		private bool autoDisableRemovedInputs = true;

		private bool isEnabled;
		private bool hasSubscribed;

#if TOOLBOX_ADDRESSABLES
		private AsyncOperationHandle<ScriptablePlayerInputsList>? assetHandle;
#endif

		private void Start()
		{
#if TOOLBOX_ADDRESSABLES
			if (useAddressables)
			{
				assetHandle = Addressables.LoadAssetAsync<ScriptablePlayerInputsList>(inputsListReference);
				assetHandle.Value.Completed += OnLoadedAsset;
			}
			else
#endif
			if (enableOnStart)
			{
				EnableInput();
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
				inputsList.OnAddedOrInserted -= OnInputAdded;
				inputsList.OnRemoved -= OnInputRemoved;
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
			if (assetHandle != null)
			{
				Addressables.Release(assetHandle.Value);
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

#if TOOLBOX_ADDRESSABLES
		private void OnLoadedAsset(AsyncOperationHandle<ScriptablePlayerInputsList> operation)
		{
			inputsList = operation.Result;

			if (!hasSubscribed)
			{
				SubscribeToEvents();
			}

			if (enableOnStart)
			{
				EnableInput();
			}
		}
#endif

		private void SubscribeToEvents()
		{
			inputsList.OnAddedOrInserted += OnInputAdded;
			inputsList.OnRemoved += OnInputRemoved;
			hasSubscribed = true;
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

#if TOOLBOX_ADDRESSABLES
			if (useAddressables && inputsList != null && !Application.isPlaying)
			{
				inputsList = null;
			}
#endif
		}
#endif
	}
}
#endif