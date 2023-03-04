#if TOOLBOX_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Hertzole.UnityToolbox
{
	public static class AddressableExtensions
	{
		public static AsyncOperationHandle<T> LoadAsync<T>(this AssetReferenceT<T> reference, Action<AsyncOperationHandle<T>> onCompleted = null) where T : Object
		{
			AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(reference);
			if (onCompleted != null)
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
	}
}
#endif