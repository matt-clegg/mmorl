using Lidgren.Network;

namespace MMORL.Shared.Net
{
    public interface IMessageHandler
    {
        void OnDataReceived(MessageType type, NetIncomingMessage data);
        void OnPlayerConnect(NetIncomingMessage data);
        void OnPlayerDisconnect(NetIncomingMessage data);
    }
}
