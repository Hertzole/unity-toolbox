using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomPropertyDrawer(typeof(RandomFloat))]
	public sealed class RandomFloatDrawer : MinMaxDrawer
	{
		protected override VisualElement CreateGUI(SerializedProperty property, string label)
		{
			RandomFloatField field = new RandomFloatField(label)
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
			property.floatValue = EditorGUI.FloatField(position, label, property.floatValue);
		}
	}
}