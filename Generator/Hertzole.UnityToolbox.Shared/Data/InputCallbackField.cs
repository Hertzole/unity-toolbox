using System;
using System.Text;

namespace Hertzole.UnityToolbox.Shared;

[Flags]
public enum InputCallbackType : byte
{
	None = 0,
	Started = 1,
	Performed = 2,
	Canceled = 4,
	All = 8
}

public readonly struct InputCallbackField
{
	public readonly string name;
	public readonly string inputName;
	public readonly InputCallbackType callbackType;

	public readonly string hasSubscribedField;
	public readonly string subscribeToField;
	public readonly string unsubscribeFromField;
	public readonly string? startedMethod;
	public readonly string? performedMethod;
	public readonly string? canceledMethod;
	public readonly string? allMethod;

	public InputCallbackField(string name, string inputName, string uniqueName, InputCallbackType callbackType)
	{
		this.name = name;
		this.inputName = inputName;
		this.callbackType = callbackType;

		using (StringBuilderPool.Get(out StringBuilder nameBuilder))
		{
			nameBuilder.Append("hasSubscribedTo");
			nameBuilder.Append(TextUtility.FormatVariableLabel(uniqueName));
			hasSubscribedField = nameBuilder.ToString();
			nameBuilder.Clear();

			nameBuilder.Append("SubscribeTo");
			nameBuilder.Append(TextUtility.FormatVariableLabel(uniqueName));
			subscribeToField = nameBuilder.ToString();
			nameBuilder.Clear();

			nameBuilder.Append("UnsubscribeFrom");
			nameBuilder.Append(TextUtility.FormatVariableLabel(uniqueName));
			unsubscribeFromField = nameBuilder.ToString();
			nameBuilder.Clear();

			if ((callbackType & InputCallbackType.Started) != 0)
			{
				nameBuilder.Append("OnInput");
				nameBuilder.Append(TextUtility.FormatVariableLabel(uniqueName));
				nameBuilder.Append("Started");
				startedMethod = nameBuilder.ToString();
				nameBuilder.Clear();
			}

			if ((callbackType & InputCallbackType.Performed) != 0)
			{
				nameBuilder.Append("OnInput");
				nameBuilder.Append(TextUtility.FormatVariableLabel(uniqueName));
				nameBuilder.Append("Performed");
				performedMethod = nameBuilder.ToString();
				nameBuilder.Clear();
			}

			if ((callbackType & InputCallbackType.Canceled) != 0)
			{
				nameBuilder.Append("OnInput");
				nameBuilder.Append(TextUtility.FormatVariableLabel(uniqueName));
				nameBuilder.Append("Canceled");
				canceledMethod = nameBuilder.ToString();
				nameBuilder.Clear();
			}

			if ((callbackType & InputCallbackType.All) != 0)
			{
				nameBuilder.Append("OnInput");
				nameBuilder.Append(TextUtility.FormatVariableLabel(uniqueName));
				allMethod = nameBuilder.ToString();
			}
		}
	}
}