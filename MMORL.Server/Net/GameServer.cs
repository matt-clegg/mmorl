using Lidgren.Network;
using MMORL.Shared.Net;
using System;

namespace MMORL.Server.Net
{
    public class GameServer
    {
        public int Port { get; }

        private readonly NetServer _server;

        public GameServer(int port)
        {
            Port = port;

            NetPeerConfiguration config = new NetPeerConfiguration("mmorl");
            config.Port = port;

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            _server = new NetServer(config);
        }

        public void Start()
        {
            Console.WriteLine("Starting server...");
            _server.Start();
        }

        public void Shutdown()
        {
            Console.WriteLine("Stopping server...");
            _server.Shutdown("Server stopped");
        }

        public void Update(IMessageHandler messageHandler)
        {
            NetIncomingMessage message;
            while ((message = _server.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        MessageType type = (MessageType)message.ReadByte();
                        messageHandler.OnDataReceived(type, message);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)message.ReadByte();

                        Console.WriteLine($"{NetUtility.ToHexString(message.SenderConnection.RemoteUniqueIdentifier)}: {status}");

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
                    case NetIncomingMessageType.ConnectionApproval:
                        Console.WriteLine($"Attempting to approve connection: {message.SenderConnection}");
                        try
                        {
                            // TODO: Sleep to simulate connecting
                            //Thread.Sleep(2000);
                            message.SenderConnection.Approve();
                        }
                        catch
                        {
                            message.SenderConnection.Disconnect("Unable to connect");
                        }
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(message.ReadString());
                        break;
                    case NetIncomingMessageType.ConnectionLatencyUpdated:
                        break;
                    default:
                        Console.WriteLine($"Unhandled type: {message.MessageType}");
                        break;
                }
                _server.Recycle(message);
            }
        }

        public void SendMessageToAll(IMessage message, NetDeliveryMethod deliveryMethod)
        {
            NetOutgoingMessage outgoing = WriteMessage(message);
            _server.SendToAll(outgoing, deliveryMethod);
        }

        public void SendMessage(IMessage message, NetConnection connection, NetDeliveryMethod deliveryMethod)
        {
            NetOutgoingMessage outgoing = WriteMessage(message);
            _server.SendMessage(outgoing, connection, deliveryMethod);
        }

        private NetOutgoingMessage WriteMessage(IMessage message)
        {
            NetOutgoingMessage outgoing = _server.CreateMessage();
            outgoing.Write((byte)message.Type);
            message.Write(outgoing);
            return outgoing;
        }
    }
}
