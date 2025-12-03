#if TOOLBOX_UXML_ATTRIBUTES
using UnityEditor;
using UnityEditor.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomPropertyDrawer(typeof(MinMaxByte))]
	public sealed class MinMaxByteDrawer : MinMaxDrawer<MinMaxByteField, MinMaxByte>
	{
		/// <inheritdoc />
		protected override MinMaxByteField CreateField(string label)
		{
			return new MinMaxByteField(label);
		}

		/// <inheritdoc />
		protected override void BindMinMax(MinMaxByteField field, SerializedProperty min, SerializedProperty max)
		{
			field.MinField.BindProperty(min);
			field.MaxField.BindProperty(max);
		}
	}
}
#endif

