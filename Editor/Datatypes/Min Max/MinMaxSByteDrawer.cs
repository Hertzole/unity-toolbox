#if TOOLBOX_UXML_ATTRIBUTES
using UnityEditor;
using UnityEditor.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomPropertyDrawer(typeof(MinMaxSByte))]
	public sealed class MinMaxSByteDrawer : MinMaxDrawer<MinMaxSByteField, MinMaxSByte>
	{
		/// <inheritdoc />
		protected override MinMaxSByteField CreateField(string label)
		{
			return new MinMaxSByteField(label);
		}

		/// <inheritdoc />
		protected override void BindMinMax(MinMaxSByteField field, SerializedProperty min, SerializedProperty max)
		{
			field.MinField.BindProperty(min);
			field.MaxField.BindProperty(max);
		}
	}
}
#endif