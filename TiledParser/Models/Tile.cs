using System.Collections.Generic;
using System.Linq;

namespace TiledParser.Models
{
    public class Tile
    {
        public ushort Id { get; set; }
        public List<CustomProperty> Properties { get; set; }

        public bool GetProperty(string key)
        {
            return bool.Parse(Properties.FirstOrDefault(p => p.Name.Equals(key)).Value);
        }

        public bool HasProperty(string key)
        {
            return Properties?.Any(p => p.Name.Equals(key)) ?? false;
        }
    }
}
