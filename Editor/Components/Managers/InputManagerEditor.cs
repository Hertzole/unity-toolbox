#if TOOLBOX_INPUT_SYSTEM && TOOLBOX_SCRIPTABLE_VALUES
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomEditor(typeof(InputManager))]
	public sealed class InputManagerEditor : ToolboxEditor
	{
		private SerializedProperty inputsList;
		private SerializedProperty setInputActions;
		private SerializedProperty inputActions;
		private SerializedProperty enableOnStart;
		private SerializedProperty autoEnableNewInputs;
		private SerializedProperty autoDisableRemovedInputs;

		protected override void OnEnable()
		{
			base.OnEnable();
			
#if TOOLBOX_ADDRESSABLES
			useAddressables = serializedObject.FindProperty(nameof(useAddressables));
			inputsListReference = serializedObject.FindProperty(nameof(inputsListReference));
			inputActionsReference = serializedObject.FindProperty(nameof(inputActionsReference));
#endif
			inputsList = serializedObject.FindProperty(nameof(inputsList));
			setInputActions = serializedObject.FindProperty(nameof(setInputActions));
			inputActions = serializedObject.FindProperty(nameof(inputActions));
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

			PropertyField setInputActionsField = new PropertyField(setInputActions)
			{
				style =
				{
					marginTop = 8
				}
			};

			setInputActionsField.BindProperty(setInputActions);
			PropertyField inputActionsField = new PropertyField(inputActions);
			inputActionsField.BindProperty(inputActions);

#if TOOLBOX_ADDRESSABLES
			inputActionsField.style.display = setInputActions.boolValue && !useAddressables.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
			PropertyField inputActionsReferenceField = new PropertyField(inputsListReference)
			{
				style =
				{
					display = useAddressables.boolValue && setInputActions.boolValue ? DisplayStyle.Flex : DisplayStyle.None
				}
			};

			inputActionsReferenceField.BindProperty(inputActionsReference);
#else
			inputActionsField.style.display = setInputActions.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
#endif

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

			root.Add(setInputActionsField);
			root.Add(inputActionsField);
#if TOOLBOX_ADDRESSABLES
			root.Add(inputActionsReferenceField);
#endif

			root.Add(enableOnStartField);
			root.Add(autoEnableNewInputsField);
			root.Add(autoDisableRemovedInputsField);

#if TOOLBOX_ADDRESSABLES
			useAddressablesField
				.RegisterCallback<SerializedPropertyChangeEvent, (VisualElement referenceField, VisualElement objectField, SetInputActionsArgs)>(
					(evt, fields) =>
					{
						if (evt.changedProperty.boolValue)
						{
							inputsList.objectReferenceValue = null;
							fields.referenceField.style.display = DisplayStyle.Flex;
							fields.objectField.style.display = DisplayStyle.None;
							if (fields.Item3.setActionsProperty.boolValue)
							{
								fields.Item3.inputActionsReferenceField.style.display = DisplayStyle.Flex;
								fields.Item3.actionsField.style.display = DisplayStyle.None;
							}
						}
						else
						{
							inputsListReference.FindPropertyRelative("m_AssetGUID").stringValue = null;
							fields.referenceField.style.display = DisplayStyle.None;
							fields.objectField.style.display = DisplayStyle.Flex;
							if (fields.Item3.setActionsProperty.boolValue)
							{
								fields.Item3.inputActionsReferenceField.style.display = DisplayStyle.None;
								fields.Item3.actionsField.style.display = DisplayStyle.Flex;
							}
						}
					},
					(inputsListReferenceField, inputsListField,
					 new SetInputActionsArgs(setInputActions, inputActionsField, inputActionsReferenceField, useAddressables)));

			setInputActionsField.RegisterCallback<SerializedPropertyChangeEvent, SetInputActionsArgs>((evt, args) =>
			{
				args.actionsField.style.display = evt.changedProperty.boolValue && !args.useAddressables.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
				args.inputActionsReferenceField.style.display =
					evt.changedProperty.boolValue && args.useAddressables.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
			}, new SetInputActionsArgs(setInputActions, inputActionsField, inputActionsReferenceField, useAddressables));
#else
			setInputActionsField.RegisterCallback<SerializedPropertyChangeEvent, SetInputActionsArgs>((evt, args) =>
			{
				args.actionsField.style.display = evt.changedProperty.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
			}, new SetInputActionsArgs(setInputActions, inputActionsField));
#endif

			return root;
		}
		
		private readonly struct SetInputActionsArgs
		{
			public readonly SerializedProperty setActionsProperty;
			public readonly VisualElement actionsField;
#if TOOLBOX_ADDRESSABLES
			public readonly VisualElement inputActionsReferenceField;
			public readonly SerializedProperty useAddressables;
#endif

#if TOOLBOX_ADDRESSABLES
			public SetInputActionsArgs(SerializedProperty setActionsProperty,
				VisualElement actionsField,
				VisualElement inputActionsReferenceField,
				SerializedProperty useAddressables)
			{
				this.setActionsProperty = setActionsProperty;
				this.actionsField = actionsField;
				this.inputActionsReferenceField = inputActionsReferenceField;
				this.useAddressables = useAddressables;
			}
#else
			public SetInputActionsArgs(SerializedProperty setActionsProperty, VisualElement actionsField)
			{
				this.setActionsProperty = setActionsProperty;
				this.actionsField = actionsField;
			}
#endif
		}
#if TOOLBOX_ADDRESSABLES
		private SerializedProperty useAddressables;
		private SerializedProperty inputsListReference;
		private SerializedProperty inputActionsReference;
#endif
	}
}
#endif