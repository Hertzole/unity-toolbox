using System;
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
		
		public static void RegisterValueChangedCallback<T, TArgs>(this INotifyValueChanged<T> control,
			EventCallback<ChangeEvent<T>, TArgs> callback,
			TArgs userArgs)
		{
			if (control is not VisualElement element)
			{
				throw new InvalidOperationException("INotifyValueChanged<T> must be a VisualElement to register value changed callbacks.");
			}

			element.RegisterCallback(callback, userArgs);
		}
		
		public static void UnregisterValueChangedCallback<T, TArgs>(this INotifyValueChanged<T> control,
			EventCallback<ChangeEvent<T>, TArgs> callback)
		{
			if (control is not VisualElement element)
			{
				throw new InvalidOperationException("INotifyValueChanged<T> must be a VisualElement to unregister value changed callbacks.");
			}

			element.UnregisterCallback(callback);
		}
	}
}