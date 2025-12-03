#nullable enable

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
	public abstract class MinMaxField<TValueType> : BaseField<TValueType>
	{
		/// <inheritdoc />
		protected MinMaxField(string label) : base(label, null) { }
		
		public new static readonly string ussClassName = "hertzole-min-max-field";
		public new static readonly string inputUssClassName = ussClassName + "__input";
		public static readonly string minInputUssClassName = ussClassName + "__min-input";
		public static readonly string maxInputUssClassName = ussClassName + "__max-input";
	}
	
	public abstract class MinMaxField<TValueType, TField, TFieldValue> : MinMaxField<TValueType>
		where TValueType : IMinMax<TFieldValue> where TField : BindableElement, new()
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

				TValueType current = rawValue;
				current.Min = value;
				rawValue = current;
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

				TValueType current = rawValue;
				current.Max = value;
				rawValue = current;
				forceUpdateDisplay = true;
				UpdateDisplay();
			}
		}

		public TField MinField { get; }
		public TField MaxField { get; }

		protected MinMaxField(string label) : base(label)
		{
			labelElement.AddToClassList(BaseCompositeField<Vector2, FloatField, float>.labelUssClassName);

			AddToClassList(ussClassName);
			AddToClassList(BaseCompositeField<Vector2, FloatField, float>.ussClassName);

			VisualElement input = this.Q<VisualElement>(className: BaseField<int>.inputUssClassName);
			input.AddToClassList(inputUssClassName);
			input.AddToClassList(BaseCompositeField<Vector2, FloatField, float>.inputUssClassName);
			input.focusable = false;
			delegatesFocus = false;

			MinField = CreateField("Min");
			MinField.name = minInputUssClassName;
			MinField.delegatesFocus = true;

			if (MinField.TryGetLabelElement(out Label? minLabel))
			{
				minLabel.style.flexBasis = 28;
                minLabel.style.minWidth = 28;
			}

			MinField.AddToClassList(BaseCompositeField<Vector2, FloatField, float>.fieldUssClassName);
			MinField.AddToClassList(BaseCompositeField<Vector2, FloatField, float>.firstFieldVariantUssClassName);

			MaxField = CreateField("Max");
            MaxField.name = maxInputUssClassName;
			MaxField.delegatesFocus = true;
			
            if (MaxField.TryGetLabelElement(out Label? maxLabel))
            {
                maxLabel.style.flexBasis = 28;
                maxLabel.style.minWidth = 28;
            }

			MaxField.AddToClassList(BaseCompositeField<Vector2, FloatField, float>.fieldUssClassName);

			VisualElement spacer = new VisualElement();
			spacer.AddToClassList(BaseCompositeField<Vector2, FloatField, float>.spacerUssClassName);

			input.Add(MinField);
			input.Add(MaxField);
			input.Add(spacer);
		}

		private void UpdateDisplay()
		{
			SetFieldValueWithoutNotify(MinField, rawValue.Min);
            SetFieldValueWithoutNotify(MaxField, rawValue.Max);
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

		protected abstract TField CreateField(string fieldLabel);
		
		protected abstract void SetFieldValueWithoutNotify(TField field, TFieldValue newValue);
	}
}