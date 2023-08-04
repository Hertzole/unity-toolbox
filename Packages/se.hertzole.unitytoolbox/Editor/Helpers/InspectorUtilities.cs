using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	public static class InspectorUtilities
	{
		public static void CreateDefaultInspector(SerializedObject serializedObject, VisualElement root, params string[] ignoreProperties)
		{
			SerializedProperty iterator = serializedObject.GetIterator();
			for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
			{
				if (ContainsString(ignoreProperties, iterator.propertyPath))
				{
					continue;
				}

				PropertyField field = new PropertyField(iterator);
				if (string.Equals("m_Script", iterator.propertyPath, StringComparison.Ordinal))
				{
					field.SetEnabled(false);
				}
				
				root.Add(field);
			}
		}
		
		private static bool ContainsString(IReadOnlyList<string> array, string value)
		{
			for (int i = 0; i < array.Count; i++)
			{
				if (string.Equals(array[i], value, StringComparison.Ordinal))
				{
					return true;
				}
			}

			return false;
		}
	}
}