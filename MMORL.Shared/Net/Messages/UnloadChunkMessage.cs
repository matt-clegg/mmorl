using Lidgren.Network;
using Toolbox;

namespace MMORL.Shared.Net.Messages
{
    public class UnloadChunkMessage : IMessage
    {
        public MessageType Type => MessageType.UnloadChunkMessage;

        public Point2D Chunk { get; set; }

        public UnloadChunkMessage() { }

        public UnloadChunkMessage(Point2D chunk)
        {
            Chunk = chunk;
        }

        public void Read(NetIncomingMessage message)
        {
            Chunk = new Point2D(message.ReadInt16(), message.ReadInt16());
        }

        public void Write(NetOutgoingMessage message)
        {
            message.Write((short)Chunk.X);
            message.Write((short)Chunk.Y);
        }
    }
}
