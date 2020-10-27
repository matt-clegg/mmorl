using Lidgren.Network;
using MMORL.Shared.Util;
using MMORL.Shared.World;
using Toolbox;

namespace MMORL.Shared.Net
{
    public class ChunkDataMessage : Poolable, IMessage
    {
        public MessageType Type => MessageType.ChunkData;

        public Chunk Chunk { get; private set; }

        public ChunkDataMessage() { }

        public ChunkDataMessage(Chunk chunk)
        {
            Chunk = chunk;
        }

        public void Read(NetIncomingMessage message)
        {
            short chunkX = message.ReadInt16();
            short chunkY = message.ReadInt16();
            byte size = message.ReadByte();

            ushort[,] tiles = new ushort[size, size];
            GameColor[,] colors = new GameColor[size, size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    tiles[x, y] = message.ReadUInt16();
                    colors[x, y] = (GameColor)message.ReadByte();
                }
            }

            Chunk = new Chunk(chunkX, chunkY, size, tiles, colors);
        }

        public void Write(NetOutgoingMessage message)
        {
            message.Write(Chunk.X);
            message.Write(Chunk.Y);
            message.Write(Chunk.Size);

            for (int x = 0; x < Chunk.Size; x++)
            {
                for (int y = 0; y < Chunk.Size; y++)
                {
                    message.Write(Chunk.GetTile(x, y).Id);
                    message.Write((byte)Chunk.GetColor(x, y));
                }
            }
        }

        public override void Recycle()
        {
            Chunk = null;
        }
    }
}