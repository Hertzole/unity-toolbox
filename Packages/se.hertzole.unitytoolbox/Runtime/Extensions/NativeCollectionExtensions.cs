using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using MathRandom = Unity.Mathematics.Random;
using UnityRandom = UnityEngine.Random;
using SystemRandom = System.Random;
#if TOOLBOX_BURST
using Unity.Burst;
#endif

namespace Hertzole.UnityToolbox
{
#if TOOLBOX_BURST
	[BurstCompile]
#if TOOLBOX_COLLECTIONS_2
	[GenerateTestsForBurstCompatibility]
#elif TOOLBOX_COLLECTIONS
	[BurstCompatible]
#endif
#endif
	public static class NativeCollectionExtensions
	{
#if TOOLBOX_BURST
		[BurstDiscard]
#endif
		public static void Shuffle<T>(this NativeArray<T> array) where T : struct
		{
			if (array.Length == 0)
			{
				return;
			}

			for (int i = 0; i < array.Length; i++)
			{
				T temp = array[i];
				int randomIndex = UnityRandom.Range(0, array.Length);
				array[i] = array[randomIndex];
				array[randomIndex] = temp;
			}
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static void Shuffle<T>(this NativeArray<T> array, ref MathRandom random) where T : struct
		{
			if (array.Length == 0)
			{
				return;
			}

			for (int i = 0; i < array.Length; i++)
			{
				T temp = array[i];
				int randomIndex = random.NextInt(array.Length);
				array[i] = array[randomIndex];
				array[randomIndex] = temp;
			}
		}

#if TOOLBOX_BURST
		[BurstDiscard]
#endif
		public static void Shuffle<T>(this NativeArray<T> array, SystemRandom random) where T : struct
		{
			if (array.Length == 0)
			{
				return;
			}

			for (int i = 0; i < array.Length; i++)
			{
				T temp = array[i];
				int randomIndex = random.Next(array.Length);
				array[i] = array[randomIndex];
				array[randomIndex] = temp;
			}
		}

#if TOOLBOX_BURST
		[BurstDiscard]
#endif
		public static T GetRandom<T>(this NativeArray<T> array) where T : struct
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (array.Length == 0)
			{
				throw new IndexOutOfRangeException("Array is empty.");
			}
#endif

			return array[UnityRandom.Range(0, array.Length)];
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static T GetRandom<T>(this NativeArray<T> array, ref MathRandom random) where T : struct
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (array.Length == 0)
			{
				throw new IndexOutOfRangeException("Array is empty.");
			}
#endif

			return array[random.NextInt(array.Length)];
		}

#if TOOLBOX_BURST
		[BurstDiscard]
#endif
		public static T GetRandom<T>(this NativeArray<T> array, SystemRandom random) where T : struct
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (array.Length == 0)
			{
				throw new IndexOutOfRangeException("Array is empty.");
			}
#endif

			return array[random.Next(array.Length)];
		}

#if TOOLBOX_BURST
		[BurstDiscard]
#endif
		public static ref T GetRandomRef<T>(this NativeArray<T> array) where T : struct
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (array.Length == 0)
			{
				throw new IndexOutOfRangeException("Array is empty.");
			}
#endif

			return ref array.GetRef(UnityRandom.Range(0, array.Length));
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static ref T GetRandomRef<T>(this NativeArray<T> array, ref MathRandom random) where T : struct
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (array.Length == 0)
			{
				throw new IndexOutOfRangeException("Array is empty.");
			}
#endif

			return ref array.GetRef(random.NextInt(array.Length));
		}

#if TOOLBOX_BURST
		[BurstDiscard]
#endif
		public static ref T GetRandomRef<T>(this NativeArray<T> array, SystemRandom random) where T : struct
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (array.Length == 0)
			{
				throw new IndexOutOfRangeException("Array is empty.");
			}
#endif

			return ref array.GetRef(random.Next(array.Length));
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static T GetSmallest<T>(this NativeArray<T> array) where T : struct, IComparable<T>
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (array.Length == 0)
			{
				throw new IndexOutOfRangeException("Array is empty.");
			}
