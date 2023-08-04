#if TOOLBOX_INPUT_SYSTEM && TOOLBOX_SCRIPTABLE_VALUES
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomEditor(typeof(Hertzole.UnityToolbox.InputManager))]
	public sealed class InputManagerEditor : UnityEditor.Editor
	{
		private SerializedProperty inputsList;
		private SerializedProperty enableOnStart;
		private SerializedProperty autoEnableNewInputs;
		private SerializedProperty autoDisableRemovedInputs;

		private void OnEnable()
		{
#if TOOLBOX_ADDRESSABLES
			useAddressables = serializedObject.FindProperty(nameof(useAddressables));
			inputsListReference = serializedObject.FindProperty(nameof(inputsListReference));
#endif
			inputsList = serializedObject.FindProperty(nameof(inputsList));
			enableOnStart = serializedObject.FindProperty(nameof(enableOnStart));
			autoEnableNewInputs = serializedObject.FindProperty(nameof(autoEnableNewInputs));
			autoDisableRemovedInputs = serializedObject.FindProperty(nameof(autoDisableRemovedInputs));
		}

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new VisualElement();

#if TOOLBOX_ADDRESSABLES
			PropertyField useAddressablesField = new PropertyField(useAddressables);
			useAddressablesField.BindProperty(useAddressables);

			PropertyField inputsListReferenceField = new PropertyField(inputsListReference)
			{
				style =
				{
					display = useAddressables.boolValue ? DisplayStyle.Flex : DisplayStyle.None
				}
			};

			inputsListReferenceField.BindProperty(inputsListReference);

#endif

			PropertyField inputsListField = new PropertyField(inputsList);
#if TOOLBOX_ADDRESSABLES
			inputsListField.style.display = useAddressables.boolValue ? DisplayStyle.None : DisplayStyle.Flex;
#endif
			inputsListField.BindProperty(inputsList);

			PropertyField enableOnStartField = new PropertyField(enableOnStart)
			{
				style =
				{
					marginTop = 8
				}
			};
			enableOnStartField.BindProperty(enableOnStart);
			PropertyField autoEnableNewInputsField = new PropertyField(autoEnableNewInputs);
			autoEnableNewInputsField.BindProperty(autoEnableNewInputs);
			PropertyField autoDisableRemovedInputsField = new PropertyField(autoDisableRemovedInputs);
			autoDisableRemovedInputsField.BindProperty(autoDisableRemovedInputs);

#if TOOLBOX_ADDRESSABLES
			root.Add(useAddressablesField);
			root.Add(inputsListReferenceField);
#endif

			root.Add(inputsListField);
			root.Add(enableOnStartField);
			root.Add(autoEnableNewInputsField);
			root.Add(autoDisableRemovedInputsField);

#if TOOLBOX_ADDRESSABLES
			useAddressablesField.RegisterCallback<SerializedPropertyChangeEvent, (VisualElement referenceField, VisualElement objectField)>((evt, fields) =>
			{
				if (evt.changedProperty.boolValue)
				{
					inputsList.objectReferenceValue = null;
					fields.referenceField.style.display = DisplayStyle.Flex;
					fields.objectField.style.display = DisplayStyle.None;
				}
				else
				{
					inputsListReference.FindPropertyRelative("m_AssetGUID").stringValue = null;
					fields.referenceField.style.display = DisplayStyle.None;
					fields.objectField.style.display = DisplayStyle.Flex;
				}
			}, (inputsListReferenceField, inputsListField));
#endif

			return root;
		}
#if TOOLBOX_ADDRESSABLES
		private SerializedProperty useAddressables;
		private SerializedProperty inputsListReference;
#endif
	}
}
#endif