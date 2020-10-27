using Lidgren.Network;

namespace MMORL.Shared.Net.Messages
{
    public class RemoveEntityMessage : IMessage
    {
        public MessageType Type => MessageType.RemoveEntity;

        public int EntityId { get; set; }

        public RemoveEntityMessage() { }

        public RemoveEntityMessage(int entityId)
        {
            EntityId = entityId;
        }

        public void Read(NetIncomingMessage message)
        {
            EntityId = message.ReadInt32();
        }

        public void Write(NetOutgoingMessage message)
        {
            message.Write(EntityId);
        }
    }
}
