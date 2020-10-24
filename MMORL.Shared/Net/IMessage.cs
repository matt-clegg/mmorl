using Lidgren.Network;

namespace MMORL.Shared.Net
{
    public interface IMessage
    {
        MessageType Type { get; }
        void Write(NetOutgoingMessage message);
        void Read(NetIncomingMessage message);
    }
}