#endif

			ref T smallest = ref array.GetRef(0);
			for (int i = 1; i < array.Length; i++)
			{
				ref T current = ref array.GetRef(i);

				if (current.CompareTo(smallest) < 0)
				{
					smallest = current;
				}
			}

			return smallest;
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static T GetLargest<T>(this NativeArray<T> array) where T : struct, IComparable<T>
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (array.Length == 0)
			{
				throw new IndexOutOfRangeException("Array is empty.");
			}
#endif

			ref T largest = ref array.GetRef(0);
			for (int i = 1; i < array.Length; i++)
			{
				ref T current = ref array.GetRef(i);

				if (current.CompareTo(largest) > 0)
				{
					largest = current;
				}
			}

			return largest;
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static ref T GetSmallestRef<T>(this NativeArray<T> array) where T : struct, IComparable<T>
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (array.Length == 0)
			{
				throw new IndexOutOfRangeException("Array is empty.");
			}
#endif

			ref T smallest = ref array.GetRef(0);
			for (int i = 1; i < array.Length; i++)
			{
				ref T current = ref array.GetRef(i);

				if (current.CompareTo(smallest) < 0)
				{
					smallest = current;
				}
			}

			return ref smallest;
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static ref T GetLargestRef<T>(this NativeArray<T> array) where T : struct, IComparable<T>
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (array.Length == 0)
			{
				throw new IndexOutOfRangeException("Array is empty.");
			}
#endif

			ref T largest = ref array.GetRef(0);
			for (int i = 1; i < array.Length; i++)
			{
				ref T current = ref array.GetRef(i);

				if (current.CompareTo(largest) > 0)
				{
					largest = current;
				}
			}

			return ref largest;
		}

#if TOOLBOX_BURST
		[BurstDiscard]
#endif
		public static string ToCommaSeparatedString<T>(this NativeArray<T> array) where T : struct
		{
			return array.Length == 0 ? string.Empty : string.Join(", ", array);
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static bool IsNullOrEmpty<T>(this NativeArray<T> array) where T : struct
		{
			return !array.IsCreated || array.Length == 0;
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static bool IsNullOrEmpty<T>(this NativeArray<T>? array) where T : struct
		{
			return !array.HasValue || !array.Value.IsCreated || array.Value.Length == 0;
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static ref T GetRef<T>(this NativeArray<T> array, int index) where T : struct
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			// Important! Validate this index first, or else the application will hard crash!
			if (index < 0 || index >= array.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}
#endif
			unsafe
			{
				return ref UnsafeUtility.ArrayElementAsRef<T>(array.GetUnsafePtr(), index);
			}
		}

#if TOOLBOX_COLLECTIONS
#if TOOLBOX_BURST
		[BurstDiscard]
#endif
		public static void Shuffle<T>(this NativeList<T> array) where T : unmanaged
		{
			if (array.Length == 0)
			{
				return;
			}

			for (int i = 0; i < array.Length; i++)
			{
				T temp = array[i];
				int randomIndex = UnityRandom.Range(0, array.Length);
				array[i] = array[randomIndex];
				array[randomIndex] = temp;
			}
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static void Shuffle<T>(this NativeList<T> array, ref MathRandom random) where T : unmanaged
		{
			if (array.Length == 0)
			{
				return;
			}

			for (int i = 0; i < array.Length; i++)
			{
				T temp = array[i];
				int randomIndex = random.NextInt(array.Length);
				array[i] = array[randomIndex];
				array[randomIndex] = temp;
			}
		}

#if TOOLBOX_BURST
		[BurstDiscard]
#endif
		public static void Shuffle<T>(this NativeList<T> array, SystemRandom random) where T : unmanaged
		{
			if (array.Length == 0)
			{
				return;
			}

			for (int i = 0; i < array.Length; i++)
			{
				T temp = array[i];
				int randomIndex = random.Next(array.Length);
				array[i] = array[randomIndex];
				array[randomIndex] = temp;
			}
		}

#if TOOLBOX_BURST
		[BurstDiscard]
#endif
		public static T GetRandom<T>(this NativeList<T> list) where T : unmanaged
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (list.Length == 0)
			{
				throw new IndexOutOfRangeException("List is empty.");
			}
#endif

			return list[UnityRandom.Range(0, list.Length)];
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static T GetRandom<T>(this NativeList<T> list, ref MathRandom random) where T : unmanaged
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (list.Length == 0)
			{
				throw new IndexOutOfRangeException("List is empty.");
			}
#endif

			return list[random.NextInt(list.Length)];
		}

#if TOOLBOX_BURST
		[BurstDiscard]
#endif
		public static T GetRandom<T>(this NativeList<T> list, SystemRandom random) where T : unmanaged
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (list.Length == 0)
			{
				throw new IndexOutOfRangeException("List is empty.");
			}
#endif

			return list[random.Next(list.Length)];
		}

#if TOOLBOX_BURST
		[BurstDiscard]
#endif
		public static ref T GetRandomRef<T>(this NativeList<T> list) where T : unmanaged
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (list.Length == 0)
			{
				throw new IndexOutOfRangeException("List is empty.");
			}
#endif

			return ref list.GetRef(UnityRandom.Range(0, list.Length));
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static ref T GetRandomRef<T>(this NativeList<T> list, ref MathRandom random) where T : unmanaged
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (list.Length == 0)
			{
				throw new IndexOutOfRangeException("List is empty.");
			}
#endif

			return ref list.GetRef(random.NextInt(list.Length));
		}

