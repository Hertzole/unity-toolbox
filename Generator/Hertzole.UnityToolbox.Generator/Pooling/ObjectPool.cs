using System;
using System.Collections.Concurrent;

namespace Hertzole.UnityToolbox.Generator.Pooling;

public static class ObjectPool<T> where T : new()
{
	private static readonly ConcurrentStack<T> pool = new ConcurrentStack<T>();

	public static PoolHandle<T> Get(out T item)
	{
		if (pool.TryPop(out item))
		{
			return new PoolHandle<T>(item);
		}

		item = new T();
		return new PoolHandle<T>(item);
	}

	public static void Return(T item)
	{
		pool.Push(item);
	}
}

public readonly struct PoolHandle<T> : IDisposable where T : new()
{
	private readonly T value;

	public PoolHandle(T value)
	{
		this.value = value;
	}

	public void Dispose()
	{
		ObjectPool<T>.Return(value);
	}
}