#if TOOLBOX_SCRIPTABLE_VALUES && (TOOLBOX_MIRAGE || FISHNET)
using System;
using System.Collections.Generic;
using AuroraPunks.ScriptableValues;

namespace Hertzole.UnityToolbox
{
	public sealed partial class SyncedScriptableValue<T> : IDisposable
	{
		private ScriptableValue<T> targetScriptableValue;

		private bool originalReadOnly;
		private readonly bool setReadOnly;

		public T Value
		{
			get { return targetScriptableValue.Value; }
			set
			{
				if (!CanModifyValue())
				{
					throw new InvalidOperationException("SyncedScriptableValues can only be modified on the server.");
				}

				if (setReadOnly)
				{
					targetScriptableValue.IsReadOnly = false;
				}

				if (!EqualityComparer<T>.Default.Equals(targetScriptableValue.Value, value))
				{
					bool oldEqualityCheck = targetScriptableValue.SetEqualityCheck;
					targetScriptableValue.SetEqualityCheck = false;
					targetScriptableValue.Value = value;
					targetScriptableValue.SetEqualityCheck = oldEqualityCheck;

					OnSetValue(value);
				}

				if (setReadOnly)
				{
					targetScriptableValue.IsReadOnly = true;
				}
			}
		}

		public SyncedScriptableValue(bool setReadOnly = true)
		{
			this.setReadOnly = setReadOnly;
		}

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
			
			OnInitialized();
		}

		private partial void OnInitialized();

		private partial bool CanModifyValue();

		private partial void OnSetValue(T newValue);

		~SyncedScriptableValue()
		{
			ReleaseUnmanagedResources();
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