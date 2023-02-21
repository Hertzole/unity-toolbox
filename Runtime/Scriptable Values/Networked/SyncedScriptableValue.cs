#if TOOLBOX_SCRIPTABLE_VALUES && (TOOLBOX_MIRAGE || FISHNET)
using System;
using System.Collections.Generic;
using AuroraPunks.ScriptableValues;

namespace Hertzole.UnityToolbox
{
	public sealed partial class SyncedScriptableValue<T> : IDisposable
	{
		private ScriptableValue<T> targetScriptableValue;

		public void Initialize(ScriptableValue<T> targetValue)
		{
			targetScriptableValue = targetValue;
			
			targetValue.OnValueChanged += OnValueChangedInternal;

			OnInitialized();
		}

		private void OnValueChangedInternal(T previousValue, T newValue)
		{
			if (!CanModifyValue())
			{
				throw new InvalidOperationException("Only server can modify value.");
			}

			OnValueChanged(previousValue, newValue);
		}
		
		private partial void OnValueChanged(T previousValue, T newValue);

		private partial void OnInitialized();

		private partial bool CanModifyValue();

		~SyncedScriptableValue()
		{
			ReleaseUnmanagedResources();
		}

		private void ReleaseUnmanagedResources()
		{
			if (targetScriptableValue != null)
			{
				targetScriptableValue.OnValueChanged -= OnValueChangedInternal;
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