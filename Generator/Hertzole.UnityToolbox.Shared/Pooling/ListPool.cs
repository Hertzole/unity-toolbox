using System.Collections.Generic;

namespace Hertzole.UnityToolbox.Shared;

internal static class ListPool<T>
{
	private static readonly ObjectPool<List<T>> pool = new ObjectPool<List<T>>(() => new List<T>(), null, x => x.Clear());

	public static List<T> Get()
	{
		return pool.Get();
	}

	public static PoolHandle<List<T>> Get(out List<T> item)
	{
		return pool.Get(out item);
	}

	public static void Return(List<T> item)
	{
		pool.Return(item);
	}
}