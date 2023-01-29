using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Hertzole.UnityToolbox.Editor
{
	public class ManagedReferenceListView<T> : ListView
	{
		private readonly SerializedProperty listProperty;

		private readonly GUIContent noTypesFound = new GUIContent("No types found.");

		public ManagedReferenceListView(SerializedProperty listProperty)
		{
			this.listProperty = listProperty;

			schedule.Execute(() =>
			{
				Button addButton = this.Q<Button>(ussClassName + "__add-button");
				addButton.clickable = new Clickable(ClickAddManaged);
			});

			this.BindProperty(listProperty);
		}

		private void ClickAddManaged()
		{
			GenericMenu menu = new GenericMenu();

			TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<T>();
			Type last = null;

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
				
				last = type;

				GUIContent typeName = new GUIContent(ObjectNames.NicifyVariableName(type.Name));

				menu.AddItem(typeName, false, () => { AddManagedItem(type); });
			}

			if (menu.GetItemCount() == 0)
			{
				menu.AddDisabledItem(noTypesFound);
				menu.ShowAsContext();
			}
			else if (menu.GetItemCount() == 1)
			{
				AddManagedItem(last);
			}
			else
			{
				menu.ShowAsContext();
			}
		}

		private void AddManagedItem(Type type)
		{
			object instance = Activator.CreateInstance(type);
			listProperty.InsertArrayElementAtIndex(listProperty.arraySize);
			SerializedProperty element = listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);
			element.managedReferenceValue = instance;
			listProperty.serializedObject.ApplyModifiedProperties();
			RefreshItems();
		}
	}
}