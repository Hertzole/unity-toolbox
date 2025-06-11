using System;

namespace Hertzole.UnityToolbox
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public sealed class GenerateInputCallbacksAttribute : Attribute
	{
		public bool GenerateStarted { get; set; } = false;
		public bool GeneratePerformed { get; set; } = false;
		public bool GenerateCanceled { get; set; } = false;
		public bool GenerateAll { get; set; } = false;

		public GenerateInputCallbacksAttribute() { }

		public GenerateInputCallbacksAttribute(string inputName) { }
	}
}