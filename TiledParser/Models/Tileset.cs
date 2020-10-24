using System.Collections.Generic;

namespace TiledParser.Models
{
    internal class Tileset
    {
        public string Name { get; set; }
        public ushort FirstGid { get; set; }
        public List<Tile> Tiles { get; set; }
    }
}
