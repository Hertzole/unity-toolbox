using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomPropertyDrawer(typeof(Identifier))]
	public sealed class IdentifierDrawer : ToolboxPropertyDrawer
	{
		protected override void DrawGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty prop = property.FindPropertyRelative("stringValue");
            prop.stringValue = EditorGUI.TextField(position, label, prop.stringValue);
		}

		protected override VisualElement CreateGUI(SerializedProperty property, string label)
		{
			SerializedProperty prop = property.FindPropertyRelative("stringValue");
			TextField field = new TextField(label);
			field.BindProperty(prop);

			return field;
		}
	}
}