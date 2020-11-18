namespace MMORL.Shared.Pathfinding
{
    public interface IWeightedGraph<T> : IGraph<T>
    {
        float Cost(T a, T b);
    }
}
