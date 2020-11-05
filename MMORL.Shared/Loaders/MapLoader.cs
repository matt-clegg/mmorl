using MMORL.Shared.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace MMORL.Shared.Loaders
{
    public static class MapLoader
    {
        private const string Magic = "MMORL";
        private const int Version = 1;

        public static Map LoadFromFile(string path, int chunkSize)
        {
            Map map = null;

            using (FileStream stream = new FileStream(path, FileMode.Open))
            using (GZipStream gZip = new GZipStream(stream, CompressionMode.Decompress))
            using (BinaryReader reader = new BinaryReader(gZip, Encoding.UTF8))
            {
                string magic = reader.ReadString();
                if (!magic.Equals(Magic))
                {
                    throw new InvalidOperationException("Unknown map format.");
                }

                int version = reader.ReadInt32();
                if (version != Version)
                {
                    throw new InvalidOperationException("Invalid map version.");
                }

                List<Tile> tiles = TileLoader.Load(reader);

                foreach (Tile tile in tiles)
                {
                    Tile.Register(tile);
                }

                List<Warp> warps = WarpLoader.Load(reader);
                List<Chunk> chunks = ChunkLoader.Load(reader);

                map = new Map(chunks, warps, chunkSize);
            }

            return map;
        }
    }
}

