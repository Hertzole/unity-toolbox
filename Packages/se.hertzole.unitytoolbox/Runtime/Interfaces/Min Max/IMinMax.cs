namespace Hertzole.UnityToolbox
{
    public interface IMinMax<T>
    {
        T Min { get; set; }
        T Max { get; set; }
        
        void Deconstruct(out T minValue, out T maxValue);
    }
}