using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	public static class InspectorExtensions
	{
		public static void MakePropertyField<T>(this BaseField<T> field)
		{
			if (!field.ClassListContains(BaseField<T>.alignedFieldUssClassName))
			{
				field.AddToClassList(BaseField<T>.alignedFieldUssClassName);
			}

			field.labelElement.AddToClassList(PropertyField.labelUssClassName);
		}
	}
}