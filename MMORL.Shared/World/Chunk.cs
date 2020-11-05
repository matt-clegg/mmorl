using MMORL.Shared.Util;

namespace MMORL.Shared.World
{
    public class Chunk
    {
        public short X { get; set; }
        public short Y { get; set; }

        public byte Size { get; set; }

        private readonly ushort[,] _tiles;
        private readonly GameColor[,] _colors;

        public Chunk(short x, short y, byte size, ushort[,] tiles, GameColor[,] colors)
        {
            X = x;
            Y = y;
            Size = size;
            _tiles = tiles;
            _colors = colors;
        }

        public bool InBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Size && y < Size;
        }

        public Tile GetTile(int x, int y)
        {
            return Tile.GetTile(_tiles[x, y]);
        }

        public bool IsSolid(int x, int y)
        {
            return GetTile(x, y).IsSolid;
        }

        public bool IsTransparent(int x, int y)
        {
            return GetTile(x, y).IsTransparent;
        }

        public GameColor GetColor(int x, int y)
        {
            return _colors[x, y];
        }
    }
}
