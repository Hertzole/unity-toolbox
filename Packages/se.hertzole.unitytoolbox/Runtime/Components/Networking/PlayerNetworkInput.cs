﻿#if TOOLBOX_SCRIPTABLE_VALUES && TOOLBOX_INPUT_SYSTEM && (TOOLBOX_MIRAGE || FISHNET)
using UnityEngine;
#if TOOLBOX_ADDRESSABLES
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#endif

namespace Hertzole.UnityToolbox
{
	public sealed partial class PlayerNetworkInput
	{
#if TOOLBOX_ADDRESSABLES
		[SerializeField] 
		private bool useAddressable = false;
		[SerializeField] 
		private AssetReferenceT<ScriptablePlayerInputsList> inputsListReference = default;
#endif
		[SerializeField]
		private ScriptablePlayerInputsList inputsList = default;

		private IHasPlayerInput[] inputs;
		
#if TOOLBOX_ADDRESSABLES
		private AsyncOperationHandle<ScriptablePlayerInputsList>? inputsHandle;
#endif

		private void AddInputs()
		{
			if (inputsList == null)
			{
				return;
			}
			
			foreach (IHasPlayerInput input in inputs)
			{
				inputsList.Add(input);
			}
		}
		
		private void RemoveInputs()
		{
			if (inputsList == null)
			{
				return;
			}
			
			foreach (IHasPlayerInput input in inputs)
			{
				inputsList.Remove(input);
			}
		}
		
#if TOOLBOX_ADDRESSABLES
		private void LoadInputs()
		{
			inputsHandle = inputsListReference.LoadAsync(handle =>
			{
				if (handle.Status == AsyncOperationStatus.Succeeded)
				{
					inputsList = handle.Result;
					AddInputs();
				}
			});
		}

		private void OnDestroy()
		{
			inputsHandle.Release();
		}
#endif
		
#if UNITY_EDITOR && TOOLBOX_ADDRESSABLES
#if FISHNET
		protected override void OnValidate()
#else
		private void OnValidate()
#endif
		{
#if FISHNET
			base.OnValidate();
#endif
			
			if (useAddressable && !Application.isPlaying && inputsList != null)
			{
				inputsList = null;
			}
		}
#endif
	}
}
#endif