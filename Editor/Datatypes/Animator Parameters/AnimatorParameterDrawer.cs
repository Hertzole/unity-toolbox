#if TOOLBOX_ANIMATION
using System.Collections.Generic;
using System.Text;
using Hertzole.UnityToolbox.Editor;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	public abstract class AnimatorParameterDrawer : ToolboxPropertyDrawer
	{
		private readonly List<ParameterData> parameters = new List<ParameterData>();

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

		protected override void DrawGUI(Rect position, SerializedProperty property, GUIContent label) { }

		protected override VisualElement CreateGUI(SerializedProperty property)
		{
			FindAnimators(property);
			List<int> choices = new List<int>(parameters.Count);

			for (int i = 0; i < parameters.Count; i++)
			{
				choices.Add(i);
			}

			SerializedProperty nameProperty = property.FindPropertyRelative("name");
			int selectedIndex = GetSelectedIndex(parameters, nameProperty.stringValue);

			PopupField<int> popupField = new PopupField<int>(property.displayName, choices, selectedIndex, FormatParameterName, FormatParameterName);
			popupField.RegisterValueChangedCallback(evt =>
			{
				nameProperty.stringValue = parameters[evt.newValue].name;
				nameProperty.serializedObject.ApplyModifiedProperties();
			});

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
			HashSet<AnimatorController> controllers = new HashSet<AnimatorController>();

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

			foreach (AnimatorController controller in controllers)
			{
				FindParameters(controller, parameters, isSingle, ParameterType);
			}
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
			StringBuilder sb = new StringBuilder();

			foreach (AnimatorControllerParameter parameter in controller.parameters)
			{
				if (parameter.type != type)
				{
					continue;
				}

				sb.Clear();
				if (!isSingle)
				{
					sb.Append($"{controller.name}/");
				}

				sb.Append(parameter.name);

				parameters.Add(new ParameterData(sb.ToString(), parameter.name));
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