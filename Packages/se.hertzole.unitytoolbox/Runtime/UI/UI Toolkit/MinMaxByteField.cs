#if TOOLBOX_UXML_ATTRIBUTES
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	[UxmlElement]
	public sealed partial class MinMaxByteField : MinMaxField<MinMaxByte, IntegerField, byte>
	{
		public MinMaxByteField() : this(string.Empty) { }

		/// <inheritdoc />
		public MinMaxByteField(string label) : base(label) { }

		/// <inheritdoc />
		protected override IntegerField CreateField(string fieldLabel)
		{
			IntegerField field = new IntegerField(fieldLabel);
			field.ClampValue(byte.MinValue, byte.MaxValue);
			return field;
		}

		/// <inheritdoc />
		protected override void SetFieldValueWithoutNotify(IntegerField field, byte newValue)
		{
			field.SetValueWithoutNotify(newValue);
		}
	}
}
#endif
