#if TOOLBOX_UXML_ATTRIBUTES
using UnityEditor;
using UnityEditor.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomPropertyDrawer(typeof(MinMaxInt))]
	public sealed class MinMaxIntDrawer : MinMaxDrawer<MinMaxIntField, MinMaxInt>
	{
		/// <inheritdoc />
		protected override MinMaxIntField CreateField(string label)
		{
			return new MinMaxIntField(label);
		}

		/// <inheritdoc />
		protected override void BindMinMax(MinMaxIntField field, SerializedProperty min, SerializedProperty max)
		{
			field.MinField.BindProperty(min);
			field.MaxField.BindProperty(max);
		}
	}
}
#endif