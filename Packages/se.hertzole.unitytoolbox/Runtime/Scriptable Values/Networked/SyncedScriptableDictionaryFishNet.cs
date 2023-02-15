#if TOOLBOX_SCRIPTABLE_VALUES && FISHNET
using System;
using System.Collections.Generic;
using FishNet.Object.Synchronizing;
using FishNet.Object.Synchronizing.Internal;
using FishNet.Serializing;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace Hertzole.UnityToolbox
{
	public partial class SyncedScriptableDictionary<TKey, TValue> : SyncBase, ICustomSync
	{
		private readonly struct ChangedData
		{
			internal readonly SyncDictionaryOperation operation;
			internal readonly TKey key;
			internal readonly TValue value;
			
			public ChangedData(SyncDictionaryOperation operation, TKey key, TValue value)
			{
				this.operation = operation;
				this.key = key;
				this.value = value;
			}
		}

		private bool valuesChanged;
		private bool sendAll;

		private bool didAdd;
		private bool didSet;
		private bool didRemove;
		private bool didClear;
		
		private readonly IDictionary<TKey, TValue> collection = new Dictionary<TKey, TValue>();
		private readonly IDictionary<TKey, TValue> clientHostCollection = new Dictionary<TKey, TValue>();
		private readonly IDictionary<TKey, TValue> initialValues = new Dictionary<TKey, TValue>();

		private readonly List<ChangedData> changed = new List<ChangedData>();
		private readonly List<ChangedData> serverOnChanges = new List<ChangedData>();
		private readonly List<ChangedData> clientOnChanges = new List<ChangedData>();

		private partial void OnInitialized()
		{
			
		}

		protected override void Registered()
		{
			base.Registered();
			
			using(IEnumerator<KeyValuePair<TKey, TValue>> enumerator = initialValues.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					collection.Add(enumerator.Current.Key, enumerator.Current.Value);
				}
			}
		}

		public override void OnStartCallback(bool asServer)
		{
			base.OnStartCallback(asServer);
			
			// var currentCollection = asServer ? serverOnChanges : clientOnChanges;
		}

		private void AddOperation(SyncDictionaryOperation operation, TKey key, TValue value)
		{
			if (!IsRegistered)
			{
				return;
			}

			bool asServerInvoke = !IsNetworkInitialized || NetworkBehaviour.IsServer;

			if (asServerInvoke)
			{
				valuesChanged = true;
				if (Dirty())
				{
					var change = new ChangedData(operation, key, value);
					changed.Add(change);
				}
			}
		}

		public override void WriteDelta(PooledWriter writer, bool resetSyncTick = true)
		{
			base.WriteDelta(writer, resetSyncTick);

			if (sendAll)
			{
				sendAll = false;
				changed.Clear();
				WriteFull(writer);
			}
			else
			{
				writer.WriteBoolean(false);
				writer.WriteInt32(changed.Count);

				for (int i = 0; i < changed.Count; i++)
				{
					var change = changed[i];
					writer.WriteByte((byte)change.operation);

					//Clear does not need to write anymore data so it is not included in checks.
					if (change.operation == SyncDictionaryOperation.Add ||
					    change.operation == SyncDictionaryOperation.Set)
					{
						writer.Write(change.key);
						writer.Write(change.value);
					}
					else if (change.operation == SyncDictionaryOperation.Remove)
					{
						writer.Write(change.key);
					}
				}
				
				changed.Clear();
			}
		}

		public override void WriteFull(PooledWriter writer)
		{
			if (!valuesChanged)
			{
				return;
			}

			base.WriteHeader(writer, false);

			writer.WriteBoolean(true);
			writer.WriteInt32(collection.Count);
			using (IEnumerator<KeyValuePair<TKey, TValue>> enumerator = collection.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					writer.Write(enumerator.Current.Key);
					writer.Write(enumerator.Current.Value);
				}
			}
		}

		public override void Read(PooledReader reader)
		{
			bool asServer = false;
			bool asClientAndHost = (!asServer && NetworkBehaviour.IsServer);
			var currentCollection = (asClientAndHost) ? clientHostCollection : this.collection;

			bool fullWrite = reader.ReadBoolean();
			if (fullWrite)
			{
				currentCollection.Clear();
			}

			int changes = reader.ReadInt32();
			for (int i = 0; i < changes; i++)
			{
				SyncDictionaryOperation operation = (SyncDictionaryOperation)reader.ReadByte();
				TKey key = default;
				TValue value = default;

				/* Add, Set.
				 * Use the Set code for add and set,
				 * especially so collection doesn't throw
				 * if entry has already been added. */
				if (operation == SyncDictionaryOperation.Add || operation == SyncDictionaryOperation.Set)
				{
					key = reader.Read<TKey>();
					value = reader.Read<TValue>();
					collection[key] = value;
					if (operation == SyncDictionaryOperation.Add)
					{
						didAdd = true;
					}
					else
					{
						didSet = true;
					}
					targetDictionary[key] = value;
				}
				//Clear.
				else if (operation == SyncDictionaryOperation.Clear)
				{
					collection.Clear();
					didClear = true;
					targetDictionary.Clear();
				}
				//Remove.
				else if (operation == SyncDictionaryOperation.Remove)
				{
					key = reader.Read<TKey>();
					collection.Remove(key);
					didRemove = true;
					targetDictionary.Remove(key);
				}
			}
		}

		public override void Reset()
		{
			base.Reset();
			sendAll = false;
			changed.Clear();
			collection.Clear();
			clientHostCollection.Clear();
			targetDictionary.Clear();
			valuesChanged = false;
			
			using(var enumerator = initialValues.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					collection.Add(enumerator.Current.Key, enumerator.Current.Value);
					targetDictionary.Add(enumerator.Current.Key, enumerator.Current.Value);
				}
			}
		}

		private partial void OnItemAdded(TKey key, TValue value)
		{
			if (!CanNetworkSetValues())
			{
				return;
			}

			if (didAdd)
			{
				didAdd = false;
				return;
			}

			collection.Add(key, value);
			AddOperation(SyncDictionaryOperation.Add, key, value);
		}

		private partial void OnItemSet(TKey key, TValue oldValue, TValue newValue)
		{
			if (!CanNetworkSetValues())
			{
				return;
			}

			if (didSet)
			{
				didSet = false;
				return;
			}
			
			collection[key] = newValue;
			AddOperation(SyncDictionaryOperation.Set, key, newValue);
		}

		private partial void OnItemRemoved(TKey key, TValue value)
		{
			if (!CanNetworkSetValues())
			{
				return;
			}

			if (didRemove)
			{
				didRemove = false;
				return;
			}

			if (collection.Remove(key))
			{
				AddOperation(SyncDictionaryOperation.Remove, key, default);
			}
		}

		private partial void OnCleared()
		{
			if (!CanNetworkSetValues())
			{
				return;
			}
			
			if (didClear)
			{
				didClear = false;
				return;
			}
			
			collection.Clear();
			AddOperation(SyncDictionaryOperation.Clear, default, default);
		}

		private partial void OnDisposed() { }

		public object GetSerializedType()
		{
			return null;
		}
	}
}
#endif