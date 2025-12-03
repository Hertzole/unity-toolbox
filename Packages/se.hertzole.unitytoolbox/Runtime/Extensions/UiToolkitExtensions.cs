using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	public static partial class UiToolkitExtensions
	{
		private static readonly Dictionary<Type, PropertyInfo> focusPropertyCache = new Dictionary<Type, PropertyInfo>();

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

		public static void RegisterExpandedChangedCallback(this Foldout foldout, EventCallback<ChangeEvent<bool>> callback)
		{
			foldout.RegisterCallback<ChangeEvent<bool>, EventCallback<ChangeEvent<bool>>>(static (evt, args) =>
			{
				if (evt.currentTarget == evt.target)
				{
					args.Invoke(evt);
				}
			}, callback);
		}

		public static void RegisterExpandedChangedCallback<TArgs>(this Foldout foldout, EventCallback<ChangeEvent<bool>, TArgs> callback, TArgs userArgs)
		{
			foldout.RegisterCallback<ChangeEvent<bool>, (EventCallback<ChangeEvent<bool>, TArgs>, TArgs)>(static (evt, args) =>
			{
				if (evt.currentTarget == evt.target)
				{
					args.Item1.Invoke(evt, args.Item2);
				}
			}, (callback, userArgs));
		}

		public static bool TryGetLabelElement(this BindableElement element, [CanBeNull][NotNullWhen(true)] out Label label)
		{
			Label labelElement = element.Q<Label>(className: "unity-label");
			if (labelElement != null)
			{
				label = labelElement;
				return true;
			}

			label = null;
			return false;
		}

		public static bool IsFocused<T>(this TextInputBaseField<T> field)
		{
			Type fieldType = field.GetType();

			if (!focusPropertyCache.TryGetValue(fieldType, out PropertyInfo property))
			{
				property = fieldType.GetProperty("hasFocus", BindingFlags.Instance | BindingFlags.NonPublic);
				focusPropertyCache[fieldType] = property;
			}

			bool result = property != null && (bool) property.GetValue(field);
			return result;
		}

		public static void ClampValue(this INotifyValueChanged<int> field, int min, int max)
		{
			field.RegisterValueChangedCallback(static (evt, args) =>
			{
				if (args.field is TextInputBaseField<int> textField && textField.IsFocused())
				{
					return;
				}

				if (evt.newValue < args.min)
				{
					args.field.SetValueWithoutNotify(args.min);
				}
				else if (evt.newValue > args.max)
				{
					args.field.SetValueWithoutNotify(args.max);
				}
			}, (field, min, max));
		}
		
		public static void ClampValue(this INotifyValueChanged<uint> field, uint min, uint max)
		{
			field.RegisterValueChangedCallback(static (evt, args) =>
			{
				if (args.field is TextInputBaseField<uint> textField && textField.IsFocused())
				{
					return;
				}

				if (evt.newValue < args.min)
				{
					args.field.SetValueWithoutNotify(args.min);
				}
				else if (evt.newValue > args.max)
				{
					args.field.SetValueWithoutNotify(args.max);
				}
			}, (field, min, max));
		}
		
		public static void ClampValue(this INotifyValueChanged<long> field, long min, long max)
		{
			field.RegisterValueChangedCallback(static (evt, args) =>
			{
				if (args.field is TextInputBaseField<long> textField && textField.IsFocused())
				{
					return;
				}

				if (evt.newValue < args.min)
				{
					args.field.SetValueWithoutNotify(args.min);
				}
				else if (evt.newValue > args.max)
				{
					args.field.SetValueWithoutNotify(args.max);
				}
			}, (field, min, max));
		}
		
		public static void ClampValue(this INotifyValueChanged<ulong> field, ulong min, ulong max)
		{
			field.RegisterValueChangedCallback(static (evt, args) =>
			{
				if (args.field is TextInputBaseField<ulong> textField && textField.IsFocused())
				{
					return;
				}

				if (evt.newValue < args.min)
				{
					args.field.SetValueWithoutNotify(args.min);
				}
				else if (evt.newValue > args.max)
				{
					args.field.SetValueWithoutNotify(args.max);
				}
			}, (field, min, max));
		}
	}
}