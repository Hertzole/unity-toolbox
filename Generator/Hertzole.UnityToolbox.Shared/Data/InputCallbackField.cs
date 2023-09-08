using System;
using Hertzole.UnityToolbox.Shared;

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

public readonly record struct InputCallbackField(string Name, string InputName, InputCallbackType CallbackType, bool IsAddressable)
{
	public string HasSubscribedField { get; } = $"hasSubscribedTo{TextUtility.FormatVariableLabel(Name)}";

	public string SubscribeToField { get; } = $"SubscribeTo{TextUtility.FormatVariableLabel(Name)}";

	public string UnsubscribeFromField { get; } = $"UnsubscribeFrom{TextUtility.FormatVariableLabel(Name)}";

	public string StartedMethod { get; } = $"OnInput{TextUtility.FormatVariableLabel(Name)}Started";

	public string PerformedMethod { get; } = $"OnInput{TextUtility.FormatVariableLabel(Name)}Performed";

	public string CanceledMethod { get; } = $"OnInput{TextUtility.FormatVariableLabel(Name)}Canceled";

	public string AllMethod { get; } = $"OnInput{TextUtility.FormatVariableLabel(Name)}";

	public string Name { get; } = Name;
	public string InputName { get; } = InputName;
	public InputCallbackType CallbackType { get; } = CallbackType;
	public bool IsAddressable { get; } = IsAddressable;
}