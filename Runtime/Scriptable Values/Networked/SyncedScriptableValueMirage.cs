#if TOOLBOX_MIRAGE && TOOLBOX_SCRIPTABLE_VALUES
using System;
using System.Collections.Generic;
using AuroraPunks.ScriptableValues;
using Mirage.Collections;
using Mirage.Serialization;

namespace Hertzole.UnityToolbox
{
	public sealed partial class SyncedScriptableValue<T> : ISyncObject
	{
		private bool isReadOnly;

		private bool didSet;
		
		public event Action OnChange;

		public bool IsDirty { get; private set; }

		private partial void OnInitialized() { }

		private partial bool CanModifyValue()
		{
			if (didSet)
			{
				return true;
			}
			
			return !isReadOnly;
		}

		private partial void OnValueChanged(T previousValue, T newValue)
		{
			if (didSet)
			{
				didSet = false;
			}
			else
			{
				IsDirty = true;
				OnChange?.Invoke();
			}
		}

		/// <summary>
		///     Called after a successful sync.
		/// </summary>
		public void Flush()
		{
			IsDirty = false;
		}

		public void OnSerializeAll(NetworkWriter writer)
		{
			isReadOnly = false;
			
			writer.Write(targetScriptableValue.Value);
		}

		public void OnSerializeDelta(NetworkWriter writer)
		{
			isReadOnly = false;
			
			writer.Write(targetScriptableValue.Value);
		}

		public void OnDeserializeAll(NetworkReader reader)
		{
			Deserialize(reader);
		}

		public void OnDeserializeDelta(NetworkReader reader)
		{
			Deserialize(reader);
		}

		private void Deserialize(NetworkReader reader)
		{
			isReadOnly = true;

			didSet = true;
			bool oldEqualityCheck = targetScriptableValue.SetEqualityCheck;
			targetScriptableValue.SetEqualityCheck = false;
			targetScriptableValue.Value = reader.Read<T>();
			targetScriptableValue.SetEqualityCheck = oldEqualityCheck;

			OnChange?.Invoke();
		}

		public void Reset()
		{
			isReadOnly = false;
			IsDirty = false;
		}
	}
}
#endif