using System;
using UnityEditor;

namespace Hertzole.UnityToolbox.Editor
{
	public readonly struct LabelWidthScope : IDisposable
	{
		private readonly float originalWidth;

		public LabelWidthScope(float width)
		{
			originalWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = width;
		}

		public void Dispose()
		{
			EditorGUIUtility.labelWidth = originalWidth;
		}
	}
}