using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	public sealed class RandomIntField : MinMaxField<IMinMaxInt, IntegerField, int>
	{
		public RandomIntField() : this(null) { }

		public RandomIntField(string label) : base(label) { }

		public new class UxmlFactory : UxmlFactory<RandomIntField, UxmlTraits> { }

		public new class UxmlTraits : BaseField<IMinMaxInt>.UxmlTraits
		{
			private readonly UxmlIntAttributeDescription minValue = new UxmlIntAttributeDescription { name = "min-value" };
			private readonly UxmlIntAttributeDescription maxValue = new UxmlIntAttributeDescription { name = "max-value" };

			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				((RandomIntField) ve).SetValueWithoutNotify(new RandomInt(minValue.GetValueFromBag(bag, cc), maxValue.GetValueFromBag(bag, cc)));
			}
		}
	}
}