using MMORL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Toolbox;

namespace MMORL.Shared.World
{
    public class Map
    {
        private readonly Dictionary<Point2D, Chunk> _chunks = new Dictionary<Point2D, Chunk>();
        public IReadOnlyCollection<Chunk> Chunks => _chunks.Values;

        private readonly List<Warp> _warps;
        private readonly List<MobSpawnDefinition> _spawns;
        public IReadOnlyCollection<MobSpawnDefinition> Spawns => _spawns;

        private readonly List<Entity> _entities = new List<Entity>();
        public IReadOnlyCollection<Entity> Entities => _entities;

        public int ChunkSize { get; }

        public Map(int chunkSize)
        {
            ChunkSize = chunkSize;
        }

        public Map(List<Chunk> chunks, List<Warp> warps, List<MobSpawnDefinition> spawns, int chunkSize)
        {
            _chunks = chunks.ToDictionary(c => new Point2D(c.X, c.Y));
            _warps = warps;
            _spawns = spawns;
            ChunkSize = chunkSize;
        }

        public void Add(Entity entity, int x, int y)
        {
            entity.Initialise(this, x, y);
            _entities.Add(entity);
        }

        public void Remove(int entityId)
        {
            _entities.RemoveAll(e => e.Id == entityId);
        }

        public void MoveEntity(int id, int x, int y)
        {
            foreach (Entity entity in Entities)
            {
                if (entity.Id == id)
                {
                    entity.Move(x, y);
                    return;
                }
            }
        }

        public Entity GetEntity(int x, int y)
        {
            foreach (Entity entity in _entities)
            {
                if (entity.X == x && entity.Y == y)
                {
                    return entity;
                }
            }

            return null;
        }

        public T GetEntityAs<T>(int id) where T : Entity
        {
            foreach(Entity entity in _entities)
            {
                if(entity.Id == id)
                {
                    return entity as T;
                }
            }

            return default;
        }

        public Tile GetTile(int x, int y)
        {
            Point2D chunkPos = ToChunkCoords(x, y);
            Chunk chunk = GetChunk(chunkPos.X, chunkPos.Y);

            Point2D localChunk = ToLocalChunkCoords(x, y);

            return chunk?.GetTile(localChunk.X, localChunk.Y);
        }

        public Warp GetWarp(int id)
        {
            foreach (Warp warp in _warps)
            {
                if (warp.Id == id)
                {
                    return warp;
                }
            }

            return null;
        }

        public Warp GetWarp(int x, int y)
        {
            foreach (Warp warp in _warps)
            {
                if (warp.X == x && warp.Y == y)
                {
                    return warp;
                }
            }

            return null;
        }

        public void LoadChunk(Chunk chunk)
        {
            Point2D position = new Point2D(chunk.X, chunk.Y);
            if (!_chunks.ContainsKey(position))
            {
                _chunks.Add(position, chunk);
            }
            else
            {
                _chunks[position] = chunk;
            }
        }

        public void UnloadChunk(int x, int y)
        {
            Point2D position = new Point2D(x, y);
            if (_chunks.ContainsKey(position))
            {
                _chunks.Remove(position);
            }
        }

        public void ClearChunks()
        {
            _chunks.Clear();
        }

        public Chunk GetChunk(int x, int y)
        {
            return _chunks.TryGetValue(new Point2D(x, y), out Chunk chunk) ? chunk : null;
        }

        public Point2D ToChunkCoords(int x, int y)
        {
            int chunkX;
            int chunkY;

            if (x < 0)
            {
                chunkX = (int)Math.Floor((decimal)x / ChunkSize);
            }
            else
            {
                chunkX = x / ChunkSize;
            }

            if (y < 0)
            {
                chunkY = (int)Math.Floor((decimal)y / ChunkSize);
            }
            else
            {
                chunkY = y / ChunkSize;
            }

            return new Point2D(chunkX, chunkY);
        }

        public Point2D ToLocalChunkCoords(int x, int y)
        {
            int localX = Math.Abs(x % ChunkSize);
            int localY = Math.Abs(y % ChunkSize);

            if (x < 0 && localX != 0)
            {
                localX = ChunkSize - localX;
            }

            if (y < 0 && localY != 0)
            {
                localY = ChunkSize - localY;
            }

            return new Point2D(localX, localY);
        }
    }
}
