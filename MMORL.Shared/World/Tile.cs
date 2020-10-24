using System;
using System.Collections.Generic;

namespace MMORL.Shared.World
{
    public class Tile
    {
        private static Dictionary<ushort, Tile> _tiles = new Dictionary<ushort, Tile>();
        public static IReadOnlyCollection<Tile> RegisteredTiles => _tiles.Values;

        public ushort Id { get; }
        public string Sprite { get; }
        public bool IsSolid { get; }
        public bool IsTransparent { get; }

        public Tile(ushort id, bool isSolid, bool isTransparent)
        {
            Id = id;
            Sprite = $"tile_{id}";
            IsSolid = isSolid;
            IsTransparent = isTransparent;
        }

        public static void Register(Tile tile)
        {
            if (tile.Id < 0 || tile.Id >= ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(tile.Id), $"Invalid tile ID: {tile.Id}");
            }

            if (_tiles.ContainsKey(tile.Id))
            {
                throw new ArgumentException(nameof(tile.Id), $"Duplicate tile ID: {tile.Id}");
            }

            _tiles.Add(tile.Id, tile);
        }

        public static bool IsRegistered(ushort id)
        {
            return _tiles.ContainsKey(id);
        }

        public static Tile GetTile(ushort id)
        {
            if (!IsRegistered(id))
            {
                throw new InvalidOperationException($"Tile not registered: {id}");
            }
            return _tiles[id];
        }
    }
}
