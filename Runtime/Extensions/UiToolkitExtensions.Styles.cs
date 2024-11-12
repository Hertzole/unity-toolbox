using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	public static partial class UiToolkitExtensions
	{
		public static void SetUniformBorder(this VisualElement element, StyleColor color, StyleFloat width, StyleLength radius)
		{
			element.style.borderTopColor = color;
			element.style.borderRightColor = color;
			element.style.borderBottomColor = color;
			element.style.borderLeftColor = color;
			element.style.borderTopWidth = width;
			element.style.borderRightWidth = width;
			element.style.borderBottomWidth = width;
			element.style.borderLeftWidth = width;
			element.style.borderTopLeftRadius = radius;
			element.style.borderTopRightRadius = radius;
			element.style.borderBottomRightRadius = radius;
			element.style.borderBottomLeftRadius = radius;
		}
	}
}