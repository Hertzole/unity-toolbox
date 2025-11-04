#nullable enable

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Hertzole.UnityToolbox.Editor
{
    public class ManagedReferenceListView : ListView
    {
        private static readonly GUIContent noTypesFound = new GUIContent("No types found.");

        public static void ShowAddManagedObjectMenu<T>(SerializedProperty listProperty, Action? onTypeSelected = null)
        {
            GenericMenu menu = new GenericMenu();

            TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<T>();

            for (int i = 0; i < types.Count; i++)
            {
                Type type = types[i];

                if (type.IsAbstract || type.IsGenericType || type.IsInterface || !type.IsSerializable)
                {
                    continue;
                }

                // Ignore Unity types as they can not be managed references.
                if (typeof(Object).IsAssignableFrom(type))
                {
                    continue;
                }

                GUIContent typeName = new GUIContent(ObjectNames.NicifyVariableName(type.Name));

                menu.AddItem(typeName, false, OnClickAddItem, new MenuData(type, listProperty, onTypeSelected));
            }

            if (menu.GetItemCount() == 0)
            {
                menu.AddDisabledItem(noTypesFound);
                menu.ShowAsContext();
            }
            else
            {
                menu.ShowAsContext();
            }
        }

        private static void OnClickAddItem(object data)
        {
            MenuData menuData = (MenuData) data;
            Type type = menuData.type;
            SerializedProperty listProperty = menuData.listProperty;

            object instance = Activator.CreateInstance(type);
            menuData.listProperty.InsertArrayElementAtIndex(listProperty.arraySize);
            SerializedProperty element = listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);
            element.managedReferenceValue = instance;
            listProperty.serializedObject.ApplyModifiedProperties();

            menuData.onTypeSelected?.Invoke();
        }

        private readonly struct MenuData
        {
            public readonly Type type;
            public readonly SerializedProperty listProperty;
            public readonly Action? onTypeSelected;

            public MenuData(Type type, SerializedProperty listProperty, Action? onTypeSelected)
            {
                this.type = type;
                this.listProperty = listProperty;
                this.onTypeSelected = onTypeSelected;
            }
        }
    }
}