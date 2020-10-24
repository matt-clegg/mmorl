namespace TiledParser.Models
{
    internal class Chunk
    {
        public int[] Data { get; set; }
        public byte Width { get; set; }
        public byte Height { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
    }
}
