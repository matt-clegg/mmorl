using System.Collections.Generic;

namespace MMORL.Shared.Pathfinding
{
    public interface IGraph<T>
    {
        int Area { get; }
        IEnumerable<T> GetNeighbors(T id);
    }
}
