using Lidgren.Network;
using MMORL.Server.Entities;
using MMORL.Shared.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Toolbox;

namespace MMORL.Server.Net
{
    public class GameServer
    {
        public int Port { get; }

        private readonly NetServer _server;

        private readonly List<PlayerNetConnection> _playerConnections = new List<PlayerNetConnection>();

        public Pool Pool { get; private set; }

        public GameServer(int port)
        {
            Port = port;

            Pool = new Pool();

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
                            Thread.Sleep(1000);
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

        public void AddNewPlayerConnection(NetConnection netConnection, ServerEntity player)
        {
            _playerConnections.Add(new PlayerNetConnection(netConnection, player));
        }

        public void RemovePlayerConnection(NetConnection netConnection)
        {
            _playerConnections.RemoveAll(con => con.NetConnection.RemoteUniqueIdentifier == netConnection.RemoteUniqueIdentifier);
        }

        public void RemovePlayerConnection(ServerEntity player)
        {
            _playerConnections.RemoveAll(con => con.Player.Id == player.Id);
        }

        public NetConnection GetConnectionForPlayer(ServerEntity player)
        {
            return _playerConnections.FirstOrDefault(con => con.Player.Id == player.Id)?.NetConnection;
        }

        public ServerEntity GetPlayerFromConnection(NetConnection netConnection)
        {
            return _playerConnections.FirstOrDefault(con => con.NetConnection.RemoteUniqueIdentifier == netConnection.RemoteUniqueIdentifier)?.Player;
        }

        public void SendMessageToAll(IMessage message, NetDeliveryMethod deliveryMethod)
        {
            NetOutgoingMessage outgoing = WriteMessage(message);
            _server.SendToAll(outgoing, deliveryMethod);
        }

        public void SendMessageToAllExcept(IMessage message, NetConnection except, NetDeliveryMethod deliveryMethod)
        {
            NetOutgoingMessage outgoing = WriteMessage(message);
            _server.SendToAll(outgoing, except, deliveryMethod, 0);
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
