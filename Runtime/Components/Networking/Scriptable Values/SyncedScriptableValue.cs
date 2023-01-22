#if TOOLBOX_MIRAGE && TOOLBOX_SCRIPTABLE_VALUES
using System;
using AuroraPunks.ScriptableValues;
using Mirage.Collections;
using Mirage.Serialization;

namespace Hertzole.UnityToolbox
{
	public sealed class SyncedScriptableValue<T> : ISyncObject, IDisposable where T : IEquatable<T>
	{
		private ScriptableValue<T> targetScriptableValue;

		private bool isReadOnly;

		private bool originalReadOnly;
		private readonly bool setReadOnly;

		public T Value
		{
			get { return targetScriptableValue.Value; }
			set
			{
				if (isReadOnly)
				{
					throw new InvalidOperationException("SyncedScriptableValues can only be modified on the server.");
				}

				if (setReadOnly)
				{
					targetScriptableValue.IsReadOnly = false;
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

				if (setReadOnly)
				{
					targetScriptableValue.IsReadOnly = true;
				}
			}
		}

		public bool IsDirty { get; private set; }

		public SyncedScriptableValue(bool setReadOnly = true)
		{
			this.setReadOnly = setReadOnly;
		}

		public event Action OnChange;

		public event ScriptableValue<T>.OldNewValue<T> OnValueChanging { add { targetScriptableValue.OnValueChanging += value; } remove { targetScriptableValue.OnValueChanging -= value; } }

		public event ScriptableValue<T>.OldNewValue<T> OnValueChanged { add { targetScriptableValue.OnValueChanged += value; } remove { targetScriptableValue.OnValueChanged -= value; } }

		public void Initialize(ScriptableValue<T> targetValue)
		{
			targetScriptableValue = targetValue;

			if (targetScriptableValue != null && setReadOnly)
			{
				originalReadOnly = targetScriptableValue.IsReadOnly;
				targetScriptableValue.IsReadOnly = true;
			}
		}

		~SyncedScriptableValue()
		{
			ReleaseUnmanagedResources();
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

		private void ReleaseUnmanagedResources()
		{
			if (targetScriptableValue != null && setReadOnly)
			{
				targetScriptableValue.IsReadOnly = originalReadOnly;
			}
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}
	}
}
#endif