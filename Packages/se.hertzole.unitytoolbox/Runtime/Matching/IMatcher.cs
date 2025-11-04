#nullable enable

using System;

namespace Hertzole.UnityToolbox
{
    /// <summary>
    ///     A matcher that can be used with <see cref="MatchGroup" />.
    /// </summary>
    public interface IMatcher : IDisposable
    {
        /// <summary>
        ///     Should be invoked on any value changes that could affect the match result.
        /// </summary>
        event Action? OnMatcherUpdated;

        /// <summary>
        ///     Called when the match group initializes.
        /// </summary>
        void Initialize();

        /// <summary>
        ///     Checks if the matcher matches.
        /// </summary>
        /// <returns><see langword="true" /> if this matcher matches the desired output; otherwise <see langword="false" />.</returns>
        bool Matches();
    }
}