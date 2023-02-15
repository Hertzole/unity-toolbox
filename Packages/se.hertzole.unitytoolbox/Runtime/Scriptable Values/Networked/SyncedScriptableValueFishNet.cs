#if TOOLBOX_SCRIPTABLE_VALUES && FISHNET
using System.Collections.Generic;
using FishNet.Object.Helping;
using FishNet.Object.Synchronizing;
using FishNet.Object.Synchronizing.Internal;
using FishNet.Serializing;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	partial class SyncedScriptableValue<T> : SyncBase, ICustomSync
	{
		private T initialValue;
		private T previousClientValue;
		private T value;
		
		private partial void OnInitialized()
		{
			initialValue = targetScriptableValue.Value;
			UpdateValues(initialValue);
		}

		private void UpdateValues(T next)
		{
			previousClientValue = next;
			value = next;
		}

		private partial bool CanModifyValue()
		{
			return CanNetworkSetValues(false);
		}

		private partial void OnSetValue(T newValue)
		{
			SetValue(newValue, true);
		}

		private void SetValue(T newValue, bool calledByUser)
		{
			if (!IsRegistered)
			{
				return;
			}

			if (IsNetworkInitialized && CodegenHelper.NetworkObject_Deinitializing(NetworkBehaviour))
			{
				return;
			}

			if (calledByUser)
			{
				bool asServerInvoke = !IsNetworkInitialized || NetworkBehaviour.IsServer;

				if (!IsNetworkInitialized)
				{
					UpdateValues(newValue);
				}
				else
				{
					if (EqualityComparer<T>.Default.Equals(value, newValue))
					{
						return;
					}

					value = newValue;
				}

				TryDirty(asServerInvoke);
			}
			else
			{
				if (!NetworkManager.IsServer)
				{
					UpdateValues(newValue);
				}
				else
				{
					previousClientValue = newValue;
				}
			}

			void TryDirty(bool asServer)
			{
				if (!IsNetworkInitialized)
				{
					return;
				}

				if (asServer)
				{
					Dirty();
				}
			}
		}

		protected override void Registered()
		{
			base.Registered();
			initialValue = value;
		}

		public override void WriteDelta(PooledWriter writer, bool resetSyncTick = true)
		{
			base.WriteDelta(writer, resetSyncTick);
			writer.Write(value);
		}

		public override void WriteFull(PooledWriter writer)
		{
			if (EqualityComparer<T>.Default.Equals(initialValue, value))
			{
				return;
			}

			WriteDelta(writer, false);
		}

		public override void Read(PooledReader reader)
		{
			base.Read(reader);
			
			T next = reader.Read<T>();
			SetValue(next, false);
			if (setReadOnly)
			{
				targetScriptableValue.IsReadOnly = false;
			}
			
			targetScriptableValue.Value = value;
			
			if (setReadOnly)
			{
				targetScriptableValue.IsReadOnly = true;
			}
		}

		public object GetSerializedType()
		{
			return null;
		}
	}
}
#endif