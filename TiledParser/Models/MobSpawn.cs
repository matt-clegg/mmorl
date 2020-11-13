using System;
using System.Collections.Generic;
using System.Text;

namespace TiledParser.Models
{
    public class MobSpawn
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<string> Mobs { get; set; }
    }
}
