#if TOOLBOX_UXML_ATTRIBUTES
using UnityEditor;
using UnityEditor.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomPropertyDrawer(typeof(MinMaxUInt))]
	public sealed class MinMaxUIntDrawer : MinMaxDrawer<MinMaxUIntField, MinMaxUInt>
	{
		/// <inheritdoc />
		protected override MinMaxUIntField CreateField(string label)
		{
			return new MinMaxUIntField(label);
		}

		/// <inheritdoc />
		protected override void BindMinMax(MinMaxUIntField field, SerializedProperty min, SerializedProperty max)
		{
			field.MinField.BindProperty(min);
			field.MaxField.BindProperty(max);
		}
	}
}
#endif

