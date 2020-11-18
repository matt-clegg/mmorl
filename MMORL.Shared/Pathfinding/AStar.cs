using Priority_Queue;
using System;
using System.Collections.Generic;

namespace MMORL.Shared.Pathfinding
{
    public static class AStar<T> where T : IEquatable<T>
    {
        public delegate float Heuristic(T a, T b);

        private static QueueNode<T> NewNode(T item) => new QueueNode<T>(item);

        public static List<T> FindPath(IWeightedGraph<T> graph, T start, T goal, Heuristic heuristic, bool includeStart = false, bool includeGoal = false)
        {
            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }
            if (start == null)
            {
                throw new ArgumentNullException(nameof(start));
            }
            if (goal == null)
            {
                throw new ArgumentNullException(nameof(goal));
            }
            if (heuristic == null)
            {
                throw new ArgumentNullException(nameof(heuristic));
            }

            // We are already starting at the goal node.
            if (start.Equals(goal))
            {
                return null;
            }

            if (graph.Area <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(graph.Area), "The graph area must be greater than 0.");
            }

            var frontier = new FastPriorityQueue<QueueNode<T>>(graph.Area);
            var cameFrom = new Dictionary<T, T>();
            var costSoFar = new Dictionary<T, float>();

            frontier.Enqueue(NewNode(start), 0);

            cameFrom[start] = start;
            costSoFar[start] = 0;

            float p = 14.0f / (float)(Math.Sqrt(graph.Area) * 2);

            while (frontier.Count > 0)
            {
                T current = frontier.Dequeue().Item;

                if (current.Equals(goal))
                {
                    List<T> path = RetracePath(cameFrom, start, goal, includeStart, includeGoal);
                    return path;
                }

                foreach (T neighbor in graph.GetNeighbors(current))
                {
                    float newCost = costSoFar[current] + graph.Cost(current, neighbor);

                    if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                    {
                        costSoFar[neighbor] = newCost;

                        float h = heuristic(neighbor, goal);
                        h *= 1.0f + p;
                        frontier.Enqueue(NewNode(neighbor), newCost + h);
                        cameFrom[neighbor] = current;
                    }
                }
            }

            return null;
        }

        private static List<T> RetracePath(Dictionary<T, T> cameFrom, T start, T goal, bool includeStart, bool includeGoal)
        {
            var path = new List<T>();
            T current = goal;

            while (!current.Equals(start))
            {
                path.Add(current);
                current = cameFrom[current];
            }

            if (includeStart)
            {
                path.Add(start);
            }

            if (!includeGoal)
            {
                path.RemoveAt(0);
            }

            path.Reverse();
            return path;
        }
    }
}
