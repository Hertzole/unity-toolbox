using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	public static partial class UiToolkitExtensions
	{
		public static T QueryParent<T>(this VisualElement element, string name = null, params string[] classes) where T : VisualElement
		{
			VisualElement parent = element;

			while (parent != null)
			{
				bool nameMatches = string.IsNullOrEmpty(name) || parent.name == name;

				if (parent is T t && nameMatches && t.HasClasses(classes))
				{
					return t;
				}

				parent = parent.parent;
			}

			return null;
		}

		public static bool HasClasses(this VisualElement element, params string[] classes)
		{
			if (classes == null || classes.Length == 0)
			{
				return true;
			}

			for (int i = 0; i < classes.Length; i++)
			{
				if (!element.ClassListContains(classes[i]))
				{
					return false;
				}
			}

			return true;
		}
	}
}