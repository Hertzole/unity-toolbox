using System;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	[Obsolete("Use GizmosScope instead. This will be removed in a future version.")]
	public readonly struct GizmoColorScope : IDisposable
	{
		private readonly Color originalColor;
		
		public GizmoColorScope(Color color)
		{
			originalColor = Gizmos.color;
			Gizmos.color = color;
		}

		public void Dispose()
		{
			Gizmos.color = originalColor;
		}
	}
}