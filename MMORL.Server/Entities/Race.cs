using MMORL.Shared.Util;

namespace MMORL.Server.Entities
{
    public class Race
    {
        public string Name { get; }
        public string Sprite { get; }
        public GameColor Color { get; }
        public int Speed { get; set; }

        public Race(string name, string sprite, GameColor color, int speed, Race parent = null)
        {
            Name = parent?.Name ?? name;
            Sprite = parent?.Sprite ?? sprite;
            Color = parent?.Color ?? color;
            Speed = parent?.Speed ?? speed;
        }
    }
}
