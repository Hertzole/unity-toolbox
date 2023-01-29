#if TOOLBOX_SCRIPTABLE_VALUES
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomEditor(typeof(CursorManager))]
	public sealed class CursorManagerEditor : UnityEditor.Editor
	{
		private SerializedProperty lockCursor;
		private SerializedProperty matches;

		private void OnEnable()
		{
			lockCursor = serializedObject.FindProperty(nameof(lockCursor));
			matches = serializedObject.FindProperty(nameof(matches));
		}

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new VisualElement
			{
				name = "cursor-manager-root"
			};

			PropertyField lockCursorField = new PropertyField(lockCursor);
			lockCursorField.Bind(serializedObject);

			ManagedReferenceListView<IScriptableMatch> matchesField = new ManagedReferenceListView<IScriptableMatch>(matches)
			{
				name = "matches-list",
				headerTitle = "Matches",
				showBorder = true,
				showFoldoutHeader = true,
				showAddRemoveFooter = true,
				reorderable = true,
				reorderMode = ListViewReorderMode.Animated,
				selectionType = SelectionType.Single,
				virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
				makeItem = MakeListItem,
				bindItem = BindListItem
			};

			root.Add(lockCursorField);
			root.Add(VisualElementUtils.VerticalSpace());
			root.Add(matchesField);

			return root;
		}

		private void BindListItem(VisualElement root, int index)
		{
			PropertyField targetField = root.Q<PropertyField>("target");
			PropertyField valueField = root.Q<PropertyField>("value");

			targetField.BindProperty(matches.GetArrayElementAtIndex(index).FindPropertyRelative("target"));
			valueField.BindProperty(matches.GetArrayElementAtIndex(index).FindPropertyRelative("value"));
		}

		private static VisualElement MakeListItem()
		{
			VisualElement root = new VisualElement();

			PropertyField target = new PropertyField
			{
				name = "target"
			};

			PropertyField value = new PropertyField
			{
				name = "value"
			};

			root.Add(target);
			root.Add(value);

			return root;
		}
	}
}
#endif