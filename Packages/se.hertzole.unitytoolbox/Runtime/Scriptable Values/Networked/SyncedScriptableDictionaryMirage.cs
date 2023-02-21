#if TOOLBOX_SCRIPTABLE_VALUES && TOOLBOX_MIRAGE
using System;
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

		private partial void OnItemAdded(TKey key, TValue value)
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

		private partial void OnItemSet(TKey key, TValue oldValue, TValue newValue)
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

		private partial void OnItemRemoved(TKey key, TValue value)
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

		private partial void OnCleared()
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
			if (didSet)
			{
				didSet = false;
			}
			else
			{
				didSet = true;
				targetDictionary[key] = newValue;
			}
		}

		private void OnInsertInternal(TKey key, TValue value)
		{
			if (didAdd)
			{
				didAdd = false;
			}
			else
			{
				didAdd = true;
				targetDictionary.Add(key, value);
			}
		}

		private void OnRemoveInternal(TKey key, TValue value)
		{
			if (didRemove)
			{
				didRemove = false;
			}
			else
			{
				didRemove = true;
				targetDictionary.Remove(key);
			}
		}

		private void OnClearInternal()
		{
			if (didClear)
			{
				didClear = false;
			}
			else
			{
				didClear = true;
				targetDictionary.Clear();
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