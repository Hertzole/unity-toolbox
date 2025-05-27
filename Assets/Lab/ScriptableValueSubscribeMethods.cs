using System;
using Hertzole.ScriptableValues;

namespace Hertzole.UnityToolbox.Lab
{
	public partial class ScriptableValueSubscribeMethods
	{
		// [GenerateSubscribeMethods]
		public ScriptableBool boolValue;
		// [GenerateSubscribeMethods]
		public ScriptableBoolEvent boolEvent;
		// [GenerateSubscribeMethods]
		public ScriptableEvent scriptableEvent;
		// [GenerateSubscribeMethods]
		public ScriptableBool BoolValue { get; set; }
		// [GenerateSubscribeMethods]
		public ScriptableValue<bool> BigValue { get; set; }

		// private partial void OnBoolValueChanged(bool previousValue, bool newValue)
		// {
		// 	throw new NotImplementedException();
		// }
		//
		// private partial void OnBoolEventInvoked(object sender, bool args)
		// {
		// 	throw new NotImplementedException();
		// }
		//
		// private partial void OnScriptableEventInvoked(object sender, EventArgs e)
		// {
		// 	throw new NotImplementedException();
		// }
		//
		// private partial void OnBoolValue_1Changed(bool previousValue, bool newValue)
		// {
		// 	
		// }
		//
		// private partial void OnBigValueChanged(bool previousValue, bool newValue)
		// {
		// 	throw new NotImplementedException();
		// }
	}
}