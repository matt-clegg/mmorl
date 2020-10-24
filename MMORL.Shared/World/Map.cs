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

        private readonly List<Entity> _entities = new List<Entity>();
        public IReadOnlyCollection<Entity> Entities => _entities.AsReadOnly();

        public int ChunkSize { get; }

        public Map(int chunkSize)
        {
            ChunkSize = chunkSize;
        }

        public Map(List<Chunk> chunks, int chunkSize)
        {
            _chunks = chunks.ToDictionary(c => new Point2D(c.X, c.Y));
            ChunkSize = chunkSize;
        }

        public void Add(Entity entity, int x, int y)
        {
            _entities.Add(entity);
            entity.X = x;
            entity.Y = y;
        }

        public Tile GetTile(int x, int y)
        {
            Point2D chunkPos = ToChunkCoords(x, y);
            Chunk chunk = GetChunk(chunkPos.X, chunkPos.Y);
            return chunk?.GetTile(Math.Abs(x % ChunkSize), Math.Abs(y % ChunkSize));
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
    }
}
