using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomEditor(typeof(Spawnpoint))]
	internal sealed class SpawnpointEditor : UnityEditor.Editor
	{
		private SerializedProperty possibleRotations;
		private SerializedProperty spawnPosition;
		private SerializedProperty size;
		private SerializedProperty offset;
		private SerializedProperty boxColor;
		private SerializedProperty directionColor;

		private void OnEnable()
		{
			possibleRotations = serializedObject.FindProperty(nameof(possibleRotations));
			spawnPosition = serializedObject.FindProperty(nameof(spawnPosition));
			size = serializedObject.FindProperty(nameof(size));
			offset = serializedObject.FindProperty(nameof(offset));
			boxColor = serializedObject.FindProperty(nameof(boxColor));
			directionColor = serializedObject.FindProperty(nameof(directionColor));
#if TOOLBOX_SCRIPTABLE_VALUES
#if TOOLBOX_ADDRESSABLES
			useAddressables = serializedObject.FindProperty(nameof(useAddressables));
			spawnpointsListReference = serializedObject.FindProperty(nameof(spawnpointsListReference));
#endif // TOOLBOX_ADDRESSABLES
			spawnpointsList = serializedObject.FindProperty(nameof(spawnpointsList));
#endif // TOOLBOX_SCRIPTABLE_VALUES
		}

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new VisualElement();

			PropertyField spawnPositionField = new PropertyField(spawnPosition);
			PropertyField sizeField = new PropertyField(size);
			PropertyField offsetField = new PropertyField(offset);
			PropertyField boxColorField = new PropertyField(boxColor);
			PropertyField directionColorField = new PropertyField(directionColor)
			{
				style = { marginBottom = 6 }
			};

			PropertyField possibleRotationsField = new PropertyField(possibleRotations);

#if TOOLBOX_SCRIPTABLE_VALUES
#if TOOLBOX_ADDRESSABLES
			PropertyField useAddressablesField = new PropertyField(useAddressables);
			PropertyField spawnpointsListReferenceField = new PropertyField(spawnpointsListReference)
			{
				style = { marginBottom = 6 }
			};
#endif // TOOLBOX_ADDRESSABLES
			PropertyField spawnpointsListField = new PropertyField(spawnpointsList)
			{
				style = { marginBottom = 6 }
			};
#endif // TOOLBOX_SCRIPTABLE_VALUES

#if TOOLBOX_SCRIPTABLE_VALUES
#if TOOLBOX_ADDRESSABLES
			useAddressablesField.RegisterCallback<SerializedPropertyChangeEvent, (VisualElement, VisualElement)>((evt, args) => { },
				(spawnpointsListField, spawnpointsListReferenceField));

			useAddressablesField.RegisterValueChangeCallback(evt =>
			{
				UpdateAddressableFields(evt.changedProperty.boolValue, spawnpointsListReferenceField, spawnpointsListField);
			});

			root.Add(useAddressablesField);
			root.Add(spawnpointsListReferenceField);
#endif // TOOLBOX_ADDRESSABLES
			root.Add(spawnpointsListField);
#endif // TOOLBOX_SCRIPTABLE_VALUES

			root.Add(spawnPositionField);
			root.Add(InspectorUtilities.CreateHeader("Boundary Box"));
			root.Add(sizeField);
			root.Add(offsetField);
			root.Add(InspectorUtilities.CreateHeader("Gizmo Settings"));
			root.Add(boxColorField);
			root.Add(directionColorField);
			root.Add(possibleRotationsField);

			UpdateAddressableFields(useAddressables.boolValue, spawnpointsListReferenceField, spawnpointsListField);

			return root;
		}

#if TOOLBOX_SCRIPTABLE_VALUES && TOOLBOX_ADDRESSABLES
		private static void UpdateAddressableFields(bool useAddressables, VisualElement referenceField, VisualElement objectField)
		{
			referenceField.style.display = useAddressables ? DisplayStyle.Flex : DisplayStyle.None;
			objectField.style.display = useAddressables ? DisplayStyle.None : DisplayStyle.Flex;
		}
#endif

#if TOOLBOX_SCRIPTABLE_VALUES
#if TOOLBOX_ADDRESSABLES
		private SerializedProperty useAddressables;
		private SerializedProperty spawnpointsListReference;
#endif // TOOLBOX_ADDRESSABLES
		private SerializedProperty spawnpointsList;
#endif // TOOLBOX_SCRIPTABLE_VALUES
	}
}