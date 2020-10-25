using Lidgren.Network;

namespace MMORL.Shared.Net.Messages
{
    public class ClearMovesMessage : IMessage
    {
        public MessageType Type => MessageType.ClearMoves;

        public int EntityId { get; set; }

        public ClearMovesMessage() { }

        public ClearMovesMessage(int entityId)
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
