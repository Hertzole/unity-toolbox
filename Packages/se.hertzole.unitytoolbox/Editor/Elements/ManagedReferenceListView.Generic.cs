using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
    public class ManagedReferenceListView<T> : ManagedReferenceListView
    {
        private readonly SerializedProperty listProperty;

        public ManagedReferenceListView(SerializedProperty listProperty)
        {
            this.listProperty = listProperty;

            onAdd += ClickAddManaged;

            this.BindProperty(listProperty);
            this.Bind(listProperty.serializedObject);
        }

        private void ClickAddManaged(BaseListView listView)
        {
            ShowAddManagedObjectMenu<T>(listProperty, listView.RefreshItems);
        }

        public static ManagedReferenceListView<T> DefaultList(SerializedProperty property, string label)
        {
            return new ManagedReferenceListView<T>(property)
            {
                headerTitle = label,
                showBorder = true,
                showFoldoutHeader = true,
                showAddRemoveFooter = true,
                reorderable = true,
                reorderMode = ListViewReorderMode.Animated,
                selectionType = SelectionType.Single,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight
            };
        }
    }
}