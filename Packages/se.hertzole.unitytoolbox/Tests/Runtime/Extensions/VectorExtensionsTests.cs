using System.Collections;
using NUnit.Framework;
using UnityEngine;

namespace Hertzole.UnityToolbox.Tests
{
    public class VectorExtensionsTests
    {
        public static IEnumerable ToInt_Vector2Cases()
        {
            yield return new TestCaseData(new Vector2(1, 2f)).Returns(new Vector2Int(1, 2));
            yield return new TestCaseData(new Vector2(1.5f, 2.5f)).Returns(new Vector2Int(2, 2));
            yield return new TestCaseData(new Vector2(1.1f, 2.51f)).Returns(new Vector2Int(1, 3));
        }

        public static IEnumerable FloorToInt_Vector2Cases()
        {
            yield return new TestCaseData(new Vector2(1.9f, 2.51f)).Returns(new Vector2Int(1, 2));
            yield return new TestCaseData(new Vector2(1.5f, 2.5f)).Returns(new Vector2Int(1, 2));
            yield return new TestCaseData(new Vector2(1, 2f)).Returns(new Vector2Int(1, 2));
        }

        public static IEnumerable CecilToInt_Vector2Cases()
        {
            yield return new TestCaseData(new Vector2(1.1f, 2.9f)).Returns(new Vector2Int(2, 3));
            yield return new TestCaseData(new Vector2(1.5f, 2.5f)).Returns(new Vector2Int(2, 3));
            yield return new TestCaseData(new Vector2(1, 2f)).Returns(new Vector2Int(1, 2));
        }

        public static IEnumerable ToInt_Vector3Cases()
        {
            yield return new TestCaseData(new Vector3(1.1f, 2.51f, 3.9f)).Returns(new Vector3Int(1, 3, 4));
            yield return new TestCaseData(new Vector3(1.5f, 2.5f, 3.5f)).Returns(new Vector3Int(2, 2, 4));
            yield return new TestCaseData(new Vector3(1, 2f, 3f)).Returns(new Vector3Int(1, 2, 3));
        }

        public static IEnumerable FloorToInt_Vector3Cases()
        {
            yield return new TestCaseData(new Vector3(1.1f, 2.51f, 3.9f)).Returns(new Vector3Int(1, 2, 3));
            yield return new TestCaseData(new Vector3(1.5f, 2.5f, 3.5f)).Returns(new Vector3Int(1, 2, 3));
            yield return new TestCaseData(new Vector3(1, 2f, 3f)).Returns(new Vector3Int(1, 2, 3));
        }

        public static IEnumerable CecilToInt_Vector3Cases()
        {
            yield return new TestCaseData(new Vector3(1.1f, 2.9f, 3.9f)).Returns(new Vector3Int(2, 3, 4));
            yield return new TestCaseData(new Vector3(1.5f, 2.5f, 3.5f)).Returns(new Vector3Int(2, 3, 4));
            yield return new TestCaseData(new Vector3(1, 2f, 3f)).Returns(new Vector3Int(1, 2, 3));
        }

        [Test]
        [TestCaseSource(nameof(ToInt_Vector2Cases))]
        public Vector2Int ToInt_Vector2(Vector2 value)
        {
            return value.ToInt();
        }

        [Test]
        [TestCaseSource(nameof(FloorToInt_Vector2Cases))]
        public Vector2Int FloorToInt_Vector2(Vector2 value)
        {
            return value.FloorToInt();
        }

        [Test]
        [TestCaseSource(nameof(CecilToInt_Vector2Cases))]
        public Vector2Int CeilToInt_Vector2(Vector2 value)
        {
            return value.CeilToInt();
        }

        [Test]
        [TestCaseSource(nameof(ToInt_Vector3Cases))]
        public Vector3Int ToInt_Vector3(Vector3 value)
        {
            return value.ToInt();
        }

        [Test]
        [TestCaseSource(nameof(FloorToInt_Vector3Cases))]
        public Vector3Int FloorToInt_Vector3(Vector3 value)
        {
            return value.FloorToInt();
        }

        [Test]
        [TestCaseSource(nameof(CecilToInt_Vector3Cases))]
        public Vector3Int CeilToInt_Vector3(Vector3 value)
        {
            return value.CeilToInt();
        }
    }
}