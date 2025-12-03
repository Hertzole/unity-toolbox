#if TOOLBOX_UXML_ATTRIBUTES
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	[UxmlElement]
	public sealed partial class MinMaxSByteField : MinMaxField<MinMaxSByte, IntegerField, sbyte>
	{
		public MinMaxSByteField() : this(string.Empty) { }

		/// <inheritdoc />
		public MinMaxSByteField(string label) : base(label) { }

		/// <inheritdoc />
		protected override IntegerField CreateField(string fieldLabel)
		{
			IntegerField field = new IntegerField(fieldLabel);
			field.ClampValue(sbyte.MinValue, sbyte.MaxValue);
			return field;
		}

		/// <inheritdoc />
		protected override void SetFieldValueWithoutNotify(IntegerField field, sbyte newValue)
		{
			field.SetValueWithoutNotify(newValue);
		}
	}
}
#endif