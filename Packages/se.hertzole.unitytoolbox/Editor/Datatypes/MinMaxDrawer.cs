using UnityEditor;
using UnityEngine;

namespace Hertzole.UnityToolbox.Editor
{
	public abstract class MinMaxDrawer : ToolboxPropertyDrawer
	{
		private static readonly GUIContent maxFieldLabel = new GUIContent("Max");
		private static readonly GUIContent minFieldLabel = new GUIContent("Min");

		protected override void DrawGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			using (new LabelWidthScope(28))
			{
				Rect minRect = new Rect(position.x, position.y, position.width / 3 - 3, position.height);
				Rect maxRect = new Rect(position.x + position.width / 3 + 1, position.y, position.width / 3 - 2, position.height);

				DrawField(minRect, property.FindPropertyRelative("min"), minFieldLabel);
				DrawField(maxRect, property.FindPropertyRelative("max"), maxFieldLabel);
			}
			
			EditorGUI.EndProperty();
		}

		protected abstract void DrawField(Rect position, SerializedProperty property, GUIContent label);
	}
}