#if TOOLBOX_UXML_ATTRIBUTES
using System;
using UnityEditor.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	public abstract class MinMaxValueUxmlConverter<TValue, TType> : UxmlAttributeConverter<TValue>
		where TValue : unmanaged, IMinMax<TType> where TType : unmanaged
	{
		public override TValue FromString(string value)
		{
			ReadOnlySpan<char> span = value.AsSpan();
			int index = span.IndexOf(',');
			if (index == -1)
			{
				throw new FormatException("The string is not in the correct format.");
			}

			TType min = ParseValue(span.Slice(0, index));
			TType max = ParseValue(span.Slice(index + 1));

			return CreateValue(min, max);
		}

		protected abstract TType ParseValue(ReadOnlySpan<char> span);

		protected abstract TValue CreateValue(TType min, TType max);

		public override string ToString(TValue value)
		{
			return FormattableString.Invariant($"{value.Min},{value.Max}");
		}
	}
}
#endif