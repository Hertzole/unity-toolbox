#if TOOLBOX_SCRIPTABLE_VALUES && (TOOLBOX_MIRAGE || FISHNET)
using System;
using AuroraPunks.ScriptableValues;

namespace Hertzole.UnityToolbox
{
	public sealed partial class SyncedScriptableDictionary<TKey, TValue> : IDisposable
	{
		private ScriptableDictionary<TKey, TValue> targetDictionary;

		public void Initialize(ScriptableDictionary<TKey, TValue> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException(nameof(dictionary));
			}

			targetDictionary = dictionary;

			targetDictionary.OnAdded += OnItemAdded;
			targetDictionary.OnSet += OnItemSet;
			targetDictionary.OnRemoved += OnItemRemoved;
			targetDictionary.OnCleared += OnCleared;

			OnInitialized();
		}

		public void Dispose()
		{
			targetDictionary.OnAdded -= OnItemAdded;
			targetDictionary.OnSet -= OnItemSet;
			targetDictionary.OnRemoved -= OnItemRemoved;
			targetDictionary.OnCleared -= OnCleared;

			OnDisposed();
		}

		private partial void OnInitialized();

		private partial void OnItemAdded(TKey key, TValue value);

		private partial void OnItemSet(TKey key, TValue oldValue, TValue newValue);

		private partial void OnItemRemoved(TKey key, TValue value);

		private partial void OnCleared();

		private partial void OnDisposed();

	}
}
#endif