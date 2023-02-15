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

		public event Action OnChange;

		public bool IsDirty { get; private set; }

		private partial void OnSetValue(T newValue)
		{
			IsDirty = true;
			OnChange?.Invoke();
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
			writer.Write(targetScriptableValue.Value);
		}

		public void OnSerializeDelta(NetworkWriter writer)
		{
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
			if (setReadOnly)
			{
				targetScriptableValue.IsReadOnly = false;
			}
			
			isReadOnly = true;

			bool oldEqualityCheck = targetScriptableValue.SetEqualityCheck;
			targetScriptableValue.SetEqualityCheck = false;
			targetScriptableValue.Value = reader.Read<T>();
			targetScriptableValue.SetEqualityCheck = oldEqualityCheck;

			OnChange?.Invoke();

			if (setReadOnly)
			{
				targetScriptableValue.IsReadOnly = true;
			}
		}

		public void Reset()
		{
			isReadOnly = false;
			IsDirty = false;
		}
	}
}
#endif