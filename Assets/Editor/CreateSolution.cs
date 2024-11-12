#if UNITY_EDITOR

using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;
using UnityEngine;

namespace GitTools
{
	public static class Solution
	{
		public static void Sync()
		{
			ProjectGeneration projectGeneration = new ProjectGeneration();
			AssetDatabase.Refresh();
			projectGeneration.GenerateAndWriteSolutionAndProjects();
			Debug.Log("!!!!!!!!!!!!!!!!!!!!!!! SOLUTION SYNCED !!!!!!!!!!!!!!!!!!!!!!!");
		}
	}
}
#endif