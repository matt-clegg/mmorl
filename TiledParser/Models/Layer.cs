using System.Collections.Generic;
using System.Linq;

namespace TiledParser.Models
{
    internal class Layer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Chunk> Chunks { get; set; }
        public List<LayerObject> Objects { get; set; }

        public Chunk GetChunkAt(int x, int y)
        {
            return Chunks.FirstOrDefault(c => c.X / c.Width == x && c.Y / c.Width == y);
        }

        public LayerObject GetObjectById(int id)
        {
            return Objects.FirstOrDefault(o => o.Id == id);
        }
    }
}
