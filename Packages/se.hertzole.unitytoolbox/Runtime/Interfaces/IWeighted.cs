using System;

namespace Hertzole.UnityToolbox
{
    [Obsolete("Should no longer be used. Weighted random can get the weight from items using a Func<T, int> parameter.")]
    public interface IWeighted
    {
        int RandomWeight { get; }
    }
}