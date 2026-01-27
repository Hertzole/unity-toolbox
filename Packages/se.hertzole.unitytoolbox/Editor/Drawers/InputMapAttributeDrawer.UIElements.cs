#if TOOLBOX_INPUT_SYSTEM
#nullable enable

using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Pool;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
    partial class InputMapAttributeDrawer
    {
        [NonSerialized]
        private DropdownField? field;
        [NonSerialized]
        private PropertyValidState currentValidState;
        [NonSerialized]
        private SerializedProperty? inputProperty;

        private static readonly Func<string, string> formatCallback = FormatSelectedValueCallback;

        private PropertyValidState CurrentValidState
        {
            get { return currentValidState; }
            set
            {
                if (currentValidState != value)
                {
                    currentValidState = value;
                    UpdateFieldState();
                }
            }
        }

        /// <inheritdoc />
        protected override VisualElement CreateGUI(SerializedProperty property, string label)
        {
            if (fieldInfo.FieldType != typeof(string))
            {
                return new HelpBox(string.Format(invalidTypeMessageFormat, property.name), HelpBoxMessageType.Error);
            }

            InputMapAttribute inputMapAttribute = (InputMapAttribute) attribute;

            inputProperty = property.serializedObject.FindProperty(inputMapAttribute.PlayerInputPropertyName);

            mapOptionsList = ListPool<string>.Get();

            field = new DropdownField(mapOptionsList, 0, formatCallback, formatCallback)
            {
                label = property.displayName,
                tooltip = property.tooltip,
                userData = property
            };

            if (inputProperty != null)
            {
                field.TrackPropertyValue(inputProperty, serializedProperty => { CurrentValidState = GetValidState(serializedProperty); });
            }

            // Update every 16ms because we need to check if the inputProperty has actions assigned.
            field.schedule.Execute(() => { CurrentValidState = GetValidState(inputProperty); }).Every(16);

            field.RegisterValueChangedCallback(_ => { CurrentValidState = GetValidState(inputProperty); });

            field.BindProperty(property);

            UpdateFieldState();

            return field;
        }

        private void UpdateFieldState()
        {
            if (field == null)
            {
                return;
            }

            if (CurrentValidState != PropertyValidState.Valid)
            {
                field.SetEnabled(false);
                field.SetValueWithoutNotify(invalidValues[(int) CurrentValidState].text);
                return;
            }

            SerializedProperty? property = (SerializedProperty) field.userData;

            ReadOnlyArray<InputActionMap> actionMaps = ((PlayerInput) inputProperty!.objectReferenceValue).actions.actionMaps;

            mapOptions = GetMapOptions(mapOptions, in actionMaps, mapOptionsList);
            field.SetEnabled(true);
            field.SetValueWithoutNotify(GetSelectedValue(mapOptions, property.stringValue));
        }

        private static string FormatSelectedValueCallback(string value)
        {
            return string.IsNullOrEmpty(value) ? noneContent.text : value;
        }

        private static string GetSelectedValue(GUIContent[]? options, string value)
        {
            if (options == null || options.Length == 0)
            {
                return string.Empty;
            }

            for (int i = 0; i < options.Length; i++)
            {
                if (options[i].text == value)
                {
                    return options[i].text;
                }
            }

            return string.Empty;
        }
    }
}
#endif