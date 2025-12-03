#if TOOLBOX_UXML_ATTRIBUTES
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	[UxmlElement]
	public sealed partial class MinMaxUIntField : MinMaxField<MinMaxUInt, UnsignedIntegerField, uint>
	{
		public MinMaxUIntField() : this(string.Empty) { }

		/// <inheritdoc />
		public MinMaxUIntField(string label) : base(label) { }

		/// <inheritdoc />
		protected override UnsignedIntegerField CreateField(string fieldLabel)
		{
			return new UnsignedIntegerField(fieldLabel);
		}

		/// <inheritdoc />
		protected override void SetFieldValueWithoutNotify(UnsignedIntegerField field, uint newValue)
		{
			field.SetValueWithoutNotify(newValue);
		}
	}
}
#endif

