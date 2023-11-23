using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	public sealed class RandomFloatField : MinMaxField<IMinMaxFloat, FloatField, float>
	{
		public RandomFloatField() : this(null) { }

		public RandomFloatField(string label) : base(label) { }

		public new class UxmlFactory : UxmlFactory<RandomFloatField, UxmlTraits> { }

		public new class UxmlTraits : BaseField<IMinMaxFloat>.UxmlTraits
		{
			private readonly UxmlFloatAttributeDescription minValue = new UxmlFloatAttributeDescription { name = "min-value" };
			private readonly UxmlFloatAttributeDescription maxValue = new UxmlFloatAttributeDescription { name = "max-value" };

			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				((RandomFloatField) ve).SetValueWithoutNotify(new RandomFloat(minValue.GetValueFromBag(bag, cc), maxValue.GetValueFromBag(bag, cc)));
			}
		}
	}
}