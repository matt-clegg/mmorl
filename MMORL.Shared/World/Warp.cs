namespace MMORL.Shared.World
{
    public class Warp
    {
        public int Id { get; }
        public string Name { get; }
        public int X { get; set; }
        public int Y { get; set; }

        public int TargetId { get; set; }

        public Warp(int id, string name, int x, int y, int targetId)
        {
            Id = id;
            Name = name;
            X = x;
            Y = y;
            TargetId = targetId;
        }
    }
}
