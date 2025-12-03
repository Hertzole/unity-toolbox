#if TOOLBOX_UXML_ATTRIBUTES
using UnityEditor;
using UnityEditor.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomPropertyDrawer(typeof(MinMaxULong))]
	public sealed class MinMaxULongDrawer : MinMaxDrawer<MinMaxULongField, MinMaxULong>
	{
		/// <inheritdoc />
		protected override MinMaxULongField CreateField(string label)
		{
			return new MinMaxULongField(label);
		}

		/// <inheritdoc />
		protected override void BindMinMax(MinMaxULongField field, SerializedProperty min, SerializedProperty max)
		{
			field.MinField.BindProperty(min);
			field.MaxField.BindProperty(max);
		}
	}
}
#endif

