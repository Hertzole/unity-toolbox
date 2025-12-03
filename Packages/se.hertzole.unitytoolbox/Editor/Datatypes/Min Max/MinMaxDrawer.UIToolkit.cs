#if TOOLBOX_UXML_ATTRIBUTES // Because the fields use UxmlAttributes
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	public abstract class MinMaxDrawer<T, TValueType> : MinMaxDrawer
		where T : MinMaxField<TValueType>
	{
		/// <inheritdoc />
		protected sealed override VisualElement CreateGUI(SerializedProperty property, string label)
		{
			T field = CreateField(label);
			if (field == null)
			{
				return null;
			}

			BindMinMax(field, property.FindPropertyRelative("min"), property.FindPropertyRelative("max"));

			field.BindProperty(property);

			return field;
		}

		protected abstract T CreateField(string label);

		protected abstract void BindMinMax(T field, SerializedProperty min, SerializedProperty max);
	}
}
#endif