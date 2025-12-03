#if TOOLBOX_UXML_ATTRIBUTES
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	[UxmlElement]
	public sealed partial class MinMaxDoubleField : MinMaxField<MinMaxDouble, DoubleField, double>
	{
		public MinMaxDoubleField() : this(string.Empty) { }

		/// <inheritdoc />
		public MinMaxDoubleField(string label) : base(label) { }

		/// <inheritdoc />
		protected override DoubleField CreateField(string fieldLabel)
		{
			return new DoubleField(fieldLabel);
		}

		/// <inheritdoc />
		protected override void SetFieldValueWithoutNotify(DoubleField field, double newValue)
		{
			field.SetValueWithoutNotify(newValue);
		}
	}
}
#endif

