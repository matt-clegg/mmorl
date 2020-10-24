using MMORL.Shared.Util;

namespace MMORL.Shared.Entities
{
    public class Entity
    {
        public int Id { get; }
        public string Name { get; }
        public int X { get; set; }
        public int Y { get; set; }

        public string Sprite { get; }
        public GameColor Color { get; }

        public Entity(int id, string name, string sprite, GameColor color)
        {
            Id = id;
            Name = name;
            Sprite = sprite;
            Color = color;
        }
    }
}
