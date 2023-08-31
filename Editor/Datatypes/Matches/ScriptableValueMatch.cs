using Hertzole.UnityToolbox.Matches;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomPropertyDrawer(typeof(ScriptableValueMatch<>), true)]
	public sealed class ScriptableValueMatch : ToolboxPropertyDrawer
	{
		private static readonly float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

		protected override void DrawGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Rect r = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
			property.isExpanded = EditorGUI.Foldout(r, property.isExpanded, label, true);
			if (property.isExpanded)
			{
				EditorGUI.indentLevel++;
				r.y += lineHeight;
				SerializedProperty target = property.FindPropertyRelative("target");
#if TOOLBOX_ADDRESSABLES
				SerializedProperty useAddressables = property.FindPropertyRelative("useAddressables");
				EditorGUI.PropertyField(r, useAddressables);
				r.y += lineHeight;
				if (useAddressables.boolValue)
				{
					EditorGUI.PropertyField(r, property.FindPropertyRelative("targetReference"));
					if (!Application.isPlaying)
					{
						target.objectReferenceValue = null;
					}
				}
				else
#endif
				{
					EditorGUI.PropertyField(r, target);
				}

				r.y += lineHeight;
				EditorGUI.PropertyField(r, property.FindPropertyRelative("mustMatchValue"));
				EditorGUI.indentLevel--;
			}
		}

		protected override VisualElement CreateGUI(SerializedProperty property, string label)
		{
			Foldout foldout = new Foldout
			{
				text = label,
				value = property.isExpanded
			};

			SerializedProperty target = property.FindPropertyRelative("target");
			PropertyField targetField = new PropertyField(target);
			PropertyField mustMatchValueField = new PropertyField(property.FindPropertyRelative("mustMatchValue"));

#if TOOLBOX_ADDRESSABLES
			SerializedProperty useAddressables = property.FindPropertyRelative("useAddressables");
			PropertyField useAddressablesField = new PropertyField(useAddressables);
			PropertyField targetReferenceField = new PropertyField(property.FindPropertyRelative("targetReference"));

			useAddressablesField.RegisterCallback<ChangeEvent<bool>, (PropertyField, PropertyField, SerializedProperty)>((evt, args) =>
			{
				args.Item1.style.display = evt.newValue ? DisplayStyle.Flex : DisplayStyle.None;
				args.Item2.style.display = evt.newValue ? DisplayStyle.None : DisplayStyle.Flex;

				if (evt.newValue)
				{
					args.Item3.objectReferenceValue = null;
					args.Item3.serializedObject.ApplyModifiedProperties();
				}
			}, (targetReferenceField, targetField, target));

			targetField.style.display = useAddressables.boolValue ? DisplayStyle.None : DisplayStyle.Flex;
			targetReferenceField.style.display = useAddressables.boolValue ? DisplayStyle.Flex : DisplayStyle.None;

			foldout.Add(useAddressablesField);
			foldout.Add(targetReferenceField);
#endif

			foldout.Add(targetField);
			foldout.Add(mustMatchValueField);

			return foldout;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (property.isExpanded)
			{
				return lineHeight * 4;
			}

			return lineHeight;
		}
	}
}