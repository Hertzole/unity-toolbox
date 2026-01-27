#if TOOLBOX_COLLECTIONS
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Collections;
using Unity.Mathematics;

namespace Hertzole.UnityToolbox.Tests
{
    partial class WeightedRandomTests
    {
        [Test]
        public void GetRandomIndex_MathematicsRandom()
        {
            // Arrange
            List<int> list = new List<int>(TestData);
            Random random = new Random(1);

            // Act
            RunIndexTest(list, ints => WeightedRandom.GetRandomIndex(ints, i => i, ref random));
        }

        [Test]
        public void GetRandomIndex_GenericEnumerable_MathematicsRandom()
        {
            // Arrange
            using NativeArray<int> list = new NativeArray<int>(TestData, Allocator.Temp);
            Random random = new Random(1);

            // Act
            RunIndexTest(list, ints => WeightedRandom.GetRandomIndex<int, NativeArray<int>>(ints, i => i, ref random));
        }

        [Test]
        public void GetRandomIndex_IEnumerable_MathematicsRandom()
        {
            // Arrange
            List<int> list = new List<int>(TestData);
            Random random = new Random(1);

            // Act
            RunIndexTest(list, ints => WeightedRandom.GetRandomIndex((IEnumerable<int>) ints, i => i, ref random));
        }

        [Test]
        public void GetRandom_MathematicsRandom()
        {
            // Arrange
            List<int> list = new List<int>(TestData);
            Random random = new Random(1);

            // Act
            RunRandomTest(list, ints => WeightedRandom.GetRandom(ints, i => i, ref random));
        }

        [Test]
        public void GetRandom_GenericEnumerable_MathematicsRandom()
        {
            // Arrange
            using NativeArray<int> list = new NativeArray<int>(TestData, Allocator.Temp);
            Random random = new Random(1);

            // Act
            RunRandomTest(list, ints => WeightedRandom.GetRandom<int, NativeArray<int>>(ints, i => i, ref random));
        }

        [Test]
        public void GetRandom_IEnumerable_MathematicsRandom()
        {
            // Arrange
            List<int> list = new List<int>(TestData);
            Random random = new Random(1);

            // Act
            RunRandomTest(list, ints => WeightedRandom.GetRandom((IEnumerable<int>) ints, i => i, ref random));
        }
    }
}
#endif