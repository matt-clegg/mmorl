using MMORL.Shared.Pathfinding;
using MMORL.Shared.World;
using System;
using System.Collections.Generic;
using Toolbox;

namespace MMORL.Client.Pathfinding
{
    public class ChunkPathWorld : IWeightedGraph<Point2D>
    {
        private readonly Map _map;

        public int Area => _map.Chunks.Count * _map.ChunkSize * _map.ChunkSize;

        public ChunkPathWorld(Map map)
        {
            _map = map;
        }

        public float Cost(Point2D a, Point2D b)
        {
            return Math.Abs(a.X - b.X) == 1 && Math.Abs(a.Y - b.Y) == 1 ? Heuristics.TwoSqrt : 1f;
        }

        public IEnumerable<Point2D> GetNeighbors(Point2D origin)
        {
            foreach (Point2D neighbour in Direction.GetNeighbours(origin, Direction.AllDirections))
            {
                Tile tile = _map.GetTile(neighbour.X, neighbour.Y);
                if (tile != null && !tile.IsSolid)
                {
                    yield return neighbour;
                }
            }
        }
    }
}
