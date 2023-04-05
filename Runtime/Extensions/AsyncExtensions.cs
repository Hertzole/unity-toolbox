#if TOOLBOX_UNITASK
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	public static class AsyncExtensions
	{
		public static async UniTask WaitUntilNotNull(this Object obj, CancellationToken cancellationToken = default)
		{
			while (obj == null && !cancellationToken.IsCancellationRequested)
			{
				await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
			}
		}
	}
}
#endif