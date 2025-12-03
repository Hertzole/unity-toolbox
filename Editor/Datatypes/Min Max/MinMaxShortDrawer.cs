#if TOOLBOX_UXML_ATTRIBUTES
using UnityEditor;
using UnityEditor.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomPropertyDrawer(typeof(MinMaxShort))]
	public sealed class MinMaxShortDrawer : MinMaxDrawer<MinMaxShortField, MinMaxShort>
	{
		/// <inheritdoc />
		protected override MinMaxShortField CreateField(string label)
		{
			return new MinMaxShortField(label);
		}

		/// <inheritdoc />
		protected override void BindMinMax(MinMaxShortField field, SerializedProperty min, SerializedProperty max)
		{
			field.MinField.BindProperty(min);
			field.MaxField.BindProperty(max);
		}
	}
}
#endif