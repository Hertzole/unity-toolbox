using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.Pool;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
    partial class MatchGroupEditor
    {
        private static readonly ObjectPool<HelpBox> errorBoxPool =
            new ObjectPool<HelpBox>(static () => new HelpBox(), actionOnRelease: static box => box.RemoveFromHierarchy());
        private static readonly Func<VisualElement> makeItem = MakeItem;
        private static readonly Action<VisualElement, int> bindItem = BindItem;
        private static readonly Action<VisualElement, int> unbindItem = UnbindItem;

        internal const string MATCHER_LIST_ITEM_USER_DATA = "matcherListItem";

        /// <inheritdoc />
        protected override VisualElement CreateGUI(SerializedProperty property, string label)
        {
            VisualElement root = new VisualElement();

            ManagedReferenceListView<IMatcher> listView = ManagedReferenceListView<IMatcher>.DefaultList(property.FindPropertyRelative("matchers"), label);
            listView.makeItem += makeItem;
            listView.bindItem += bindItem;
            listView.unbindItem += unbindItem;
            listView.showBoundCollectionSize = false;

            root.Add(listView);

            return root;
        }

        private static void UnbindItem(VisualElement element, int index)
        {
            PropertyField propertyField = element.Q<PropertyField>();
            propertyField.Unbind();

            HelpBox helpBox = element.Q<HelpBox>();
            if (helpBox != null)
            {
                errorBoxPool.Release(helpBox);
            }
        }

        private static void BindItem(VisualElement element, int index)
        {
            ListView listView = element.QueryParent<ListView>();

            PropertyField propertyField = element.Q<PropertyField>();

            SerializedProperty property = (SerializedProperty) listView.itemsSource[index];

            if (property.managedReferenceValue == null)
            {
                HelpBox helpBox = errorBoxPool.Get();
                helpBox.text = "This matcher is null. Please remove it.";
                helpBox.messageType = HelpBoxMessageType.Error;
                propertyField.style.display = DisplayStyle.None;
                element.Add(helpBox);
                return;
            }

            propertyField.BindProperty(property);
            propertyField.style.display = DisplayStyle.Flex;
        }

        private static VisualElement MakeItem()
        {
            VisualElement root = new VisualElement();

            PropertyField propertyField = new PropertyField
            {
                userData = MATCHER_LIST_ITEM_USER_DATA
            };

            root.Add(propertyField);

            return root;
        }
    }
}