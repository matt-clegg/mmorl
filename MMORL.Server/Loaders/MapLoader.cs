using MMORL.Server.World;
using MMORL.Shared.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;

namespace MMORL.Server.Loaders
{
    public static class MapLoader
    {
        private const string Magic = "MMORL";
        private const int Version = 2;

        public static Map LoadFromFile(string location, int chunkSize)
        {
            Map map = null;

            string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(currentPath, location);

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
                List<MobSpawnDefinition> spawns = SpawnLoader.Load(reader, 16, 24); // TODO: store tile width and height somewhere sane
                List<Chunk> chunks = ChunkLoader.Load(reader);

                map = new Map(chunks, warps, spawns, chunkSize);
            }

            return map;
        }
    }
}

