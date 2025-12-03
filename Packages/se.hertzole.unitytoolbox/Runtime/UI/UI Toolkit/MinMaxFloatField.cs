#if TOOLBOX_UXML_ATTRIBUTES
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	[UxmlElement]
	public sealed partial class MinMaxFloatField : MinMaxField<MinMaxFloat, FloatField, float>
	{
		public MinMaxFloatField() : this(string.Empty) { }

		/// <inheritdoc />
		public MinMaxFloatField(string label) : base(label) { }

		/// <inheritdoc />
		protected override FloatField CreateField(string fieldLabel)
		{
			return new FloatField(fieldLabel);
		}

		/// <inheritdoc />
		protected override void SetFieldValueWithoutNotify(FloatField field, float newValue)
		{
			field.SetValueWithoutNotify(newValue);
		}
	}
}
#endif

