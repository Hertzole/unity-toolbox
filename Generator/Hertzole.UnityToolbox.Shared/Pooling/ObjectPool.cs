using System;
using System.Collections.Concurrent;

namespace Hertzole.UnityToolbox.Shared;

internal sealed class ObjectPool<T>
{
	private readonly ConcurrentStack<T> pool = new ConcurrentStack<T>();
	private readonly Func<T> createFunc;
	private readonly Action<T>? onGet;
	private readonly Action<T>? onReturn;

	public ObjectPool(Func<T> createFunc, Action<T>? onGet, Action<T>? onReturn)
	{
		this.createFunc = createFunc;
		this.onGet = onGet;
		this.onReturn = onReturn;
	}

	public T Get()
	{
		if (!pool.TryPop(out T? item))
		{
			item = createFunc();
		}

		onGet?.Invoke(item);
		return item;
	}

	public PoolHandle<T> Get(out T item)
	{
		return PoolHandle<T>.Create(this, out item);
	}

	public void Return(T item)
	{
		onReturn?.Invoke(item);
		pool.Push(item);
	}
}

internal readonly struct PoolHandle<T> : IDisposable
{
	private readonly T value;
	private readonly ObjectPool<T> pool;

	private PoolHandle(T value, ObjectPool<T> pool)
	{
		this.value = value;
		this.pool = pool;
	}

	public static PoolHandle<T> Create(ObjectPool<T> pool, out T item)
	{
		item = pool.Get();
		PoolHandle<T> result = new PoolHandle<T>(item, pool);
		return result;
	}

	public void Dispose()
	{
		pool.Return(value);
	}
}