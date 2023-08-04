using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	public static class VisiaulElementUtilities
	{
		public static VisualElement VerticalSpace(float height = 8f)
		{
			return new VisualElement
			{
				style =
				{
					height = height
				}
			};
		}

		public static VisualElement HorizontalSpace(float width = 8f)
		{
			return new VisualElement
			{
				style =
				{
					width = width
				}
			};
		}

		public static Label Header(string text)
		{
			return new Label(text)
			{
				style =
				{
					unityFontStyleAndWeight = FontStyle.Bold,
					marginTop = 13,
					paddingLeft = 1,
					marginLeft = 3
				}
			};
		}
	}
}