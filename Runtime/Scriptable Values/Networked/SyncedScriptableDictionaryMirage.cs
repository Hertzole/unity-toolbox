#if TOOLBOX_SCRIPTABLE_VALUES && TOOLBOX_MIRAGE
using System;
using AuroraPunks.ScriptableValues;
using Mirage.Collections;
using Mirage.Serialization;

namespace Hertzole.UnityToolbox
{
	public partial class SyncedScriptableDictionary<TKey, TValue> : ISyncObject
	{

		private bool didAdd;
		private bool didSet;
		private bool didRemove;

		public readonly SyncDictionary<TKey, TValue> syncDictionary = new SyncDictionary<TKey, TValue>();
		private bool didClear;

		public bool IsDirty { get { return syncDictionary.IsDirty; } }

		public event Action OnChange { add { syncDictionary.OnChange += value; } remove { syncDictionary.OnChange -= value; } }

		private partial void OnInitialized()
		{
			syncDictionary.OnSet += OnSetInternal;
			syncDictionary.OnInsert += OnInsertInternal;
			syncDictionary.OnRemove += OnRemoveInternal;
			syncDictionary.OnClear += OnClearInternal;
		}

		private partial void OnTargetAddedInternal(TKey key, TValue value)
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

		private partial void OnTargetSetInternal(TKey key, TValue oldValue, TValue newValue)
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

		private partial void OnTargetRemovedInternal(TKey key, TValue value)
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

		private partial void OnTargetClearedInternal()
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

		private partial void OnAdd(TKey key, TValue value)
		{
			syncDictionary.Add(key, value);
		}

		private partial void OnRemove(TKey key)
		{
			syncDictionary.Remove(key);
		}

		private partial void OnClear()
		{
			syncDictionary.Clear();
		}

		private partial TValue GetValue(TKey key)
		{
			return syncDictionary[key];
		}

		private partial void SetValue(TKey key, TValue newValue)
		{
			syncDictionary[key] = newValue;
		}

		public partial bool ContainsKey(TKey key)
		{
			return syncDictionary.ContainsKey(key);
		}

		public partial bool TryGetValue(TKey key, out TValue value)
		{
			return syncDictionary.TryGetValue(key, out value);
		}

		private partial void OnDisposed()
		{
			syncDictionary.OnSet -= OnSetInternal;
			syncDictionary.OnInsert -= OnInsertInternal;
			syncDictionary.OnRemove -= OnRemoveInternal;
			syncDictionary.OnClear -= OnClearInternal;
		}
	}
}
#endif