using System;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
    /// <summary>
    ///     Represents a scope for <see cref="Gizmos" /> that will reset the Gizmos color and matrix when disposed.
    /// </summary>
    public readonly struct GizmosScope : IDisposable
    {
        private readonly Color originalColor;
        private readonly Matrix4x4 originalMatrix;

        private GizmosScope(Color originalColor, Matrix4x4 originalMatrix)
        {
            this.originalColor = originalColor;
            this.originalMatrix = originalMatrix;
        }

        /// <summary>
        ///     Creates a new <see cref="GizmosScope" />.
        /// </summary>
        /// <returns>A new instance of <see cref="GizmosScope" />.</returns>
        public static GizmosScope Create()
        {
            return new GizmosScope(Gizmos.color, Gizmos.matrix);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Gizmos.color = originalColor;
            Gizmos.matrix = originalMatrix;
        }
    }
}