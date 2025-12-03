#if TOOLBOX_UXML_ATTRIBUTES
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	[UxmlElement]
	public sealed partial class MinMaxUShortField : MinMaxField<MinMaxUShort, IntegerField, ushort>
	{
		public MinMaxUShortField() : this(string.Empty) { }

		/// <inheritdoc />
		public MinMaxUShortField(string label) : base(label) { }

		/// <inheritdoc />
		protected override IntegerField CreateField(string fieldLabel)
		{
			IntegerField field = new IntegerField(fieldLabel);
			field.ClampValue(ushort.MinValue, ushort.MaxValue);
			return field;
		}

		/// <inheritdoc />
		protected override void SetFieldValueWithoutNotify(IntegerField field, ushort newValue)
		{
			field.SetValueWithoutNotify(newValue);
		}
	}
}
#endif

