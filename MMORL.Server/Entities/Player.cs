using Lidgren.Network;
using MMORL.Server.Actions;
using MMORL.Server.Net;
using MMORL.Shared.Entities;
using MMORL.Shared.Net.Messages;
using MMORL.Shared.Util;
using MMORL.Shared.World;
using System.Collections.Generic;
using Toolbox;

namespace MMORL.Server.Entities
{
    public class Player : ServerEntity
    {
        private readonly Queue<BaseAction> _actions = new Queue<BaseAction>();

        private readonly HashSet<Point2D> _loadedChunks = new HashSet<Point2D>();
        public IReadOnlyCollection<Point2D> LoadedChunks => _loadedChunks;

        public Point2D CurrentChunk { get; private set; }
        public Point2D LastChunk { get; private set; }

        public Player(int id, string name, string sprite, GameColor color, int speed, GameServer server) : base(id, name, sprite, color, speed, server)
        {
            Type = EntityType.Player;
        }

        public override void Initialise(Map map, int x, int y)
        {
            Point2D chunkPos = map.ToChunkCoords(x, y);
            for (int xp = -Game.ChunkRadiusX; xp <= Game.ChunkRadiusX; xp++)
            {
                for (int yp = -Game.ChunkRadiusY; yp <= Game.ChunkRadiusY; yp++)
                {
                    _loadedChunks.Add(chunkPos + new Point2D(xp,yp));
                }
            }

            base.Initialise(map, x, y);
        }

        protected override BaseAction OnGetAction()
        {
            BaseAction action = null;
            if (_actions.Count > 0)
            {
                action = _actions.Dequeue();
            }
            return action;
        }

        public override void Move(int x, int y)
        {
            Point2D newChunk = Map.ToChunkCoords(x, y);

            if (CurrentChunk != newChunk)
            {
                System.Console.WriteLine("player chunk changed from " + CurrentChunk + " to " + newChunk);
                OnChunkChanged();
                LastChunk = CurrentChunk;
            }
            CurrentChunk = newChunk;

            base.Move(x, y);
        }

        private void OnChunkChanged()
        {
            Point2D chunkPos = Map.ToChunkCoords(X, Y);

            HashSet<Point2D> newChunks = new HashSet<Point2D>();

            for (int x = -Game.ChunkRadiusX; x <= Game.ChunkRadiusX; x++)
            {
                for (int y = -Game.ChunkRadiusY; y <= Game.ChunkRadiusY; y++)
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
                        NetConnection connection = Server.GetConnectionForPlayer(this);
                        Server.SendMessage(chunkData, connection, NetDeliveryMethod.ReliableUnordered);
                        System.Console.WriteLine("Sending chunk " + chunkToLoad + " to player");
                    }
                }
            }

            foreach(Point2D chunkToRemove in _loadedChunks)
            {
                if (!newChunks.Contains(chunkToRemove))
                {
                    UnloadChunkMessage unloadChunk = new UnloadChunkMessage(chunkToRemove);
                    NetConnection connection = Server.GetConnectionForPlayer(this);
                    Server.SendMessage(unloadChunk, connection, NetDeliveryMethod.ReliableUnordered);
                }
            }

            _loadedChunks.Clear();
            foreach (Point2D chunk in newChunks)
            {
                _loadedChunks.Add(chunk);
            }
        }

        public void QueueAction(BaseAction action)
        {
            _actions.Enqueue(action);
        }

        public void ClearMoves()
        {
            _actions.Clear();
        }
    }
}
