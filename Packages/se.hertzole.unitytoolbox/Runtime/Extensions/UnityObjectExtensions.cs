using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
    /// <summary>
    ///     Extensions methods related to Unity objects.
    /// </summary>
    public static class UnityObjectExtensions
    {
        /// <summary>
        ///     Tries to get a component first and if that fails, it will add the component instead.
        /// </summary>
        /// <param name="target">The object to check the component on.</param>
        /// <typeparam name="T">The component to get/add.</typeparam>
        /// <returns>The existing component or the newly added one.</returns>
        /// <exception cref="ArgumentNullException">If the target is null.</exception>
        public static T GetOrAddComponent<T>(this Component target) where T : Component
        {
            ThrowHelper.ThrowIfNull(target, nameof(target));

            if (target.TryGetComponent(out T component))
            {
                return component;
            }

            return target.gameObject.AddComponent<T>();
        }

        /// <summary>
        ///     Tries to get a component first and if that fails, it will add the component instead.
        /// </summary>
        /// <param name="target">The object to check the component on.</param>
        /// <typeparam name="T">The component to get/add.</typeparam>
        /// <returns>The existing component or the newly added one.</returns>
        /// <exception cref="ArgumentNullException">If the target is null.</exception>
        public static T GetOrAddComponent<T>(this GameObject target) where T : Component
        {
            ThrowHelper.ThrowIfNull(target, nameof(target));

            if (target.TryGetComponent(out T component))
            {
                return component;
            }

            return target.gameObject.AddComponent<T>();
        }

        public static bool TryGetComponentInParent<T>(this GameObject target, out T component)
        {
            ThrowHelper.ThrowIfNull(target, nameof(target));

            component = target.GetComponentInParent<T>();
            return component != null;
        }

        public static bool TryGetComponentInParent<T>(this Component target, out T component)
        {
            ThrowHelper.ThrowIfNull(target, nameof(target));

            component = target.GetComponentInParent<T>();
            return component != null;
        }

        public static bool TryGetComponentInChildren<T>(this GameObject target, out T component)
        {
            ThrowHelper.ThrowIfNull(target, nameof(target));

            component = target.GetComponentInChildren<T>();
            return component != null;
        }

        public static bool TryGetComponentInChildren<T>(this Component target, out T component)
        {
            ThrowHelper.ThrowIfNull(target, nameof(target));

            component = target.GetComponentInChildren<T>();
            return component != null;
        }

        /// <summary>
        ///     Sets the layer of the object and all of its children.
        /// </summary>
        /// <param name="obj">The parent to set the layer on.</param>
        /// <param name="newLayer">The layer to apply.</param>
        /// <exception cref="ArgumentNullException">If the target is null.</exception>
        public static void SetLayerRecursively(this GameObject obj, int newLayer)
        {
            ThrowHelper.ThrowIfNull(obj, nameof(obj));

            obj.layer = newLayer;

            int childCount = obj.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                obj.transform.GetChild(i).gameObject.SetLayerRecursively(newLayer);
            }
        }

        /// <summary>
        ///     Sets the tag of the object and all of its children.
        /// </summary>
        /// <param name="obj">The parent to set the layer on.</param>
        /// <param name="newTag">The tag to apply.</param>
        /// <exception cref="ArgumentNullException">If the target is null.</exception>
        public static void SetTagRecursively(this GameObject obj, string newTag)
        {
            ThrowHelper.ThrowIfNull(obj, nameof(obj));

            obj.tag = newTag;

            int childCount = obj.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                obj.transform.GetChild(i).gameObject.SetTagRecursively(newTag);
            }
        }

        /// <summary>
        ///     Sets the name of a game object only in the editor.
        /// </summary>
        /// <param name="target">The target to set the name on.</param>
        /// <param name="name">The new name.</param>
        /// <exception cref="ArgumentNullException">If the target is null.</exception>
        [Conditional("UNITY_EDITOR")]
        public static void SetEditorNameOnly(this GameObject target, string name)
        {
#if UNITY_EDITOR
            ThrowHelper.ThrowIfNull(target, nameof(target));

            target.name = name;
#endif
        }

        /// <summary>
        ///     Sets the name of a game object only in the editor and development builds.
        /// </summary>
        /// <param name="target">The target to set the name on.</param>
        /// <param name="name">The new name.</param>
        [Conditional("DEBUG")]
        public static void SetDebugNameOnly(this GameObject target, string name)
        {
#if DEBUG
            ThrowHelper.ThrowIfNull(target, nameof(target));

            target.name = name;
#endif
        }

        /// <summary>
        ///     Checks if the target <see cref="GameObject" /> is on the specified layer.
        /// </summary>
        /// <param name="target">The target game object.</param>
        /// <param name="layer">The layer to check for.</param>
        /// <returns><see langword="true" /> if the target is on the specified layer; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="target" /> is <see langword="null" />.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasLayer(this GameObject target, int layer)
        {
            ThrowHelper.ThrowIfNull(target, nameof(target));

            return target.layer == layer;
        }

        /// <summary>
        ///     Checks if the target <see cref="GameObject" /> is on the specified layer.
        /// </summary>
        /// <param name="target">The target game object.</param>
        /// <param name="layer">The layer to check for.</param>
        /// <returns><see langword="true" /> if the target is on the specified layer; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="target" /> is <see langword="null" />.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasLayer(this GameObject target, Layer layer)
        {
            ThrowHelper.ThrowIfNull(target, nameof(target));

            return target.layer == (int) layer;
        }

        /// <summary>
        ///     Checks if the target <see cref="GameObject" /> is on one of the layers in the specified <see cref="LayerMask" />.
        /// </summary>
        /// <param name="target">The target game object.</param>
        /// <param name="layerMask">The layer mask to check for.</param>
        /// <returns>
        ///     <see langword="true" /> if the target is on one of the layers in the specified layer mask; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="target" /> is <see langword="null" />.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasLayer(this GameObject target, LayerMask layerMask)
        {
            ThrowHelper.ThrowIfNull(target, nameof(target));

            return (layerMask.value & (1 << target.layer)) != 0;
        }
    }
}