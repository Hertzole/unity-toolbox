#if TOOLBOX_UXML_ATTRIBUTES
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	[UxmlElement]
	public sealed partial class MinMaxULongField : MinMaxField<MinMaxULong, UnsignedLongField, ulong>
	{
		public MinMaxULongField() : this(string.Empty) { }

		/// <inheritdoc />
		public MinMaxULongField(string label) : base(label) { }

		/// <inheritdoc />
		protected override UnsignedLongField CreateField(string fieldLabel)
		{
			return new UnsignedLongField(fieldLabel);
		}

		/// <inheritdoc />
		protected override void SetFieldValueWithoutNotify(UnsignedLongField field, ulong newValue)
		{
			field.SetValueWithoutNotify(newValue);
		}
	}
}
#endif

