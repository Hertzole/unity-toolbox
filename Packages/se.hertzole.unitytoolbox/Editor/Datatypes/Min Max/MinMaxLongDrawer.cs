#if TOOLBOX_UXML_ATTRIBUTES
using UnityEditor;
using UnityEditor.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomPropertyDrawer(typeof(MinMaxLong))]
	public sealed class MinMaxLongDrawer : MinMaxDrawer<MinMaxLongField, MinMaxLong>
	{
		/// <inheritdoc />
		protected override MinMaxLongField CreateField(string label)
		{
			return new MinMaxLongField(label);
		}

		/// <inheritdoc />
		protected override void BindMinMax(MinMaxLongField field, SerializedProperty min, SerializedProperty max)
		{
			field.MinField.BindProperty(min);
			field.MaxField.BindProperty(max);
		}
	}
}
#endif

