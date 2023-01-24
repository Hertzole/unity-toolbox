#if TOOLBOX_MIRAGE && TOOLBOX_SCRIPTABLE_VALUES
using System;
using AuroraPunks.ScriptableValues;
using Mirage.Collections;
using Mirage.Serialization;

namespace Hertzole.UnityToolbox
{
	public sealed class SyncedScriptableDictionary<TKey, TValue> : ISyncObject, IDisposable
	{
		private ScriptableDictionary<TKey, TValue> targetDictionary;

		private bool didAdd;
		private bool didSet;
		private bool didRemove;

		private bool originalIsReadOnly;
		private readonly bool isReadOnly;

		public readonly SyncDictionary<TKey, TValue> syncDictionary = new SyncDictionary<TKey, TValue>();
		private bool didClear;

		public bool IsDirty { get { return syncDictionary.IsDirty; } }

		public SyncedScriptableDictionary(bool isReadOnly = true)
		{
			this.isReadOnly = isReadOnly;
		}

		public event Action OnChange { add { syncDictionary.OnChange += value; } remove { syncDictionary.OnChange -= value; } }

		public void Initialize(ScriptableDictionary<TKey, TValue> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException(nameof(dictionary));
			}

			targetDictionary = dictionary;

			targetDictionary.OnAdded += OnTargetAddedInternal;
			targetDictionary.OnSet += OnTargetSetInternal;
			targetDictionary.OnRemoved += OnTargetRemovedInternal;
			targetDictionary.OnCleared += OnTargetClearedInternal;

			syncDictionary.OnSet += OnSetInternal;
			syncDictionary.OnInsert += OnInsertInternal;
			syncDictionary.OnRemove += OnRemoveInternal;
			syncDictionary.OnClear += OnClearInternal;

			if (isReadOnly)
			{
				originalIsReadOnly = targetDictionary.IsReadOnly;
				targetDictionary.IsReadOnly = true;
			}
		}

		private void OnTargetAddedInternal(TKey key, TValue value)
		{
			if (didAdd)
			{
				didAdd = false;
			}
			else
			{
				didAdd = true;
				syncDictionary.Add(key, value);
			}
		}

		private void OnTargetSetInternal(TKey key, TValue oldValue, TValue newValue)
		{
			if (didSet)
			{
				didSet = false;
			}
			else
			{
				didSet = true;
				syncDictionary[key] = newValue;
			}
		}

		private void OnTargetRemovedInternal(TKey key, TValue value)
		{
			if (didRemove)
			{
				didRemove = false;
			}
			else
			{
				didRemove = true;
				syncDictionary.Remove(key);
			}
		}

		private void OnTargetClearedInternal()
		{
			if (didClear)
			{
				didClear = false;
			}
			else
			{
				didClear = true;
				syncDictionary.Clear();
			}
		}

		private void OnSetInternal(TKey key, TValue oldValue, TValue newValue)
		{
			if (isReadOnly)
			{
				targetDictionary.IsReadOnly = false;
			}

			if (didSet)
			{
				didSet = false;
			}
			else
			{
				didSet = true;
				targetDictionary[key] = newValue;
			}

			if (isReadOnly)
			{
				targetDictionary.IsReadOnly = true;
			}
		}

		private void OnInsertInternal(TKey key, TValue value)
		{
			if (isReadOnly)
			{
				targetDictionary.IsReadOnly = false;
			}

			if (didAdd)
			{
				didAdd = false;
			}
			else
			{
				didAdd = true;
				targetDictionary.Add(key, value);
			}

			if (isReadOnly)
			{
				targetDictionary.IsReadOnly = true;
			}
		}

		private void OnRemoveInternal(TKey key, TValue value)
		{
			if (isReadOnly)
			{
				targetDictionary.IsReadOnly = false;
			}

			if (didRemove)
			{
				didRemove = false;
			}
			else
			{
				didRemove = true;
				targetDictionary.Remove(key);
			}

			if (isReadOnly)
			{
				targetDictionary.IsReadOnly = true;
			}
		}

		private void OnClearInternal()
		{
			if (isReadOnly)
			{
				targetDictionary.IsReadOnly = false;
			}

			if (didClear)
			{
				didClear = false;
			}
			else
			{
				didClear = true;
				targetDictionary.Clear();
			}

			if (isReadOnly)
			{
				targetDictionary.IsReadOnly = true;
			}
		}

		public void Flush()
		{
			syncDictionary.Flush();
		}

		public void OnSerializeAll(NetworkWriter writer)
		{
			syncDictionary.OnSerializeAll(writer);
		}

		public void OnSerializeDelta(NetworkWriter writer)
		{
			syncDictionary.OnSerializeDelta(writer);
		}

		public void OnDeserializeAll(NetworkReader reader)
		{
			syncDictionary.OnDeserializeAll(reader);
		}

		public void OnDeserializeDelta(NetworkReader reader)
		{
			syncDictionary.OnDeserializeDelta(reader);
		}

		public void Reset()
		{
			syncDictionary.Reset();
		}

		public void Dispose()
		{
			targetDictionary.OnAdded -= OnTargetAddedInternal;
			targetDictionary.OnSet -= OnTargetSetInternal;
			targetDictionary.OnRemoved -= OnTargetRemovedInternal;
			targetDictionary.OnCleared -= OnTargetClearedInternal;

			syncDictionary.OnSet -= OnSetInternal;
			syncDictionary.OnInsert -= OnInsertInternal;
			syncDictionary.OnRemove -= OnRemoveInternal;
			syncDictionary.OnClear -= OnClearInternal;

			if (isReadOnly)
			{
				targetDictionary.IsReadOnly = originalIsReadOnly;
			}
		}

		public void Add(TKey key, TValue value)
		{
			syncDictionary.Add(key, value);
		}

		public void Remove(TKey key)
		{
			syncDictionary.Remove(key);
		}

		public void Clear()
		{
			syncDictionary.Clear();
		}

		public bool ContainsKey(TKey key)
		{
			return syncDictionary.ContainsKey(key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return syncDictionary.TryGetValue(key, out value);
		}

		public TValue this[TKey key] { get { return syncDictionary[key]; } set { syncDictionary[key] = value; } }
	}
}
#endif