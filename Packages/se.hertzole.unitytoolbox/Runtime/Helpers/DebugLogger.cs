using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Hertzole.UnityToolbox
{
	/// <summary>
	///     A helper class for only logging in the editor and development builds.
	/// </summary>
	public static class DebugLogger
	{
		/// <summary>
		///     Logs a message to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		[Conditional("DEBUG")]
		public static void Log(object message)
		{
#if DEBUG
			Debug.Log(message);
#endif
		}

		/// <summary>
		///     Logs a message to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		/// <param name="context">Object to which the message applies.</param>
		[Conditional("DEBUG")]
		public static void Log(object message, Object context)
		{
#if DEBUG
			Debug.Log(message, context);
#endif
		}

		/// <summary>
		///     Logs a formatted message to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEBUG")]
		public static void LogFormat(string format, params object[] args)
		{
#if DEBUG
			Debug.LogFormat(format, args);
#endif
		}

		/// <summary>
		///     Logs a formatted message to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="context">Object to which the message applies.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEBUG")]
		public static void LogFormat(Object context, string format, params object[] args)
		{
#if DEBUG
			Debug.LogFormat(context, format, args);
#endif
		}

		/// <summary>
		///     A variant of Debug.Log that logs a warning message to the console if the application is in the editor or
		///     development build.
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		[Conditional("DEBUG")]
		public static void LogWarning(object message)
		{
#if DEBUG
			Debug.LogWarning(message);
#endif
		}

		/// <summary>
		///     A variant of Debug.Log that logs a warning message to the console if the application is in the editor or
		///     development build.
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		/// <param name="context">Object to which the message applies.</param>
		[Conditional("DEBUG")]
		public static void LogWarning(object message, Object context)
		{
#if DEBUG
			Debug.LogWarning(message, context);
#endif
		}

		/// <summary>
		///     Logs a formatted warning message to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEBUG")]
		public static void LogWarningFormat(string format, params object[] args)
		{
#if DEBUG
			Debug.LogWarningFormat(format, args);
#endif
		}

		/// <summary>
		///     Logs a formatted warning message to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="context">Object to which the message applies.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEBUG")]
		public static void LogWarningFormat(Object context, string format, params object[] args)
		{
#if DEBUG
			Debug.LogWarningFormat(context, format, args);
#endif
		}

		/// <summary>
		///     Logs an error message to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		[Conditional("DEBUG")]
		public static void LogError(object message)
		{
#if DEBUG
			Debug.LogError(message);
#endif
		}

		/// <summary>
		///     Logs an error message to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		/// <param name="context">Object to which the message applies.</param>
		[Conditional("DEBUG")]
		public static void LogError(object message, Object context)
		{
#if DEBUG
			Debug.LogError(message, context);
#endif
		}

		/// <summary>
		///     Logs a formatted error message to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEBUG")]
		public static void LogErrorFormat(string format, params object[] args)
		{
#if DEBUG
			Debug.LogErrorFormat(format, args);
#endif
		}

		/// <summary>
		///     Logs a formatted error message to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="context">Object to which the message applies.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEBUG")]
		public static void LogErrorFormat(Object context, string format, params object[] args)
		{
#if DEBUG
			Debug.LogErrorFormat(context, format, args);
#endif
		}

		/// <summary>
		///     Logs an exception to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="exception">The exception to log.</param>
		[Conditional("DEBUG")]
		public static void LogException(Exception exception)
		{
#if DEBUG
			Debug.LogException(exception);
#endif
		}

		/// <summary>
		///     Logs an exception to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="exception">The exception to log.</param>
		/// <param name="context">Object to which the message applies.</param>
		[Conditional("DEBUG")]
		public static void LogException(Exception exception, Object context)
		{
#if DEBUG
			Debug.LogException(exception, context);
#endif
		}

		/// <summary>
		///     Logs a assertion message to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		[Conditional("DEBUG")]
		public static void LogAssertion(object message)
		{
#if DEBUG
			Debug.LogAssertion(message);
#endif
		}

		/// <summary>
		///     Logs a assertion message to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		/// <param name="context">Object to which the message applies.</param>
		[Conditional("DEBUG")]
		public static void LogAssertion(object message, Object context)
		{
#if DEBUG
			Debug.LogAssertion(message, context);
#endif
		}

		/// <summary>
		///     Logs a formatted assertion message to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEBUG")]
		public static void LogAssertionFormat(string format, params object[] args)
		{
#if DEBUG
			Debug.LogAssertionFormat(format, args);
#endif
		}

		/// <summary>
		///     Logs a formatted assertion message to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="context">Object to which the message applies.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEBUG")]
		public static void LogAssertionFormat(Object context, string format, params object[] args)
		{
#if DEBUG
			Debug.LogAssertionFormat(context, format, args);
#endif
		}

		/// <summary>
		///     Logs a success message to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		[Conditional("DEBUG")]
		public static void LogSuccess(object message)
		{
#if DEBUG
			LoggerHelper.LogSuccess(message);
#endif
		}

		/// <summary>
		///     Logs a success message to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="message">String or object to be converted to string representation for display.</param>
		/// <param name="context">Object to which the message applies.</param>
		[Conditional("DEBUG")]
		public static void LogSuccess(object message, Object context)
		{
#if DEBUG
			LoggerHelper.LogSuccess(message, context);
#endif
		}

		/// <summary>
		///     Logs a formatted success message to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEBUG")]
		public static void LogSuccessFormat(string format, params object[] args)
		{
#if DEBUG
			LoggerHelper.LogSuccessFormat(format, null, args);
#endif
		}

		/// <summary>
		///     Logs a formatted success message to the Unity console if the application is in the editor or development build.
		/// </summary>
		/// <param name="context">Object to which the message applies.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">Format arguments.</param>
		[Conditional("DEBUG")]
		public static void LogSuccessFormat(Object context, string format, params object[] args)
		{
#if DEBUG
			LoggerHelper.LogSuccessFormat(format, context, args);
#endif
		}
	}
}