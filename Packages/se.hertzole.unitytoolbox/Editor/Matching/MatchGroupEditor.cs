using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Hertzole.UnityToolbox.Editor
{
    [CustomPropertyDrawer(typeof(MatchGroup))]
    public sealed partial class MatchGroupEditor : ToolboxPropertyDrawer
    {
        private ReorderableList list;
        private readonly GUIContent noTypesFound = new GUIContent("No types found.");

        /// <inheritdoc />
        protected override void DrawGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (list == null)
            {
                SerializedProperty matchersProperty = property.FindPropertyRelative("matchers");

                list = new ReorderableList(property.serializedObject, matchersProperty, true, false, true, true)
                {
                    onAddCallback = static l => { ManagedReferenceListView.ShowAddManagedObjectMenu<IMatcher>(l.serializedProperty); },
                    drawElementCallback = (rect1, index, active, focused) =>
                    {
                        EditorGUI.PropertyField(new Rect(rect1.x + 8, rect1.y, rect1.width, rect1.height), matchersProperty.GetArrayElementAtIndex(index));
                    },
                    elementHeightCallback = index => EditorGUI.GetPropertyHeight(matchersProperty.GetArrayElementAtIndex(index))
                };
            }

            Rect rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            GUIContent newContent = EditorGUI.BeginProperty(position, label, property);
            property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, newContent, true);
            if (!property.isExpanded)
            {
                return;
            }

            rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            list.DoList(rect);
            EditorGUI.EndProperty();
        }

        /// <inheritdoc />
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded)
            {
                // Add height for the list.
                return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("matchers"), label, true) + EditorGUIUtility.standardVerticalSpacing;
            }

            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}