#if TOOLBOX_ANIMATION
using System;
using System.Collections.Generic;
using System.Text;
using Hertzole.UnityToolbox.Editor;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	public abstract class AnimatorParameterDrawer : ToolboxPropertyDrawer
	{
		private readonly List<ParameterData> parameters = new List<ParameterData>();
		private readonly HashSet<AnimatorController> controllers = new HashSet<AnimatorController>();
		private static readonly StringBuilder stringBuilder = new StringBuilder();
		private readonly List<int> choices = new List<int>();
		private readonly List<string> availableChoices = new List<string>();

		private GUIContent[] choicesArray;

		public abstract AnimatorControllerParameterType ParameterType { get; }

		private readonly struct ParameterData
		{
			public readonly string path;
			public readonly string name;

			public ParameterData(string path, string name)
			{
				this.path = path;
				this.name = name;
			}
		}

		protected override void DrawGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			FindAnimators(property);
			availableChoices.Clear();

			for (int i = 0; i < parameters.Count; i++)
			{
				availableChoices.Add(parameters[i].path);
			}

			if (!IsArrayAndListEqual(availableChoices, choicesArray))
			{
				choicesArray = new GUIContent[availableChoices.Count];
				for (int i = 0; i < availableChoices.Count; i++)
				{
					choicesArray[i] = new GUIContent(availableChoices[i]);
				}
			}

			SerializedProperty nameProperty = property.FindPropertyRelative("name");
			int selectedIndex = GetSelectedIndex(parameters, nameProperty.stringValue);

			EditorGUI.BeginChangeCheck();
			int value = EditorGUI.Popup(position, label, selectedIndex, choicesArray);
			if (EditorGUI.EndChangeCheck())
			{
				nameProperty.stringValue = parameters[value].name;
			}
		}

		private static bool IsArrayAndListEqual(IReadOnlyList<string> list, IReadOnlyList<GUIContent> array)
		{
			if (array == null)
			{
				return false;
			}

			if (list.Count != array.Count)
			{
				return false;
			}

			for (int i = 0; i < list.Count; i++)
			{
				if (!string.Equals(list[i], array[i].text, StringComparison.Ordinal))
				{
					return false;
				}
			}

			return true;
		}

		protected override VisualElement CreateGUI(SerializedProperty property, string label)
		{
			FindAnimators(property);
			choices.Clear();

			for (int i = 0; i < parameters.Count; i++)
			{
				choices.Add(i);
			}

			SerializedProperty nameProperty = property.FindPropertyRelative("name");
			int selectedIndex = GetSelectedIndex(parameters, nameProperty.stringValue);

			PopupField<int> popupField = new PopupField<int>(label, choices, selectedIndex, FormatParameterName, FormatParameterName);
			popupField.RegisterCallback<ChangeEvent<int>, SerializedProperty>((evt, prop) =>
			{
				prop.stringValue = parameters[evt.newValue].name;
				prop.serializedObject.ApplyModifiedProperties();
			}, nameProperty);

			popupField.SetEnabled(parameters.Count != 0);

			return popupField;
		}

		private string FormatParameterName(int index)
		{
			if (parameters.Count == 0)
			{
				return "No animators found";
			}

			if (index < 0 || index >= parameters.Count)
			{
				return "Other...";
			}

			return parameters[index].path;
		}

		private void FindAnimators(SerializedProperty property)
		{
			Profiler.BeginSample("Find Animators");

			controllers.Clear();

			SerializedProperty iterator = property.serializedObject.GetIterator();
			while (iterator.Next(true))
			{
				if (iterator.propertyType == SerializedPropertyType.ObjectReference && iterator.objectReferenceValue is Animator animator)
				{
					AnimatorController controller = FindController(animator);
					if (controller != null)
					{
						controllers.Add(controller);
					}
				}
			}

			MonoBehaviour behaviour = property.serializedObject.targetObject as MonoBehaviour;
			if (behaviour != null)
			{
				foreach (Animator animator in behaviour.GetComponentsInChildren<Animator>())
				{
					AnimatorController controller = FindController(animator);
					if (controller != null)
					{
						controllers.Add(controller);
					}
				}

				foreach (Animator animator in behaviour.GetComponentsInParent<Animator>())
				{
					AnimatorController controller = FindController(animator);
					if (controller != null)
					{
						controllers.Add(controller);
					}
				}
			}

			bool isSingle = controllers.Count == 1;

			parameters.Clear();

			foreach (AnimatorController controller in controllers)
			{
				FindParameters(controller, parameters, isSingle, ParameterType);
			}

			Profiler.EndSample();
		}

		private static AnimatorController FindController(Animator animator)
		{
			RuntimeAnimatorController runtimeController;

			if (animator.runtimeAnimatorController is AnimatorOverrideController overrideController)
			{
				runtimeController = overrideController.runtimeAnimatorController as AnimatorController;
			}
			else
			{
				runtimeController = animator.runtimeAnimatorController as AnimatorController;
			}

			AnimatorController controller = (AnimatorController) runtimeController;

			return controller;
		}

		private static void FindParameters(AnimatorController controller, List<ParameterData> parameters, bool isSingle, AnimatorControllerParameterType type)
		{
			foreach (AnimatorControllerParameter parameter in controller.parameters)
			{
				if (parameter.type != type)
				{
					continue;
				}

				stringBuilder.Clear();
				if (!isSingle)
				{
					stringBuilder.Append($"{controller.name}/");
				}

				stringBuilder.Append(parameter.name);

				parameters.Add(new ParameterData(stringBuilder.ToString(), parameter.name));
			}
		}

		private static int GetSelectedIndex(in IReadOnlyList<ParameterData> parameters, in string value)
		{
			for (int i = 0; i < parameters.Count; i++)
			{
				if (parameters[i].name == value)
				{
					return i;
				}
			}

			return -1;
		}
	}
}
#endif