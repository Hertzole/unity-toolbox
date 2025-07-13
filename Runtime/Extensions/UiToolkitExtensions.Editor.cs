#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox
{
    public static partial class UiToolkitExtensions
    {
        public static void RegisterValueChangeCallback<TArgs>(this PropertyField control,
            EventCallback<SerializedPropertyChangeEvent, TArgs> callback,
            TArgs userArgs)
        {
            control.RegisterCallback(callback, userArgs);
        }
        
        public static void UnregisterValueChangeCallback<TArgs>(this PropertyField control,
            EventCallback<SerializedPropertyChangeEvent, TArgs> callback)
        {
            control.UnregisterCallback(callback);
        }
    }
}
#endif