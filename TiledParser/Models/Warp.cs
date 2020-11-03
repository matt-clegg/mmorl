using System;
using System.Collections.Generic;
using System.Text;

namespace TiledParser.Models
{
    public class Warp
    {
        public int Id { get; set; }
        public short StartX { get; set; }
        public short StartY { get; set; }
        public short EndX { get; set; }
        public short EndY { get; set; }
    }
}
