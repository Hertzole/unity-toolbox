#if TOOLBOX_SCRIPTABLE_VALUES
#nullable enable
using System.Text;
using Hertzole.UnityToolbox.Matches;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
    [CustomPropertyDrawer(typeof(ScriptableValueMatch<>), true)]
    public class ScriptableValueMatchDrawer : ToolboxPropertyDrawer
    {
        private static readonly float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        private static readonly StringBuilder nameBuilder = new StringBuilder();

        protected override void DrawGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using PooledObject<GUIContent> scope = guiContentPool.Get(out GUIContent? niceLabel);
            niceLabel.tooltip = label.tooltip;

            // Here we can assume that we're drawing inside MatchGroupEditor.
            niceLabel.text = property.depth > 1
                ? GetNiceName(property.managedReferenceValue, property.FindPropertyRelative("target").objectReferenceValue)
                : label.text;

            GUIContent? newLabel = EditorGUI.BeginProperty(position, niceLabel, property);

            Rect r = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(r, property.isExpanded, newLabel, true);
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

            EditorGUI.EndProperty();
        }

        protected override VisualElement CreateGUI(SerializedProperty property, string label)
        {
            Foldout foldout = new Foldout
            {
                text = label,
                value = property.isExpanded
            };

            foldout.RegisterExpandedChangedCallback(static (evt, args) => { args.isExpanded = evt.newValue; }, property);

            SerializedProperty target = property.FindPropertyRelative("target");

            foldout.RegisterCallback<AttachToPanelEvent, (SerializedProperty property, SerializedProperty target)>(static (evt, args) =>
            {
                Foldout foldout = (Foldout) evt.currentTarget;
                UpdateFoldoutText(foldout, args.property.managedReferenceValue, args.target.objectReferenceValue);
            }, (property, target));

            PropertyField targetField = new PropertyField(target);
            PropertyField mustMatchValueField = new PropertyField(property.FindPropertyRelative("mustMatchValue"));

            targetField.RegisterValueChangeCallback(
                static (evt, args) => { UpdateFoldoutText(args.foldout, args.property.managedReferenceValue, evt.changedProperty.objectReferenceValue); },
                (foldout, property));

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

        private static void UpdateFoldoutText(Foldout foldout, object managedValue, Object targetValue)
        {
            VisualElement parent = foldout.parent;
            if (parent == null || parent.userData is not string data || data != MatchGroupEditor.MATCHER_LIST_ITEM_USER_DATA)
            {
                return;
            }

            foldout.text = GetNiceName(managedValue, targetValue);
        }

        private static string GetNiceName(object managedValue, Object targetValue)
        {
            nameBuilder.Clear();
            nameBuilder.Append(ObjectNames.NicifyVariableName(managedValue.GetType().Name));
            nameBuilder.Append(" (");

            if (targetValue != null)
            {
                nameBuilder.Append(targetValue.name);
            }
            else
            {
                nameBuilder.Append("None");
            }

            nameBuilder.Append(')');

            return nameBuilder.ToString();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded)
            {
                int lines = 3;
#if TOOLBOX_ADDRESSABLES
	            lines++; // For the use addressables field.
				SerializedProperty useAddressables = property.FindPropertyRelative("useAddressables");
	            if (useAddressables.boolValue)
	            {
		            lines++; // For the target reference field.
	            }
#endif

                return lineHeight * lines;
            }

            return lineHeight;
        }
    }
}
#endif