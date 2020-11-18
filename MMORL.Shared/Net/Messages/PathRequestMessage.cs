using Lidgren.Network;
using System.Collections.Generic;
using Toolbox;

namespace MMORL.Shared.Net.Messages
{
    public class PathRequestMessage : IMessage
    {
        public MessageType Type => MessageType.PathRequest;

        public int EntityId { get; set; }
        public List<Point2D> Path { get; set; }

        public PathRequestMessage() { }

        public PathRequestMessage(int entityId, List<Point2D> path)
        {
            EntityId = entityId;
            Path = path;
        }

        public void Read(NetIncomingMessage message)
        {
            EntityId = message.ReadInt32();
            ushort count = message.ReadUInt16();
            Path = new List<Point2D>(count);

            for (int i = 0; i < count; i++)
            {
                Point2D point = new Point2D(message.ReadInt16(), message.ReadInt16());
                Path.Add(point);
            }
        }

        public void Write(NetOutgoingMessage message)
        {
            message.Write(EntityId);
            message.Write((ushort)Path.Count);
            foreach (Point2D point in Path)
            {
                message.Write((short)point.X);
                message.Write((short)point.Y);
            }
        }
    }
}
