#if TOOLBOX_UXML_ATTRIBUTES
using UnityEditor;
using UnityEditor.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomPropertyDrawer(typeof(MinMaxDouble))]
	public sealed class MinMaxDoubleDrawer : MinMaxDrawer<MinMaxDoubleField, MinMaxDouble>
	{
		/// <inheritdoc />
		protected override MinMaxDoubleField CreateField(string label)
		{
			return new MinMaxDoubleField(label);
		}

		/// <inheritdoc />
		protected override void BindMinMax(MinMaxDoubleField field, SerializedProperty min, SerializedProperty max)
		{
			field.MinField.BindProperty(min);
			field.MaxField.BindProperty(max);
		}
	}
}
#endif

