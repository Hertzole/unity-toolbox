#if TOOLBOX_UXML_ATTRIBUTES
using UnityEditor;
using UnityEditor.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomPropertyDrawer(typeof(MinMaxFloat))]
	public sealed class MinMaxFloatDrawer : MinMaxDrawer<MinMaxFloatField, MinMaxFloat>
	{
		/// <inheritdoc />
		protected override MinMaxFloatField CreateField(string label)
		{
			return new MinMaxFloatField(label);
		}

		/// <inheritdoc />
		protected override void BindMinMax(MinMaxFloatField field, SerializedProperty min, SerializedProperty max)
		{
			field.MinField.BindProperty(min);
			field.MaxField.BindProperty(max);
		}
	}
}
#endif

