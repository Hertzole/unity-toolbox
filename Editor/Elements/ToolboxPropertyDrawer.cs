using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	/// <summary>
	///     Base class for toolbox property drawers. This class handles the tooltip and alignment of the property.
	/// </summary>
	public abstract class ToolboxPropertyDrawer : PropertyDrawer
	{
		protected virtual bool HandleTooltip { get { return true; } }

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			DrawGUI(position, property, label);
		}

		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			string label =
#if UNITY_2022_2_OR_NEWER
				preferredLabel;
			#else
				property.displayName;
			#endif
			
			VisualElement field = CreateGUI(property, label);
			if (field == null)
			{
				return null;
			}

			field.RegisterCallback<AttachToPanelEvent, VisualElement>((evt, args) =>
			{
				if (args.parent is PropertyField)
				{
					args.AddToClassList(BaseField<object>.alignedFieldUssClassName);
				}
			}, field);

			if (HandleTooltip && !string.IsNullOrEmpty(property.tooltip))
			{
				field.tooltip = property.tooltip;
			}

			return field;
		}

		protected abstract void DrawGUI(Rect position, SerializedProperty property, GUIContent label);
		
		protected abstract VisualElement CreateGUI(SerializedProperty property, string label);
	}
}