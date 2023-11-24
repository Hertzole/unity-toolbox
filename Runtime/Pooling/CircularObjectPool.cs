#if UNITY_2021_2_OR_NEWER // Only available in 2021.2 or newer due to ObjectPool.
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

namespace Hertzole.UnityToolbox
{
	/// <summary>
	///     Pool that automatically pools the first active object when the max count has been reached.
	/// </summary>
	/// <typeparam name="T">The type in the pool.</typeparam>
	public sealed class CircularObjectPool<T> : IDisposable, IObjectPool<T> where T : class
	{
		private readonly int maxCount;
		private readonly List<T> active;
		private readonly ObjectPool<T> pool;

		/// <summary>
		///     Number of objects that have been created by the pool but are currently in use and have not yet been returned.
		/// </summary>
		public int CountActive
		{
			get { return active.Count; }
		}

		/// <summary>
		///     The total number of active and inactive objects.
		/// </summary>
		public int CountAll
		{
			get { return pool.CountAll; }
		}

		/// <summary>
		///     Number of objects that are currently available in the pool.
		/// </summary>
		public int CountInactive
		{
			get { return pool.CountInactive; }
		}

		public CircularObjectPool(int maxCount,
			Func<T> createFunc,
			Action<T> actionOnGet = null,
			Action<T> actionOnRelease = null,
			Action<T> actionOnDestroy = null)
		{
			this.maxCount = maxCount;
			active = new List<T>(maxCount);
			pool = new ObjectPool<T>(createFunc, actionOnGet, actionOnRelease, actionOnDestroy);
		}

		/// <summary>
		///     Releases all objects created by this pool back to the pool.
		/// </summary>
		public void ReleaseAll()
		{
			while (active.Count > 0)
			{
				int index = active.Count - 1;
				pool.Release(active[index]);
				active.RemoveAt(index);
			}

			Assert.AreEqual(0, active.Count, "All objects should have been released.");
		}

		/// <summary>
		///     Removes all pooled items. If the pool contains a destroy callback then it will be called for each item that is in
		///     the pool.
		/// </summary>
		public void Dispose()
		{
			Clear();
		}

		/// <summary>
		///     Get an instance from the pool. If the pool is empty then a new instance will be created. If the max count is
		///     reached then the first object will be reused.
		/// </summary>
		/// <returns>A pooled object of a new instance if the pool is empty.</returns>
		public T Get()
		{
			if (active.Count >= maxCount)
			{
				T obj = active[0];
				active.RemoveAt(0);
				pool.Release(obj);
			}

			T item = pool.Get();

			active.Add(item);
			return item;
		}

		public PooledObject<T> Get(out T v)
		{
			if (active.Count >= maxCount)
			{
				T obj = active[0];
				active.RemoveAt(0);
				pool.Release(obj);
			}

			PooledObject<T> item = pool.Get(out v);

			active.Add(v);
			return item;
		}

		/// <summary>
		///     Returns the instance back to the pool. You can only return objects that were created from this pool.
		/// </summary>
		/// <param name="obj">The instance to return to the pool.</param>
		public void Release(T obj)
		{
			if (active.Contains(obj))
			{
				active.Remove(obj);
				pool.Release(obj);
			}
			else
			{
				Debug.LogWarning("Trying to release an object that is not from this pool.");
			}
		}

		/// <summary>
		///     Removes all pooled items. If the pool contains a destroy callback then it will be called for each item that is in
		///     the pool.
		/// </summary>
		public void Clear()
		{
			for (int i = 0; i < active.Count; i++)
			{
				pool.Release(active[i]);
			}

			pool.Clear();
			active.Clear();
		}
	}
}
#endif