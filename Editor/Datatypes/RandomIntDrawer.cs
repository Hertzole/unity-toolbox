using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomPropertyDrawer(typeof(RandomInt))]
	public sealed class RandomIntDrawer : MinMaxDrawer
	{
		protected override VisualElement CreateGUI(SerializedProperty property, string label)
		{
			RandomIntField field = new RandomIntField(label)
			{
				bindingPath = property.propertyPath,
				MinField =
				{
					bindingPath = property.FindPropertyRelative("min").propertyPath
				},
				MaxField =
				{
					bindingPath = property.FindPropertyRelative("max").propertyPath
				},
				labelElement =
				{
					bindingPath = property.propertyPath
				}
			};

			field.Bind(property.serializedObject);

			field.MakePropertyField();

			return field;
		}

		protected override void DrawField(Rect position, SerializedProperty property, GUIContent label)
		{
			property.intValue = EditorGUI.IntField(position, label, property.intValue);
		}
	}
}