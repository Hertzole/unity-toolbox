using UnityEngine;

namespace Hertzole.UnityToolbox
{
	internal static class LoggerHelper
	{
		public static void LogSuccess(object message, Object context = null)
		{
			if (context == null)
			{
				Debug.Log($"<color=green>{message}</color>");
			}
			else
			{
				Debug.Log($"<color=green>{message}</color>", context);
			}
		}
		
		public static void LogSuccessFormat(object message, Object context = null, params object[] args)
		{
			if (context == null)
			{
				Debug.LogFormat($"<color=green>{message}</color>", args);
			}
			else
			{
				Debug.LogFormat(context, $"<color=green>{message}</color>", args);
			}
		}
	}
}