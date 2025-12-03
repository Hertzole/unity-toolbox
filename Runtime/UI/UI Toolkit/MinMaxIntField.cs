#if TOOLBOX_UXML_ATTRIBUTES
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	[UxmlElement]
	public sealed partial class MinMaxIntField : MinMaxField<MinMaxInt, IntegerField, int>
	{
		public MinMaxIntField() : this(string.Empty) { }
		
		/// <inheritdoc />
		public MinMaxIntField(string label) : base(label) { }

		/// <inheritdoc />
		protected override IntegerField CreateField(string fieldLabel)
		{
			return new IntegerField(fieldLabel);
		}

		/// <inheritdoc />
		protected override void SetFieldValueWithoutNotify(IntegerField field, int newValue)
		{
			field.SetValueWithoutNotify(newValue);
		}
	}
}
#endif