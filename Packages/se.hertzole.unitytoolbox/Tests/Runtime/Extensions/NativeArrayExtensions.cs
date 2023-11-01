using NUnit.Framework;
using Unity.Collections;
using Unity.Jobs;
using MathRandom = Unity.Mathematics.Random;
using UnityRandom = UnityEngine.Random;
using SystemRandom = System.Random;
#if TOOLBOX_BURST
using Unity.Burst;
#endif

namespace Hertzole.UnityToolbox.Tests
{
	public class NativeArrayExtensions : BaseTest
	{
		[Test]
		public void GetLargest()
		{
			NativeArray<int> array = new NativeArray<int>(10, Allocator.Temp);

			for (int i = 0; i < 10; i++)
			{
				array[i] = i;
			}

			array.Shuffle();

			int largest = array.GetLargest();
			array.Dispose();
			Assert.AreEqual(9, largest);
		}

		[Test]
		public void GetSmallest()
		{
			NativeArray<int> array = new NativeArray<int>(10, Allocator.Temp);

			for (int i = 0; i < 10; i++)
			{
				array[i] = i;
			}

			array.Shuffle();

			int smallest = array.GetSmallest();
			array.Dispose();
			Assert.AreEqual(0, smallest);
		}

		[Test]
		public void GetLargest_Burst()
		{
			NativeArray<int> array = new NativeArray<int>(10, Allocator.TempJob);

			for (int i = 0; i < 10; i++)
			{
				array[i] = i;
			}

			array.Shuffle();

			NativeArray<int> result = new NativeArray<int>(1, Allocator.TempJob);

			GetLargestJob job = new GetLargestJob
			{
				array = array,
				result = result
			};

			job.Schedule().Complete();

			array.Dispose();

			int largest = result[0];
			result.Dispose();
			Assert.AreEqual(9, largest);
		}

		[Test]
		public void GetSmallest_Burst()
		{
			NativeArray<int> array = new NativeArray<int>(10, Allocator.TempJob);

			for (int i = 0; i < 10; i++)
			{
				array[i] = i;
			}

			array.Shuffle();

			NativeArray<int> result = new NativeArray<int>(1, Allocator.TempJob);

			GetSmallestJob job = new GetSmallestJob
			{
				array = array,
				result = result
			};

			job.Schedule().Complete();

			array.Dispose();

			int smallest = result[0];
			result.Dispose();
			Assert.AreEqual(0, smallest);
		}

		[Test]
		public void Shuffle_UnityRandom()
		{
			NativeArray<int> array = new NativeArray<int>(1000, Allocator.Temp);

			for (int i = 0; i < 1000; i++)
			{
				array[i] = i;
			}

			array.Shuffle();

			bool allEqual = true;

			for (int i = 0; i < 1000; i++)
			{
				if (array[i] != i)
				{
					allEqual = false;
					break;
				}
			}

			array.Dispose();
			Assert.IsFalse(allEqual);
		}

		[Test]
		public void Shuffle_MathRandom()
		{
			NativeArray<int> array = new NativeArray<int>(1000, Allocator.Temp);

			for (int i = 0; i < 1000; i++)
			{
				array[i] = i;
			}

			MathRandom random = new MathRandom(1234);
			array.Shuffle(ref random);

			bool allEqual = true;

			for (int i = 0; i < 1000; i++)
			{
				if (array[i] != i)
				{
					allEqual = false;
					break;
				}
			}

			array.Dispose();
			Assert.IsFalse(allEqual);
		}

		[Test]
		public void Shuffle_SystemRandom()
		{
			NativeArray<int> array = new NativeArray<int>(1000, Allocator.Temp);

			for (int i = 0; i < 1000; i++)
			{
				array[i] = i;
			}

			SystemRandom random = new SystemRandom(1234);
			array.Shuffle(random);

			bool allEqual = true;

			for (int i = 0; i < 1000; i++)
			{
				if (array[i] != i)
				{
					allEqual = false;
					break;
				}
			}

			array.Dispose();
			Assert.IsFalse(allEqual);
		}

		[Test]
		public void Shuffle_Burst()
		{
			NativeArray<int> array = new NativeArray<int>(1000, Allocator.TempJob);

			for (int i = 0; i < 1000; i++)
			{
				array[i] = i;
			}

			MathRandom random = new MathRandom(1234);
			ShuffleJob job = new ShuffleJob
			{
				array = array,
				random = random
			};

			job.Schedule().Complete();

			bool allEqual = true;

			for (int i = 0; i < 1000; i++)
			{
				if (array[i] != i)
				{
					allEqual = false;
					break;
				}
			}

			array.Dispose();
			Assert.IsFalse(allEqual);
		}

		[Test]
		public void GetRandom_UnityRandom()
		{
			NativeArray<int> array = new NativeArray<int>(1000, Allocator.Temp);

			for (int i = 0; i < 1000; i++)
			{
				array[i] = i;
			}

			int random = array.GetRandom();
			array.Dispose();
			Assert.GreaterOrEqual(random, 0);
			Assert.Less(random, 1000);
		}

