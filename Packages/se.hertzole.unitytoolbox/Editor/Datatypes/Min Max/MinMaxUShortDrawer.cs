#if TOOLBOX_UXML_ATTRIBUTES
using UnityEditor;
using UnityEditor.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomPropertyDrawer(typeof(MinMaxUShort))]
	public sealed class MinMaxUShortDrawer : MinMaxDrawer<MinMaxUShortField, MinMaxUShort>
	{
		/// <inheritdoc />
		protected override MinMaxUShortField CreateField(string label)
		{
			return new MinMaxUShortField(label);
		}

		/// <inheritdoc />
		protected override void BindMinMax(MinMaxUShortField field, SerializedProperty min, SerializedProperty max)
		{
			field.MinField.BindProperty(min);
			field.MaxField.BindProperty(max);
		}
	}
}
#endif

