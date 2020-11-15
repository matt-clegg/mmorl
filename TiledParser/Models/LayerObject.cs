using System.Collections.Generic;
using System.Linq;

namespace TiledParser.Models
{
    internal class LayerObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<CustomProperty> Properties { get; set; }

        public bool GetPropertyValue(string key)
        {
            return bool.Parse(Properties.FirstOrDefault(p => p.Name.Equals(key)).Value);
        }

        public CustomProperty GetProperty(string key)
        {
            return Properties.FirstOrDefault(p => p.Name.Equals(key));
        }

        public bool HasProperty(string key)
        {
            return Properties?.Any(p => p.Name.Equals(key)) ?? false;
        }
    }
}
