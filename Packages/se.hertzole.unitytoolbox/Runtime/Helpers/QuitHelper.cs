using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hertzole.UnityToolbox
{
	/// <summary>
	///     A helper class for helping you quit the application, both at runtime and in the editor.
	/// </summary>
	public static class QuitHelper
	{
		/// <summary>
		///     Is the application currently quitting?
		/// </summary>
		/// <example>
		///	<code>
		///	void CreateInstance()
		/// {
		///		if (QuitHelper.IsQuitting)
		///		{
		///			return;
		///		}
		///
		///		// Create instance.
		/// }
		/// </code>
		/// </example>
		public static bool IsQuitting { get; private set; }

#if UNITY_EDITOR
		/// <summary>
		///     Invoked when entering play mode.
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			// Resets the static variable when entering play mode.
			IsQuitting = false;
		}
#endif

		/// <summary>
		///     Invoked on domain reload in the editor and when the game starts in the player.
		/// </summary>
#if UNITY_EDITOR
		[InitializeOnLoadMethod]
#else
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
		private static void RegisterEvents()
		{
			Application.quitting += OnQuitting;
		}

		/// <summary>
		///     Called when the application is quitting.
		/// </summary>
		private static void OnQuitting()
		{
			IsQuitting = true;
		}

		/// <summary>
		///     Quits the application at runtime and exits play mode in the editor.
		/// </summary>
		/// <example>
		///     <code>
		///  void QuitGame()
		///  {
		/// 		QuitHelper.Quit();
		///  }
		///  </code>
		/// </example>
		public static void Quit()
		{
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}

		/// <summary>
		///     Quits the application at runtime with an exit code and exits play mode in the editor.
		/// </summary>
		/// <param name="exitCode">The exit code.</param>
		/// <example>
		///     <code>
		///  void QuitGameWithErrorCode()
		///  {
		/// 		QuitHelper.Quit(1);
		///  }
		///  </code>
		/// </example>
		public static void Quit(int exitCode)
		{
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
			Application.Quit(exitCode);
#endif
		}
	}
}