		[Test]
		public void GetRandom_MathRandom()
		{
			NativeArray<int> array = new NativeArray<int>(1000, Allocator.Temp);

			for (int i = 0; i < 1000; i++)
			{
				array[i] = i;
			}

			MathRandom random = new MathRandom(1234);
			int randomInt = array.GetRandom(ref random);
			array.Dispose();
			Assert.GreaterOrEqual(randomInt, 0);
			Assert.Less(randomInt, 1000);
		}

		[Test]
		public void GetRandom_SystemRandom()
		{
			NativeArray<int> array = new NativeArray<int>(1000, Allocator.Temp);

			for (int i = 0; i < 1000; i++)
			{
				array[i] = i;
			}

			SystemRandom random = new SystemRandom(1234);
			int randomInt = array.GetRandom(random);
			array.Dispose();
			Assert.GreaterOrEqual(randomInt, 0);
			Assert.Less(randomInt, 1000);
		}

		[Test]
		public void GetRandom_Burst()
		{
			NativeArray<int> array = new NativeArray<int>(1000, Allocator.TempJob);
			NativeArray<int> result = new NativeArray<int>(1, Allocator.TempJob);

			for (int i = 0; i < 1000; i++)
			{
				array[i] = i;
			}

			MathRandom random = new MathRandom(1234);
			GetRandomJob job = new GetRandomJob
			{
				array = array,
				random = random,
				result = result
			};

			job.Schedule().Complete();

			int randomInt = job.result[0];
			array.Dispose();
			result.Dispose();
			Assert.GreaterOrEqual(randomInt, 0);
			Assert.Less(randomInt, 1000);
		}

		[Test]
		public void IsNullOrEmpty_NotCreated()
		{
			NativeArray<int> array = default;
			Assert.IsTrue(array.IsNullOrEmpty());
		}

		[Test]
		public void IsNullOrEmpty_CreatedButEmpty()
		{
			NativeArray<int> array = new NativeArray<int>(0, Allocator.Temp);

			bool isNullOrEmpty = array.IsNullOrEmpty();
			array.Dispose();
			Assert.IsTrue(isNullOrEmpty);
		}

		[Test]
		public void IsNullOrEmpty_CreatedAndNotEmpty()
		{
			NativeArray<int> array = new NativeArray<int>(1, Allocator.Temp);

			bool isNullOrEmpty = array.IsNullOrEmpty();
			array.Dispose();
			Assert.IsFalse(isNullOrEmpty);
		}

		[Test]
		public void IsNullOrEmpty_CreatedButEmpty_Burst()
		{
			NativeArray<int> array = new NativeArray<int>(0, Allocator.TempJob);
			NativeArray<bool> result = new NativeArray<bool>(1, Allocator.TempJob);

			IsNullOrEmptyJob job = new IsNullOrEmptyJob
			{
				array = array,
				result = result
			};

			job.Schedule().Complete();

			bool isNullOrEmpty = job.result[0];
			array.Dispose();
			result.Dispose();
			Assert.IsTrue(isNullOrEmpty);
		}

		[Test]
		public void IsNullOrEmpty_CreatedAndNotEmpty_Burst()
		{
			NativeArray<int> array = new NativeArray<int>(1, Allocator.TempJob);
			NativeArray<bool> result = new NativeArray<bool>(1, Allocator.TempJob);

			IsNullOrEmptyJob job = new IsNullOrEmptyJob
			{
				array = array,
				result = result
			};

			job.Schedule().Complete();

			bool isNullOrEmpty = job.result[0];
			array.Dispose();
			result.Dispose();
			Assert.IsFalse(isNullOrEmpty);
		}

#if TOOLBOX_BURST
		[BurstCompile(CompileSynchronously = true)]
#endif
		private struct GetLargestJob : IJob
		{
			public NativeArray<int> array;
			public NativeArray<int> result;

			public void Execute()
			{
				int smallest = array.GetLargest();
				result[0] = smallest;
			}
		}

#if TOOLBOX_BURST
		[BurstCompile(CompileSynchronously = true)]
#endif
		private struct GetSmallestJob : IJob
		{
			public NativeArray<int> array;
			public NativeArray<int> result;

			public void Execute()
			{
				int smallest = array.GetSmallest();
				result[0] = smallest;
			}
		}

#if TOOLBOX_BURST
		[BurstCompile(CompileSynchronously = true)]
#endif
		private struct ShuffleJob : IJob
		{
			public NativeArray<int> array;
			public MathRandom random;

			public void Execute()
			{
				array.Shuffle(ref random);
			}
		}

#if TOOLBOX_BURST
		[BurstCompile(CompileSynchronously = true)]
#endif
		private struct GetRandomJob : IJob
		{
			public NativeArray<int> array;
			public MathRandom random;
			public NativeArray<int> result;

			public void Execute()
			{
				int randomInt = array.GetRandom(ref random);
				result[0] = randomInt;
			}
		}

#if TOOLBOX_BURST
		[BurstCompile(CompileSynchronously = true)]
#endif
		private struct IsNullOrEmptyJob : IJob
		{
			public NativeArray<int> array;
			public NativeArray<bool> result;

			public void Execute()
			{
				result[0] = array.IsNullOrEmpty();
			}
		}
	}
}