using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomPropertyDrawer(typeof(Layer))]
	public sealed class LayerDrawer : ToolboxPropertyDrawer
	{
		protected override void DrawGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty layerProperty = property.FindPropertyRelative("value");
			layerProperty.intValue = EditorGUI.LayerField(position, label, layerProperty.intValue);

			if (!IsLayer(layerProperty.intValue))
			{
				layerProperty.intValue = 0;
				layerProperty.serializedObject.ApplyModifiedProperties();
			}
		}

		protected override VisualElement CreateGUI(SerializedProperty property, string label)
		{
			SerializedProperty layerProperty = property.FindPropertyRelative("value");
			LayerField field = new LayerField(label, layerProperty.intValue);
			field.BindProperty(layerProperty);

			if (!IsLayer(layerProperty.intValue))
			{
				layerProperty.intValue = 0;
				layerProperty.serializedObject.ApplyModifiedProperties();
			}

			return field;
		}

		private static bool IsLayer(in int layer)
		{
			if (layer < 0 || layer >= 32)
			{
				return false;
			}

			return InternalEditorUtility.GetLayerName(layer).Length != 0;
		}
	}
}