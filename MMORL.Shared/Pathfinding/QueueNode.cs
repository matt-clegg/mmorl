using Priority_Queue;

namespace MMORL.Shared.Pathfinding
{
    internal class QueueNode<T> : FastPriorityQueueNode
    {
        public T Item { get; }

        public QueueNode(T item)
        {
            Item = item;
        }
    }
}
