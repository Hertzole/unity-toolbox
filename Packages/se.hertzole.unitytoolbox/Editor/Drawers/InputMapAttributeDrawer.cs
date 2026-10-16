#if TOOLBOX_INPUT_SYSTEM
#nullable enable

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Pool;

namespace Hertzole.UnityToolbox.Editor
{
    [CustomPropertyDrawer(typeof(InputMapAttribute))]
    public sealed partial class InputMapAttributeDrawer : ToolboxPropertyDrawer, IDisposable
    {
        private static readonly GUIContent[] invalidValues =
        {
            new GUIContent("Invalid input property"),
            new GUIContent("No input component assigned"),
            new GUIContent("Not player input"),
            new GUIContent("No input asset")
        };

        private static readonly GUIContent noneContent = new GUIContent("<None>");
        private static readonly string invalidTypeMessageFormat = $"'{{0}}' is not a string. {nameof(InputMapAttribute)} can only be used on strings.";

        [NonSerialized]
        private GUIContent[]? mapOptions;
        [NonSerialized]
        private List<string>? mapOptionsList;

        /// <inheritdoc />
        protected override void DrawGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (fieldInfo.FieldType != typeof(string))
            {
                position.height = EditorGUIUtility.singleLineHeight * 2;
                EditorGUI.HelpBox(position, string.Format(invalidTypeMessageFormat, property.name), MessageType.Error);
                return;
            }

            InputMapAttribute inputMapAttribute = (InputMapAttribute) attribute;

            inputProperty = property.serializedObject.FindProperty(inputMapAttribute.PlayerInputPropertyName);

            label = EditorGUI.BeginProperty(position, label, property);

            PropertyValidState validState = GetValidState(inputProperty);

            if (validState != PropertyValidState.Valid)
            {
                bool oEnabled = GUI.enabled;
                GUI.enabled = false;
                EditorGUI.Popup(position, label, (int) validState, invalidValues);
                GUI.enabled = oEnabled;
            }
            else
            {
                DrawPopupGUI(position, property, label);
            }

            EditorGUI.EndProperty();
        }

        private void DrawPopupGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ReadOnlyArray<InputActionMap> actionMaps = ((PlayerInput) inputProperty!.objectReferenceValue).actions.actionMaps;

            mapOptions = GetMapOptions(mapOptions, in actionMaps);

            int selected = GetSelectedIndex(mapOptions!, property.stringValue);

            EditorGUI.BeginChangeCheck();
            selected = EditorGUI.Popup(position, label, selected, mapOptions);

            if (EditorGUI.EndChangeCheck())
            {
                property.stringValue = selected == 0 ? string.Empty : mapOptions[selected]!.text;
            }
        }

        /// <inheritdoc />
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (fieldInfo.FieldType != typeof(string))
            {
                return EditorGUIUtility.singleLineHeight * 2;
            }

            return EditorGUIUtility.singleLineHeight;
        }

        private static GUIContent[] GetMapOptions(GUIContent[]? existingOptions, in ReadOnlyArray<InputActionMap> actionMaps, List<string>? optionsList = null)
        {
            if (existingOptions == null || existingOptions.Length != actionMaps.Count + 1) // +1 because of the none item.
            {
                existingOptions = new GUIContent[actionMaps.Count + 1];
                for (int i = 0; i < existingOptions.Length; i++)
                {
                    existingOptions[i] = i > 0 ? guiContentPool.Get() : noneContent;
                }
            }

            for (int i = 0; i < existingOptions.Length; i++)
            {
                if (i == 0)
                {
                    optionsList?.Add(string.Empty);
                    continue;
                }

                existingOptions[i].text = actionMaps[i - 1].name;

                optionsList?.Add(actionMaps[i - 1].name);
            }

            return existingOptions!;
        }

        private static PropertyValidState GetValidState(SerializedProperty? property)
        {
            if (property == null)
            {
                return PropertyValidState.InvalidProperty;
            }

            if (property.objectReferenceValue == null)
            {
                return PropertyValidState.NotAssigned;
            }

            if (property.objectReferenceValue is not PlayerInput input)
            {
                return PropertyValidState.NotPlayerInput;
            }

            if (input.actions == null)
            {
                return PropertyValidState.NoInputAsset;
            }

            return PropertyValidState.Valid;
        }

        private static int GetSelectedIndex(GUIContent[] options, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            for (int i = 0; i < options.Length; i++)
            {
                if (options[i].text == value)
                {
                    return i;
                }
            }

            return 0;
        }

        private enum PropertyValidState
        {
            Valid = 100,
            InvalidProperty = 0,
            NotAssigned = 1,
            NotPlayerInput = 2,
            NoInputAsset = 3
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (mapOptionsList != null)
            {
                ListPool<string>.Release(mapOptionsList);
                mapOptionsList = null;
            }

            if (mapOptions != null)
            {
                // Start at 1 because 0 is the static none content.
                for (int i = 1; i < mapOptions.Length; i++)
                {
                    guiContentPool.Release(mapOptions[i]);
                }
            }
        }
    }
}
#endif