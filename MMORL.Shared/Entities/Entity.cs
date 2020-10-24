using MMORL.Shared.Util;
using MMORL.Shared.World;

namespace MMORL.Shared.Entities
{
    public class Entity
    {
        public int Id { get; }
        public string Name { get; }
        public int X { get; set; }
        public int Y { get; set; }

        public int Speed { get; }

        public string Sprite { get; }
        public GameColor Color { get; }

        public Map Map { get; private set; }

        public Entity(int id, string name, string sprite, GameColor color, int speed)
        {
            Id = id;
            Name = name;
            Sprite = sprite;
            Color = color;
            Speed = speed;
        }

        public void Initialise(Map map, int x, int y)
        {
            X = x;
            Y = y;
            Map = map;
        }

        public virtual void ProcessTurn()
        {

        }
    }
}
