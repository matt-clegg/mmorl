using MMORL.Shared.World;
using System.Collections.Generic;
using System.IO;

namespace MMORL.Server.Loaders
{
    public static class SpawnLoader
    {
        public static List<MobSpawnDefinition> Load(BinaryReader reader, int tileWidth, int tileHeight)
        {
            ushort count = reader.ReadUInt16();
            List<MobSpawnDefinition> spawns = new List<MobSpawnDefinition>(count);

            for (int i = 0; i < count; i++)
            {
                int x = reader.ReadInt32() / tileWidth;
                int y = reader.ReadInt32() / tileHeight;
                int width = reader.ReadInt32() / tileWidth;
                int height = reader.ReadInt32() / tileHeight;

                byte mobCount = reader.ReadByte();
                List<string> mobs = new List<string>(mobCount);
                for (int j = 0; j < mobCount; j++)
                {
                    string mob = reader.ReadString().Trim();
                    mobs.Add(mob);
                }

                spawns.Add(new MobSpawnDefinition(x, y, width, height, mobs));
            }

            return spawns;
        }
    }
}
