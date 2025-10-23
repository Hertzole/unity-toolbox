using UnityEngine;

namespace Hertzole.UnityToolbox
{
    public static class MinMaxExtensions
    {
        public static int GetRandomValue<T>(this T minMax) where T : IMinMaxInt
        {
            return Random.Range(minMax.Min, minMax.Max);
        }
    }
}