using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomPropertyDrawer(typeof(Tag))]
	public sealed class TagDrawer : ToolboxPropertyDrawer
	{
		protected override void DrawGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty tagProperty = property.FindPropertyRelative("value");
			tagProperty.stringValue = EditorGUI.TagField(position, label, tagProperty.stringValue);

			if (tagProperty.stringValue != "" && !IsTag(tagProperty.stringValue))
			{
				tagProperty.stringValue = "Untagged";
				tagProperty.serializedObject.ApplyModifiedProperties();
			}
		}

		protected override VisualElement CreateGUI(SerializedProperty property)
		{
			SerializedProperty tagProperty = property.FindPropertyRelative("value");

			TagField field = new TagField(property.displayName);
			field.BindProperty(tagProperty);

			if (tagProperty.stringValue != "" && !IsTag(tagProperty.stringValue))
			{
				tagProperty.stringValue = "Untagged";
				tagProperty.serializedObject.ApplyModifiedProperties();
			}

			return field;
		}

		private static bool IsTag(in string tag)
		{
			string[] tags = InternalEditorUtility.tags;
			for (int i = 0; i < tags.Length; i++)
			{
				if (tags[i] == tag)
				{
					return true;
				}
			}

			return false;
		}
	}
}