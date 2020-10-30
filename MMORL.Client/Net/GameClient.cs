using Lidgren.Network;
using MMORL.Shared.Net;
using System;
using System.Threading;

namespace MMORL.Client.Net
{
    public class GameClient
    {
        public string Host { get; }
        public int Port { get; }

        private readonly NetClient _client;

        public NetConnectionStatus ClientStatus { get; private set; }

        public NetPeerStatistics Statistics => _client.Statistics;

        public GameClient(string host, int port)
        {
            Host = host;
            Port = port;

            NetPeerConfiguration config = new NetPeerConfiguration("mmorl");

            _client = new NetClient(config);
            _client.Start();
        }

        public void Connect()
        {
            try
            {
                _client.Connect(Host, Port);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect: {ex.Message}");
            }
        }

        public void Update(IMessageHandler messageHandler)
        {
            ClientStatus = _client.ConnectionStatus;

            NetIncomingMessage message;
            while ((message = _client.ReadMessage()) != null)
            {

                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        MessageType type = (MessageType)message.ReadByte();
                        messageHandler.OnDataReceived(type, message);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)message.ReadByte();

                        if (status == NetConnectionStatus.Connected)
                        {
                            if (message.SenderConnection.RemoteHailMessage != null)
                            {
                                Console.WriteLine($"Remote hail: {message.SenderConnection.RemoteHailMessage.ReadString()}");
                            }

                            messageHandler.OnPlayerConnect(message);
                        }
                        else if (status == NetConnectionStatus.Disconnected)
                        {
                            messageHandler.OnPlayerDisconnect(message);
                        }

                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(message.ReadString());
                        break;
                    default:
                        Console.WriteLine($"Unhandled type: {message.MessageType}"); ;
                        break;
                }
                _client.Recycle(message);
            }
        }

        public void SendMessage(IMessage message, NetDeliveryMethod deliveryMethod)
        {
            NetOutgoingMessage outgoing = _client.CreateMessage();
            outgoing.Write((byte)message.Type);
            message.Write(outgoing);
            _client.SendMessage(outgoing, deliveryMethod);
        }

        public void Disconnect()
        {
            const string reason = "client closed";
            _client.Disconnect(reason);
            _client.ServerConnection.Disconnect(reason);
            _client.Shutdown(reason);
            // Have to give the client thread time to shutdown properly before the
            // game closes.
            Thread.Sleep(100);
            _client.FlushSendQueue();
        }
    }
}
