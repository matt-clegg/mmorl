using Lidgren.Network;
using MMORL.Server.Actions;
using MMORL.Server.Net;
using MMORL.Shared.Entities;
using MMORL.Shared.Net;
using MMORL.Shared.Util;
using MMORL.Shared.World;
using System.Collections.Generic;
using Toolbox;

namespace MMORL.Server.Entities
{
    public class ServerEntity : Entity
    {
        public Energy Energy { get; }

        private readonly HashSet<Point2D> _loadedChunks = new HashSet<Point2D>();
        public IReadOnlyCollection<Point2D> LoadedChunks => _loadedChunks;

        private readonly Queue<BaseAction> _actions = new Queue<BaseAction>();

        private readonly GameServer _server;

        public Point2D CurrentChunk { get; private set; }
        public Point2D LastChunk { get; private set; }

        public ServerEntity(int id, string name, string sprite, GameColor color, int speed, GameServer server) : base(id, name, sprite, color, speed)
        {
            Energy = new Energy();
            _server = server;
        }

        public override void ProcessTurn()
        {
            if (Energy.CanTakeTurn || Energy.Gain(Speed))
            {
                if (_actions.Count > 0)
                {
                    _actions.Dequeue().Perform(this, _server);
                }

                Energy.Spend();
            }
        }

        public override void Move(int x, int y)
        {
            Point2D newChunk = Map.ToChunkCoords(x, y); ;

            if (CurrentChunk != newChunk)
            {
                //ChunkChangedEvent?.Invoke(this, newChunk);
                OnChunkChanged();
                LastChunk = CurrentChunk;
            }
            CurrentChunk = newChunk;

            base.Move(x, y);
        }

        public void QueueAction(BaseAction action)
        {
            _actions.Enqueue(action);
        }

        public void ClearMoves()
        {
            _actions.Clear();
        }

        private void OnChunkChanged()
        {
            Point2D chunkPos = Map.ToChunkCoords(X, Y);

            HashSet<Point2D> newChunks = new HashSet<Point2D>();

            for (int x = -2; x <= 2; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    newChunks.Add(chunkPos + new Point2D(x, y));
                }
            }

            foreach (Point2D chunkToLoad in newChunks)
            {
                if (!_loadedChunks.Contains(chunkToLoad))
                {
                    Chunk chunk = Map.GetChunk(chunkToLoad.X, chunkToLoad.Y);
                    if (chunk != null)
                    {
                        ChunkDataMessage chunkData = new ChunkDataMessage(chunk);
                        NetConnection connection = _server.GetConnectionForPlayer(this);
                        _server.SendMessage(chunkData, connection, NetDeliveryMethod.ReliableUnordered);
                    }
                }
            }

            _loadedChunks.Clear();
            foreach (Point2D chunk in newChunks)
            {
                _loadedChunks.Add(chunk);
            }
        }
    }
}
