using MMORL.Shared.World;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace MMORL.Shared.Loaders
{
    public static class MapLoader
    {
        public static Map LoadFromFile(string path, int chunkSize)
        {
            Map map = null;

            using (FileStream stream = new FileStream(path, FileMode.Open))
            using(GZipStream gZip = new GZipStream(stream, CompressionMode.Decompress))
            using (BinaryReader reader = new BinaryReader(gZip, Encoding.UTF8))
            {
                List<Tile> tiles = TileLoader.Load(reader);

                foreach (Tile tile in tiles)
                {
                    Tile.Register(tile);
                }

                List<Chunk> chunks = ChunkLoader.Load(reader);

                map = new Map(chunks, chunkSize);
            }

            return map;
        }
    }
}

