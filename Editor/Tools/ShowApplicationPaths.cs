using UnityEditor;
using UnityEngine;

namespace Hertzole.UnityToolbox.Editor
{
	public static class ShowApplicationPaths
	{
		[MenuItem("Edit/Show Application Persistent Data Path")]
		public static void ShowPersistentDataPath()
		{
			ShowPath(Application.persistentDataPath);
		}

		[MenuItem("Edit/Show Application Data Path")]
		public static void ShowDataPath()
		{
			ShowPath(Application.dataPath);
		}

		[MenuItem("Edit/Show Application Streaming Assets Path")]
		public static void ShowStreamingAssetsPath()
		{
			ShowPath(Application.streamingAssetsPath);
		}

		[MenuItem("Edit/Show Application Temporary Cache Path")]
		public static void ShowTemporaryCachePath()
		{
			ShowPath(Application.temporaryCachePath);
		}

		private static void ShowPath(string path)
		{
			EditorUtility.RevealInFinder(path);
		}
	}
}