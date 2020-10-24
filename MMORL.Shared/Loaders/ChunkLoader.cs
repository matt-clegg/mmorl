using MMORL.Shared.Util;
using MMORL.Shared.World;
using System.Collections.Generic;
using System.IO;

namespace MMORL.Shared.Loaders
{
    public static class ChunkLoader
    {
        public static List<Chunk> Load(BinaryReader reader)
        {
            ushort chunkCount = reader.ReadUInt16();

            List<Chunk> chunks = new List<Chunk>(chunkCount);
            for (int i = 0; i < chunkCount; i++)
            {
                chunks.Add(ReadChunk(reader));
            }

            System.Console.WriteLine($"Loaded {chunkCount} chunks");

            return chunks;
        }

        public static void WriteChunk(BinaryWriter writer, Chunk chunk)
        {
            writer.Write(chunk.X);
            writer.Write(chunk.Y);
            writer.Write(chunk.Size);

            for (int x = 0; x < chunk.Size; x++)
            {
                for (int y = 0; y < chunk.Size; y++)
                {
                    writer.Write(chunk.GetTile(x, y).Id);
                    writer.Write((byte)chunk.GetColor(x, y));
                }
            }
        }

        public static Chunk ReadChunk(BinaryReader reader)
        {
            short chunkX = reader.ReadInt16();
            short chunkY = reader.ReadInt16();
            byte chunkSize = reader.ReadByte();

            ushort[,] tiles = new ushort[chunkSize, chunkSize];
            GameColor[,] colors = new GameColor[chunkSize, chunkSize];

            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    ushort tileId = reader.ReadUInt16();
                    byte colorId = reader.ReadByte();
                    tiles[x, y] = tileId;
                    colors[x, y] = (GameColor)colorId;
                }
            }

            return new Chunk(chunkX, chunkY, chunkSize, tiles, colors);
        }
    }
}
