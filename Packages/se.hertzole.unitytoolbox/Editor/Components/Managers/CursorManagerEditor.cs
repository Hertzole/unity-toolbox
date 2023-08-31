#if TOOLBOX_SCRIPTABLE_VALUES
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomEditor(typeof(CursorManager))]
	public sealed class CursorManagerEditor : MonoSingletonEditor
	{
		private SerializedProperty lockCursor;
		private SerializedProperty handleCursorLocking;
		private SerializedProperty matches;

		protected override bool CreateDefaultInspector
		{
			get { return false; }
		}

		protected override void OnEnable()
		{
			base.OnEnable();

#if TOOLBOX_ADDRESSABLES
			useAddressables = serializedObject.FindProperty(nameof(useAddressables));
			lockCursorReference = serializedObject.FindProperty(nameof(lockCursorReference));
#endif

			lockCursor = serializedObject.FindProperty(nameof(lockCursor));
			handleCursorLocking = serializedObject.FindProperty(nameof(handleCursorLocking));
			matches = serializedObject.FindProperty(nameof(matches));
		}

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = base.CreateInspectorGUI();

			PropertyField lockCursorField = new PropertyField(lockCursor);
			lockCursorField.Bind(serializedObject);
			PropertyField handleCursorLockingField = new PropertyField(handleCursorLocking);
			handleCursorLockingField.Bind(serializedObject);

#if TOOLBOX_ADDRESSABLES
			PropertyField useAddressablesField = new PropertyField(useAddressables);
			useAddressablesField.Bind(serializedObject);
			PropertyField lockCursorReferenceField = new PropertyField(lockCursorReference);
			lockCursorReferenceField.Bind(serializedObject);

			useAddressablesField.RegisterCallback<SerializedPropertyChangeEvent, (PropertyField, PropertyField)>((evt, args) =>
			{
				args.Item1.style.display = evt.changedProperty.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
				args.Item2.style.display = evt.changedProperty.boolValue ? DisplayStyle.None : DisplayStyle.Flex;
			}, (lockCursorReferenceField, lockCursorField));

			lockCursorField.style.display = useAddressables.boolValue ? DisplayStyle.None : DisplayStyle.Flex;
			lockCursorReferenceField.style.display = useAddressables.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
#endif

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
				virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight
			};

			root.Add(VisiaulElementUtilities.Header("Cursor Settings"));

#if TOOLBOX_ADDRESSABLES
			root.Add(useAddressablesField);
			root.Add(lockCursorReferenceField);
#endif
			root.Add(lockCursorField);
			root.Add(handleCursorLockingField);
			root.Add(VisiaulElementUtilities.VerticalSpace());
			root.Add(matchesField);

			return root;
		}
#if TOOLBOX_ADDRESSABLES
		private SerializedProperty useAddressables;
		private SerializedProperty lockCursorReference;
#endif
	}
}
#endif