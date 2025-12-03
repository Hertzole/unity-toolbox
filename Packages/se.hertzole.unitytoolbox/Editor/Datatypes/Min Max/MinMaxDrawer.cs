using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
#if !TOOLBOX_UXML_ATTRIBUTES // UXML Attributes version will handle the drawers
	[CustomPropertyDrawer(typeof(MinMaxByte))]
	[CustomPropertyDrawer(typeof(MinMaxSByte))]
	[CustomPropertyDrawer(typeof(MinMaxShort))]
	[CustomPropertyDrawer(typeof(MinMaxUShort))]
	[CustomPropertyDrawer(typeof(MinMaxInt))]
	[CustomPropertyDrawer(typeof(MinMaxUInt))]
	[CustomPropertyDrawer(typeof(MinMaxLong))]
	[CustomPropertyDrawer(typeof(MinMaxULong))]
	[CustomPropertyDrawer(typeof(MinMaxFloat))]
	[CustomPropertyDrawer(typeof(MinMaxDouble))]
#endif
	public class MinMaxDrawer : ToolboxPropertyDrawer
	{
		private static readonly GUIContent maxFieldLabel = new GUIContent("Max");
		private static readonly GUIContent minFieldLabel = new GUIContent("Min");

		protected sealed override void DrawGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			if (Event.current.alt)
			{
				label = new GUIContent(fieldInfo.GetValue(property.serializedObject.targetObject).GetHashCode().ToString());
			}

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			using (new LabelWidthScope(28))
			{
				Rect minRect = new Rect(position.x, position.y, position.width / 3 - 3, position.height);
				Rect maxRect = new Rect(position.x + position.width / 3 + 1, position.y, position.width / 3 - 2, position.height);

				EditorGUI.PropertyField(minRect, property.FindPropertyRelative("min"), minFieldLabel);
				EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("max"), maxFieldLabel);
			}

			EditorGUI.EndProperty();
		}

		protected override VisualElement CreateGUI(SerializedProperty property, string label)
		{
			return null;
		}
	}
}