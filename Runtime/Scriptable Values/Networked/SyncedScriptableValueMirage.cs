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
		private T value;
		
		private bool isReadOnly;
		private bool didSet;
		private bool hasValue;
		
		public event Action OnChange;

		public bool IsDirty { get; private set; }

		private partial void OnInitialized()
		{
			if (hasValue)
			{
				didSet = true;
				bool oldEqualityCheck = targetScriptableValue.SetEqualityCheck;
				targetScriptableValue.SetEqualityCheck = false;
				targetScriptableValue.Value = value;
				targetScriptableValue.SetEqualityCheck = oldEqualityCheck;
			}
		}

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

			if (targetScriptableValue == null)
			{
				return;
			}
			
			writer.Write(targetScriptableValue.Value);
		}

		public void OnSerializeDelta(NetworkWriter writer)
		{
			OnSerializeAll(writer);
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

			if (targetScriptableValue == null)
			{
				value = reader.Read<T>();
				hasValue = true;
				return;
			}

			didSet = true;
			bool oldEqualityCheck = targetScriptableValue.SetEqualityCheck;
			targetScriptableValue.SetEqualityCheck = false;
			targetScriptableValue.Value = reader.Read<T>();
			targetScriptableValue.SetEqualityCheck = oldEqualityCheck;
			hasValue = false;

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