using System;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
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