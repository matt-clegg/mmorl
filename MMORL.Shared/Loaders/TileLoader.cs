using MMORL.Shared.World;
using System;
using System.Collections.Generic;
using System.IO;

namespace MMORL.Shared.Loaders
{
    public static class TileLoader
    {
        public static List<Tile> Load(BinaryReader reader)
        {
            ushort count = reader.ReadUInt16();
            List<Tile> tiles = new List<Tile>(count);

            for (int i = 0; i < count; i++)
            {
                ushort id = reader.ReadUInt16();
                bool isSolid = reader.ReadBoolean();
                bool isTransparent = reader.ReadBoolean();
                tiles.Add(new Tile(id, isSolid, isTransparent));
            }

            Console.WriteLine($"Loaded {count} tiles");

            return tiles;
        }
    }
}
