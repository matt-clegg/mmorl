using Lidgren.Network;
using MMORL.Server.Auth;
using MMORL.Server.Entities;
using MMORL.Shared.Net;
using MMORL.Shared.Net.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace MMORL.Server.Net
{
    public class GameServer
    {
        public int Port { get; }
        private static readonly HttpClient _client = new HttpClient();

        private readonly NetServer _server;
        public bool Running { get; set; }

        private readonly List<PlayerNetConnection> _playerConnections = new List<PlayerNetConnection>();

        public IMessageHandler MessageHandler { get; set; }

        public static Random Random { get; } = new Random();

        public GameServer(int port)
        {
            Port = port;

            _client.BaseAddress = new Uri(Settings.ApiEndpoint);

            NetPeerConfiguration config = new NetPeerConfiguration("mmorl");
            config.Port = port;

#if DEBUG
            config.SimulatedMinimumLatency = 0.015f; // min 15ms ping (TO THE CLIENTS, NOT ROUND TRIP)
            config.SimulatedRandomLatency = 0.005f; // 5ms randomness
#endif

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            _server = new NetServer(config);
        }

        public void Start()
        {
            Console.WriteLine("Starting server...");

            Thread thread = new Thread(Listen);
            thread.Start();
            Running = true;

            _server.Start();
        }

        public void Shutdown()
        {
            Console.WriteLine("Stopping server...");
            Running = false;
            _server.Shutdown("Server stopped");
        }

        private void Listen()
        {
            while (Running)
            {
                _server.MessageReceivedEvent.WaitOne();

                NetIncomingMessage message;
                while ((message = _server.ReadMessage()) != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            MessageType type = (MessageType)message.ReadByte();
                            MessageHandler.OnDataReceived(type, message);
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

                                MessageHandler.OnPlayerConnect(message);
                            }
                            else if (status == NetConnectionStatus.Disconnected)
                            {
                                MessageHandler.OnPlayerDisconnect(message);
                            }

                            break;
                        case NetIncomingMessageType.ConnectionApproval:
                            Console.WriteLine($"Attempting to approve connection: {message.SenderConnection}");
                            try
                            {
                                MessageType messageType = (MessageType)message.ReadByte();
                                if (messageType == MessageType.Login)
                                {
                                    // TODO: Authentication code should live somewhere else
                                    LoginMessage loginMessage = new LoginMessage();
                                    loginMessage.Read(message);

                                    Console.WriteLine("Received token " + loginMessage.Token);

                                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/auth/authenticated");
                                    request.Headers.Add("token", loginMessage.Token);
                                    var result = _client.SendAsync(request).Result;
                                    string resultContent = result.Content.ReadAsStringAsync().Result;
                                    Console.WriteLine("result of authenticated " + resultContent);

                                    // Approve sender connection based on above result
                                    message.SenderConnection.Approve();
                                }
                                else
                                {
                                    Console.WriteLine("Invalid login message type " + messageType);
                                    message.SenderConnection.Disconnect("Unable to connect");
                                }
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
