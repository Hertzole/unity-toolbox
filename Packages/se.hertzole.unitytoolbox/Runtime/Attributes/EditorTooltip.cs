using System;
using System.Diagnostics;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	[AttributeUsage(AttributeTargets.Field)]
	[Conditional("UNITY_EDITOR")]
	internal sealed class EditorTooltip : TooltipAttribute
	{
		/// <inheritdoc />
		public EditorTooltip(string tooltip) : base(tooltip) { }
	}
}