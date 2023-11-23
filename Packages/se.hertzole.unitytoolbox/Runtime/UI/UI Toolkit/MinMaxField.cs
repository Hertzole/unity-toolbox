using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	public abstract class MinMaxField<TValueType, TField, TFieldValue> : BaseField<TValueType>
		where TValueType : IMinMax<TFieldValue> where TField : BaseField<TFieldValue>, new()
	{
		private bool forceUpdateDisplay;

		public TFieldValue MinValue
		{
			get { return rawValue.Min; }
			set
			{
				if (EqualityComparer<TFieldValue>.Default.Equals(rawValue.Min, value))
				{
					return;
				}

				rawValue.Min = value;
				forceUpdateDisplay = true;
				UpdateDisplay();
			}
		}

		public TFieldValue MaxValue
		{
			get { return rawValue.Max; }
			set
			{
				if (EqualityComparer<TFieldValue>.Default.Equals(rawValue.Max, value))
				{
					return;
				}

				rawValue.Max = value;
				forceUpdateDisplay = true;
				UpdateDisplay();
			}
		}

		public TField MinField { get; }
		public TField MaxField { get; }

		protected MinMaxField(string label) : base(label, null)
		{
			labelElement.AddToClassList(BaseCompositeField<Vector2, FloatField, float>.labelUssClassName);

			AddToClassList("unity-min-max-field");
			AddToClassList(BaseCompositeField<Vector2, FloatField, float>.ussClassName);

			VisualElement input = this.Q<VisualElement>(className: inputUssClassName);
			input.AddToClassList("unity-min-max-field__input");
			input.AddToClassList(BaseCompositeField<Vector2, FloatField, float>.inputUssClassName);
			input.focusable = false;
			delegatesFocus = false;

			MinField = new TField
			{
				name = "min-input",
				label = "Min",
				delegatesFocus = true
			};

			MinField.labelElement.style.flexBasis = 28;
			MinField.labelElement.style.minWidth = 28;
			MinField.AddToClassList(BaseCompositeField<Vector2, FloatField, float>.fieldUssClassName);
			MinField.AddToClassList(BaseCompositeField<Vector2, FloatField, float>.firstFieldVariantUssClassName);

			MaxField = new TField
			{
				name = "max-input",
				label = "Max",
				delegatesFocus = true
			};

			MaxField.labelElement.style.flexBasis = 28;
			MaxField.labelElement.style.minWidth = 28;
			MaxField.AddToClassList(BaseCompositeField<Vector2, FloatField, float>.fieldUssClassName);

			VisualElement spacer = new VisualElement();
			spacer.AddToClassList(BaseCompositeField<Vector2, FloatField, float>.spacerUssClassName);

			input.Add(MinField);
			input.Add(MaxField);
			input.Add(spacer);
		}

		private void UpdateDisplay()
		{
			MinField.SetValueWithoutNotify(rawValue.Min);
			MaxField.SetValueWithoutNotify(rawValue.Max);
		}

		public override void SetValueWithoutNotify(TValueType newValue)
		{
			bool changed = forceUpdateDisplay || !EqualityComparer<TValueType>.Default.Equals(value, newValue);
			base.SetValueWithoutNotify(newValue);
			if (changed)
			{
				UpdateDisplay();
			}

			forceUpdateDisplay = false;
		}
	}
}