using MMORL.Shared.World;
using System.Collections.Generic;
using System.IO;

namespace MMORL.Shared.Loaders
{
    public static class WarpLoader
    {
        public static List<Warp> Load(BinaryReader reader)
        {
            ushort count = reader.ReadUInt16();
            List<Warp> warps = new List<Warp>(count);

            for (int i = 0; i < count; i++)
            {
                int id = reader.ReadInt32();
                string name = reader.ReadString();
                short startX = reader.ReadInt16();
                short startY = reader.ReadInt16();
                int targetId = reader.ReadInt32();

                warps.Add(new Warp(id, name, startX, startY, targetId));
            }

            return warps;
        }
    }
}
