#if TOOLBOX_UXML_ATTRIBUTES
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	[UxmlElement]
	public sealed partial class MinMaxShortField : MinMaxField<MinMaxShort, IntegerField, short>
	{
		public MinMaxShortField() : this(string.Empty) { }

		/// <inheritdoc />
		public MinMaxShortField(string label) : base(label) { }

		/// <inheritdoc />
		protected override IntegerField CreateField(string fieldLabel)
		{
			IntegerField field = new IntegerField(fieldLabel);
			field.ClampValue(short.MinValue, short.MaxValue);
			return field;
		}

		/// <inheritdoc />
		protected override void SetFieldValueWithoutNotify(IntegerField field, short newValue)
		{
			field.SetValueWithoutNotify(newValue);
		}
	}
}
#endif

