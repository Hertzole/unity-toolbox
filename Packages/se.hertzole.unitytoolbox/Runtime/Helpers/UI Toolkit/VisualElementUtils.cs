using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	public static class VisualElementUtils
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
	}
}