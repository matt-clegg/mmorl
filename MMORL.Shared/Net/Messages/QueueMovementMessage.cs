using Lidgren.Network;

namespace MMORL.Shared.Net.Messages
{
    public class QueueMovementMessage : IMessage
    {
        public MessageType Type => MessageType.QueueMovement;

        public int EntityId { get; private set; }
        public short X { get; private set; }
        public short Y { get; private set; }

        public QueueMovementMessage() { }

        public QueueMovementMessage(int entityId, int x, int y)
        {
            EntityId = entityId;
            X = (short)x;
            Y = (short)y;
        }

        public void Read(NetIncomingMessage message)
        {
            EntityId = message.ReadInt32();
            X = message.ReadInt16();
            Y = message.ReadInt16();
        }

        public void Write(NetOutgoingMessage message)
        {
            message.Write(EntityId);
            message.Write(X);
            message.Write(Y);
        }
    }
}