#if TOOLBOX_BURST
		[BurstDiscard]
#endif
		public static ref T GetRandomRef<T>(this NativeList<T> list, SystemRandom random) where T : unmanaged
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (list.Length == 0)
			{
				throw new IndexOutOfRangeException("List is empty.");
			}
#endif

			return ref list.GetRef(random.Next(list.Length));
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static T GetSmallest<T>(this NativeList<T> list) where T : unmanaged, IComparable<T>
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (list.Length == 0)
			{
				throw new IndexOutOfRangeException("List is empty.");
			}
#endif

			ref T smallest = ref list.GetRef(0);
			for (int i = 1; i < list.Length; i++)
			{
				ref T current = ref list.GetRef(i);

				if (current.CompareTo(smallest) < 0)
				{
					smallest = current;
				}
			}

			return smallest;
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static ref T GetSmallestRef<T>(this NativeList<T> list) where T : unmanaged, IComparable<T>
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (list.Length == 0)
			{
				throw new IndexOutOfRangeException("List is empty.");
			}
#endif

			ref T smallest = ref list.GetRef(0);
			for (int i = 1; i < list.Length; i++)
			{
				ref T current = ref list.GetRef(i);

				if (current.CompareTo(smallest) < 0)
				{
					smallest = current;
				}
			}

			return ref smallest;
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static T GetLargest<T>(this NativeList<T> list) where T : unmanaged, IComparable<T>
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (list.Length == 0)
			{
				throw new IndexOutOfRangeException("List is empty.");
			}
#endif

			ref T largest = ref list.GetRef(0);
			for (int i = 1; i < list.Length; i++)
			{
				ref T current = ref list.GetRef(i);

				if (current.CompareTo(largest) > 0)
				{
					largest = current;
				}
			}

			return largest;
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static ref T GetLargestRef<T>(this NativeList<T> list) where T : unmanaged, IComparable<T>
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			if (list.Length == 0)
			{
				throw new IndexOutOfRangeException("List is empty.");
			}
#endif

			ref T largest = ref list.GetRef(0);
			for (int i = 1; i < list.Length; i++)
			{
				ref T current = ref list.GetRef(i);

				if (current.CompareTo(largest) > 0)
				{
					largest = current;
				}
			}

			return ref largest;
		}

#if TOOLBOX_BURST
		[BurstDiscard]
#endif
		public static string ToCommaSeparatedString<T>(this NativeList<T> list) where T : unmanaged
		{
			return list.Length == 0 ? string.Empty : string.Join(", ", list);
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static bool IsNullOrEmpty<T>(this NativeList<T> list) where T : unmanaged
		{
			return !list.IsCreated || list.Length == 0;
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static bool IsNullOrEmpty<T>(this NativeList<T>? list) where T : unmanaged
		{
			return !list.HasValue || !list.Value.IsCreated || list.Value.Length == 0;
		}

#if TOOLBOX_BURST
		[BurstCompile]
#endif
		public static ref T GetRef<T>(this NativeList<T> array, int index) where T : unmanaged
		{
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			// Important! Validate this index first, or else the application will hard crash!
			if (index < 0 || index >= array.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}
#endif
			unsafe
			{
				return ref UnsafeUtility.ArrayElementAsRef<T>(array.GetUnsafePtr(), index);
			}
		}
#endif
	}
}