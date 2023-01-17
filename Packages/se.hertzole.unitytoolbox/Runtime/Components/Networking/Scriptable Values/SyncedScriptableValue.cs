#if TOOLBOX_MIRAGE && TOOLBOX_SCRIPTABLE_VALUES
using System;
using AuroraPunks.ScriptableValues;
using Mirage.Collections;
using Mirage.Serialization;

namespace Hertzole.UnityToolbox
{
	public sealed class SyncedScriptableValue<T> : ISyncObject where T : IEquatable<T>
	{
		private ScriptableValue<T> targetScriptableValue;

		private bool isReadOnly;

		public T Value
		{
			get { return targetScriptableValue.Value; }
			set
			{
				if (isReadOnly)
				{
					throw new InvalidOperationException("SyncedScriptableValues can only be modified on the server.");
				}

				if (!targetScriptableValue.Value.Equals(value))
				{
					bool oldEqualityCheck = targetScriptableValue.SetEqualityCheck;
					targetScriptableValue.SetEqualityCheck = false;
					targetScriptableValue.Value = value;
					targetScriptableValue.SetEqualityCheck = oldEqualityCheck;

					IsDirty = true;
					OnChange?.Invoke();
				}
			}
		}

		public bool IsDirty { get; private set; }

		public event Action OnChange;
		
		public event ScriptableValue<T>.OldNewValue<T> OnValueChanging
		{
			add { targetScriptableValue.OnValueChanging += value; }
			remove { targetScriptableValue.OnValueChanging -= value; }
		} 
		
		public event ScriptableValue<T>.OldNewValue<T> OnValueChanged
		{
			add { targetScriptableValue.OnValueChanged += value; }
			remove { targetScriptableValue.OnValueChanged -= value; }
		}

		public void Initialize(ScriptableValue<T> targetValue)
		{
			targetScriptableValue = targetValue;
		}

		/// <summary>
		/// Called after a successful sync.
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
			isReadOnly = true;
			
			bool oldEqualityCheck = targetScriptableValue.SetEqualityCheck;
			targetScriptableValue.SetEqualityCheck = false;
			targetScriptableValue.Value = reader.Read<T>();
			targetScriptableValue.SetEqualityCheck = oldEqualityCheck;
			
			OnChange?.Invoke();
		}

		public void OnDeserializeDelta(NetworkReader reader)
		{
			isReadOnly = true;
			
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