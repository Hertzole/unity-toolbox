#if TOOLBOX_ADDRESSABLES
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Hertzole.UnityToolbox
{
	public static class AddressableExtensions
	{
		// ReSharper disable Unity.PerformanceAnalysis
		public static AsyncOperationHandle<T> LoadAsync<T>(this AssetReferenceT<T> reference, Action<AsyncOperationHandle<T>> onCompleted = null, bool onlyCompleteOnSuccess = true) where T : Object
		{
			AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(reference);
			if (onlyCompleteOnSuccess)
			{
				handle.Completed += operationHandle =>
				{
					if (operationHandle.Status != AsyncOperationStatus.Succeeded)
					{
						Debug.LogError($"Failed to load reference {operationHandle.DebugName} with error: {operationHandle.OperationException}");
						Addressables.Release(operationHandle);
						return;
					}

					onCompleted?.Invoke(operationHandle);
				};
			}
			else if (onCompleted != null)
			{
				handle.Completed += onCompleted;
			}

			return handle;
		}

		public static void Release<T>(this AsyncOperationHandle<T>? handle)
		{
			if (handle.HasValue && handle.Value.IsValid())
			{
				Addressables.Release(handle.Value);
			}
		}

		public static void Release<T>(this AsyncOperationHandle<T> handle)
		{
			if (handle.IsValid())
			{
				Addressables.Release(handle);
			}
		}

		public static void Release(this AsyncOperationHandle handle)
		{
			if (handle.IsValid())
			{
				Addressables.Release(handle);
			}
		}
	}
}
#endif