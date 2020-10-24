using System.Collections.Generic;
using System.Linq;

namespace TiledParser.Models
{
    internal class Map
    {
        public List<Layer> Layers { get; set; }
        public List<Tileset> Tilesets { get; set; }
        public ushort Width { get; set; }
        public ushort Height { get; set; }

        public Tileset GetTilesetByName(string name)
        {
            return Tilesets.FirstOrDefault(t => t.Name.Equals(name));
        }

        public Layer GetLayerByName(string name)
        {
            return Layers.FirstOrDefault(l => l.Name.Equals(name));
        }

        public Chunk GetChunkFromLayer(int x, int y, string layerName)
        {
            Layer layer = Layers.FirstOrDefault(l => l.Name.ToLower().Equals(layerName.ToLower()));
            return layer.Chunks.FirstOrDefault(c => c.X == x && c.Y == y);
        }
    }
}
