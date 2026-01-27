using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
    /// <summary>
    ///     Base class for toolbox property drawers. This class handles the tooltip and alignment of the property.
    /// </summary>
    public abstract class ToolboxPropertyDrawer : PropertyDrawer
    {
        protected static readonly ObjectPool<GUIContent> guiContentPool =
            new ObjectPool<GUIContent>(static () => new GUIContent(), actionOnRelease: static content =>
            {
                content.text = string.Empty;
                content.image = null;
                content.tooltip = string.Empty;
            }, collectionCheck: false);

        private static readonly EventCallback<AttachToPanelEvent, VisualElement> checkPropertyFieldChildCallback = static (_, args) =>
        {
            if (args.parent is PropertyField)
            {
                args.AddToClassList(BaseField<object>.alignedFieldUssClassName);
            }
        };

        protected virtual bool HandleTooltip
        {
            get { return true; }
        }

        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawGUI(position, property, label);
        }

        public sealed override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            string label =
#if UNITY_2022_2_OR_NEWER
                preferredLabel;
#else
				property.displayName;
#endif

            VisualElement field = CreateGUI(property, label);
            if (field == null)
            {
                return null;
            }

            // Check if a field is a child of a PropertyField. If so, it will add the aligned class so it aligns properly in the inspector.
            field.RegisterCallbackOnce(checkPropertyFieldChildCallback, field);

            if (HandleTooltip && !string.IsNullOrEmpty(property.tooltip))
            {
                field.tooltip = property.tooltip;
            }

            return field;
        }

        protected abstract void DrawGUI(Rect position, SerializedProperty property, GUIContent label);

        protected abstract VisualElement CreateGUI(SerializedProperty property, string label);
    }
}