namespace Euler.DataStructures.Heap
{
    public interface IHeapCondition<in TKey>
    {
        bool IsValid(TKey first, TKey second);
    }
}