using System.Collections.Generic;

namespace MMORL.Shared.World
{
    public class MobSpawnDefinition
    {
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }
        public List<string> Mobs { get; }

        public MobSpawnDefinition(int x, int y, int width, int height, List<string> mobs)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Mobs = mobs;
        }
    }
}
