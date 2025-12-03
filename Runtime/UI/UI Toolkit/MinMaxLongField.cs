#if TOOLBOX_UXML_ATTRIBUTES
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	[UxmlElement]
	public sealed partial class MinMaxLongField : MinMaxField<MinMaxLong, LongField, long>
	{
		public MinMaxLongField() : this(string.Empty) { }

		/// <inheritdoc />
		public MinMaxLongField(string label) : base(label) { }

		/// <inheritdoc />
		protected override LongField CreateField(string fieldLabel)
		{
			return new LongField(fieldLabel);
		}

		/// <inheritdoc />
		protected override void SetFieldValueWithoutNotify(LongField field, long newValue)
		{
			field.SetValueWithoutNotify(newValue);
		}
	}
}
#endif

