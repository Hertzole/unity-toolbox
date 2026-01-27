#if TOOLBOX_INPUT_SYSTEM
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hertzole.UnityToolbox
{
    /// <summary>
    ///     Use on string fields to pick an input map from the targeted <see cref="PlayerInput" /> component.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    [Conditional("UNITY_EDITOR")]
    public sealed class InputMapAttribute : PropertyAttribute
    {
        public string PlayerInputPropertyName { get; }

        public InputMapAttribute(string playerInputPropertyName)
        {
            PlayerInputPropertyName = playerInputPropertyName;
        }
    }
}
#endif