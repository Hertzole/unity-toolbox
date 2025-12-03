#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace Hertzole.UnityToolbox.Tests
{
	public abstract class BaseMinMaxTest<TMinMax, TValue>
		where TMinMax : unmanaged, IEquatable<TMinMax>, IFormattable, IMinMax<TValue>
		where TValue : unmanaged, IFormattable, IConvertible, IComparable<TValue>
	{
		private static readonly MethodInfo getMinMethod = typeof(TMinMax).GetProperty(nameof(IMinMax<TValue>.Min))!.GetMethod;
		private static readonly MethodInfo getMaxMethod = typeof(TMinMax).GetProperty(nameof(IMinMax<TValue>.Max))!.GetMethod;
		private static readonly MethodInfo deconstructMethod =
			typeof(TMinMax).GetMethod(nameof(IMinMax<TValue>.Deconstruct), BindingFlags.Instance | BindingFlags.Public)!;
		private static readonly MethodInfo equals = typeof(TMinMax).GetMethod(nameof(Equals), new[] { typeof(TMinMax) })!;
		private static readonly MethodInfo equalsObject = typeof(TMinMax).GetMethod(nameof(Equals), new[] { typeof(object) })!;
		private static readonly MethodInfo getHashCode = typeof(TMinMax).GetMethod(nameof(GetHashCode))!;
		private static readonly MethodInfo toString = typeof(TMinMax).GetMethod(nameof(ToString), Array.Empty<Type>())!;
		private static readonly MethodInfo toStringFormat = typeof(TMinMax).GetMethod(nameof(ToString), new[] { typeof(string), typeof(IFormatProvider) })!;

		public static IEnumerable<TestCaseData> ReadOnlyTestCases
		{
			get
			{
				yield return new TestCaseData(getMinMethod).SetName("Min property");
				yield return new TestCaseData(getMaxMethod).SetName("Max property");
				yield return new TestCaseData(deconstructMethod).SetName("Deconstruct method");
				yield return new TestCaseData(equals).SetName($"Equals({typeof(TMinMax).Name}) method");
				yield return new TestCaseData(equalsObject).SetName("Equals(object) method");
				yield return new TestCaseData(getHashCode).SetName("GetHashCode method");
				yield return new TestCaseData(toString).SetName("ToString() method");
				yield return new TestCaseData(toStringFormat).SetName("ToString(string, IFormatProvider) method");
			}
		}

		[Test]
		public void ToString_ReturnsCorrectString()
		{
			// Arrange
			TMinMax value = CreateRandomMinMax();

			// Act
			string result = value.ToString();

			// Assert
			Assert.That(result,
				Is.EqualTo(
					$"{typeof(TMinMax).Name} (Min: {value.Min.ToString("G", CultureInfo.CurrentCulture)}, Max: {value.Max.ToString("G", CultureInfo.CurrentCulture)})"));
		}

		[Test]
		public void ToString_Format_ReturnsCorrectString()
		{
			// Arrange
			TMinMax value = CreateRandomMinMax();
			const string format = "F2";
			CultureInfo culture = CultureInfo.InvariantCulture;

			// Act
			string result = value.ToString(format, culture);

			// Assert
			Assert.That(result, Is.EqualTo($"{typeof(TMinMax).Name} (Min: {value.Min.ToString(format, culture)}, Max: {value.Max.ToString(format, culture)})"));
		}

		[Test]
		public void Equals_SameValues_ReturnsTrue()
		{
			// Arrange
			TValue min = CreateRandomValue();
			TValue max = CreateRandomValue();
			TMinMax value1 = CreateMinMax(min, max);
			TMinMax value2 = CreateMinMax(min, max);

			// Act
			bool result = value1.Equals(value2);

			// Assert
			Assert.That(result, Is.True);
		}

		[Test]
		[Retry(5)] // Retry because random values might be the same. But unlikely to be the same 5 times in a row.
		public void Equals_DifferentValues_ReturnsFalse()
		{
			// Arrange
			TMinMax value1 = CreateRandomMinMax();
			TMinMax value2 = CreateRandomMinMax();

			// Act
			bool result = value1.Equals(value2);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test]
		public void Equals_Object_SameValues_ReturnsTrue()
		{
			// Arrange
			TValue min = CreateRandomValue();
			TValue max = CreateRandomValue();
			TMinMax value1 = CreateMinMax(min, max);
			object value2 = CreateMinMax(min, max);

			// Act
			bool result = value1.Equals(value2);

			// Assert
			Assert.That(result, Is.True);
		}

		[Test]
		public void Equals_Object_DifferentType_ReturnsFalse()
		{
			// Arrange
			TMinMax value1 = CreateRandomMinMax();
			object value2 = new object();

			// Act
			bool result = value1.Equals(value2);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test]
		public void GetHashCode_SameValues_ReturnsSameHashCode()
		{
			// Arrange
			TValue min = CreateRandomValue();
			TValue max = CreateRandomValue();
			TMinMax value1 = CreateMinMax(min, max);
			TMinMax value2 = CreateMinMax(min, max);

			// Act
			int hash1 = value1.GetHashCode();
			int hash2 = value2.GetHashCode();

			// Assert
			Assert.That(hash1, Is.EqualTo(hash2));
		}

		[Test]
		[Retry(5)] // Retry because random values might be the same. But unlikely to be the same 5 times in a row.
		public void GetHashCode_DifferentValues_ReturnsDifferentHashCode()
		{
			// Arrange
			TMinMax value1 = CreateRandomMinMax();
			TMinMax value2 = CreateRandomMinMax();

			// Act
			int hash1 = value1.GetHashCode();
			int hash2 = value2.GetHashCode();

			// Assert
			Assert.That(hash1, Is.Not.EqualTo(hash2));
		}

		[Test]
		public void Deconstruct_ReturnsCorrectValues()
		{
			// Arrange
			TValue min = CreateRandomValue();
			TValue max = CreateRandomValue();
			TMinMax value = CreateMinMax(min, max);

			// Act
			value.Deconstruct(out TValue deconstructedMin, out TValue deconstructedMax);

			// Assert
			Assert.That(deconstructedMin, Is.EqualTo(min));
			Assert.That(deconstructedMax, Is.EqualTo(max));
		}

		[Test]
		[TestCaseSource(nameof(ReadOnlyTestCases))]
		public void IsMemberReadOnly(MemberInfo member)
		{
			Assert.That(member.GetCustomAttribute<IsReadOnlyAttribute>(), Is.Not.Null, $"Member {member.Name} is not marked as read-only.");
		}

		[Test]
		public void Cast_FromTuple()
		{
			// Arrange
			TValue min = CreateRandomValue();
			TValue max = CreateRandomValue();
			(TValue min, TValue max) tuple = (min, max);
			TMinMax expected = CreateMinMax(min, max);

			// Act
			TMinMax result = FromTuple(tuple);

			// Assert
			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void GetRandomValue()
		{
			// Do multiple iterations to ensure randomness
			for (int i = 0; i < 100; i++)
			{
				// Arrange
				(TValue smallMin, TValue smallMax) = CreateSmallValues();
				TMinMax minMax = CreateMinMax(smallMin, smallMax);

				// Act
				TValue randomValue = GetRandomValue(minMax);

				// Assert
				Assert.That(randomValue, Is.LessThan(minMax.Max), "Random value is greater than Max.");
				Assert.That(randomValue, Is.GreaterThanOrEqualTo(minMax.Min), "Random value is less than Min.");
			}
		}

		private TMinMax CreateRandomMinMax()
		{
			TValue min = CreateRandomValue();
			TValue max = CreateRandomValue();
			return CreateMinMax(min, max);
		}

		protected abstract TValue CreateRandomValue();

		protected abstract TMinMax CreateMinMax(TValue min, TValue max);

		protected abstract TMinMax FromTuple((TValue min, TValue max) value);

		protected abstract (TValue min, TValue max) CreateSmallValues();

		protected abstract TValue GetRandomValue(TMinMax value);
	}
}