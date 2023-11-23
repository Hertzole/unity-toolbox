using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	public static class InspectorExtensions
	{
		public static void MakePropertyField<T>(this BaseField<T> field)
		{
			field.labelElement.AddToClassList(PropertyField.labelUssClassName);
		}
	}
}