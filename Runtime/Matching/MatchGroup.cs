#nullable enable

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
    /// <summary>
    ///     A group of matchers that all need to match for the group to be considered matching. The matchers need to implement
    ///     <see cref="IMatcher" />.
    /// </summary>
    [Serializable]
    public sealed class MatchGroup : IDisposable
    {
        [SerializeReference]
        private List<IMatcher> matchers = new List<IMatcher>();

        [NonSerialized]
        private bool isMatching = false;

        private readonly Action onValueChanged;

        /// <summary>
        ///     Returns <see langword="true" /> if all matchers in the group are matching; otherwise <see langword="false" />.
        /// </summary>
        public bool IsMatching
        {
            get { return isMatching; }
            private set
            {
                if (isMatching != value)
                {
                    isMatching = value;
                    OnIsMatchingChanged?.Invoke(value);
                }
            }
        }

        /// <summary>
        ///     Indicates whether the match group has been initialized.
        /// </summary>
        [field: NonSerialized]
        public bool IsInitialized { get; private set; }

        /// <summary>
        ///     Invoked when <see cref="IsMatching" /> changes.
        /// </summary>
        public event Action<bool>? OnIsMatchingChanged;

        /// <summary>
        ///     Creates a new empty match group.
        /// </summary>
        public MatchGroup()
        {
            onValueChanged = UpdateIsMatching;
        }

        /// <summary>
        ///     Creates a new match group with the specified initial matchers.
        /// </summary>
        /// <param name="initialMatchers">The initial matchers to add to the group.</param>
        public MatchGroup(IEnumerable<IMatcher> initialMatchers) : this()
        {
            matchers.AddRange(initialMatchers);
        }

        /// <summary>
        ///     Initializes all the matchers. If they are already initialized, this does nothing.
        /// </summary>
        public void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            for (int i = 0; i < matchers.Count; i++)
            {
                matchers[i].Initialize();
                matchers[i].OnMatcherUpdated += onValueChanged;
            }

            IsInitialized = true;

            // Will update the matching state.
            UpdateIsMatching();
        }

        private void UpdateIsMatching()
        {
            IsMatching = AllMatches();
        }

        private bool AllMatches()
        {
            for (int i = 0; i < matchers.Count; i++)
            {
                if (!matchers[i].Matches())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     Adds a new matcher to the group.
        ///     <para>
        ///         If <see cref="IsInitialized" /> is <see langword="true" />, <see cref="IsMatching" /> is reevaluated and may
        ///         change.
        ///     </para>
        /// </summary>
        /// <param name="matcher">The new matcher to add.</param>
        public void AddMatcher(IMatcher matcher)
        {
            ThrowHelper.ThrowIfNull(matcher, nameof(matcher));

            matchers.Add(matcher);

            // Update IsMatching state if initialized.
            if (IsInitialized)
            {
                UpdateIsMatching();
            }
        }

        /// <summary>
        ///     Removes a matcher from the group.
        ///     <para>
        ///         If <see cref="IsInitialized" /> is <see langword="true" />, and the matcher was removed,
        ///         <see cref="IsMatching" /> is reevaluted and may change.
        ///     </para>
        /// </summary>
        /// <param name="matcher">The matcher to remove.</param>
        /// <returns><see langword="true" /> if the matcher was removed; otherwise <see langword="false" />.</returns>
        public bool RemoveMatcher(IMatcher matcher)
        {
            bool removed = matchers.Remove(matcher);
            // Update IsMatching state if initialized.
            if (removed && IsInitialized)
            {
                UpdateIsMatching();
            }

            return removed;
        }

        /// <summary>
        ///     Clears all the matchers from the group.
        ///     <para>
        ///         If <see cref="IsInitialized" /> is <see langword="true" />, <see cref="IsMatching" /> is reevaluated and may
        ///         change.
        ///     </para>
        /// </summary>
        public void ClearMatchers()
        {
            matchers.Clear();

            // Update IsMatching state if initialized.
            if (IsInitialized)
            {
                UpdateIsMatching();
            }
        }

        /// <summary>
        ///     Disposes all the matchers. If they are not initialized, this does nothing.
        /// </summary>
        public void Dispose()
        {
            if (!IsInitialized)
            {
                return;
            }

            for (int i = 0; i < matchers.Count; i++)
            {
                matchers[i].Dispose();
                matchers[i].OnMatcherUpdated -= onValueChanged;
            }

            OnIsMatchingChanged = null;

            isMatching = false;
            IsInitialized = false;
        }
    }
}