using Lidgren.Network;
using MMORL.Client.Data;
using MMORL.Client.Entities;
using MMORL.Shared;
using MMORL.Shared.Entities;
using MMORL.Shared.Net;
using MMORL.Shared.World;
using System;
using System.Linq;
using Toolbox;

namespace MMORL.Client.Net
{
    public class ClientMessageHandler : IMessageHandler
    {
        private readonly GameClient _client;
        private readonly GameWorld _gameWorld;

        private readonly Pool _messagePool = new Pool();

        public ClientMessageHandler(GameClient client, GameWorld gameWorld)
        {
            _client = client;
            _gameWorld = gameWorld;
        }

        public void OnDataReceived(MessageType type, NetIncomingMessage data)
        {
            switch (type)
            {
                case MessageType.MapInformation:
                    {
                        MapInformationMessage message = new MapInformationMessage();
                        message.Read(data);

                        foreach (Tile tile in message.Tiles)
                        {
                            Tile.Register(tile);
                        }

                        SpriteLoader.LoadTiles(Tile.RegisteredTiles.Select(t => t.Id).ToList(), Engine.Assets);
                        break;
                    }
                case MessageType.ChunkData:
                    {
                        ChunkDataMessage message = _messagePool.Create<ChunkDataMessage>();
                        message.Read(data);
                        _gameWorld.Map.LoadChunk(message.Chunk);
                        _messagePool.Recycle(message);
                        break;
                    }
                case MessageType.MoveEntity:
                    {
                        MoveEntityMessage message = _messagePool.Create<MoveEntityMessage>();
                        message.Read(data);
                        _gameWorld.Map.MoveEntity(message.Id, message.X, message.Y);
                        _messagePool.Recycle(message);
                        break;
                    }
                case MessageType.SpawnEntity:
                    {
                        SpawnEntityMessage message = new SpawnEntityMessage();
                        message.Read(data);

                        Entity entity = null;

                        switch (message.EntityType)
                        {
                            case EntityType.LocalPlayer:
                                entity = new LocalPlayer(message.EntityId, message.Name, message.Sprite, message.Color, message.Speed, _client);
                                break;
                            case EntityType.Player:
                                entity = new Player(message.EntityId, message.Name, message.Sprite, message.Color, message.Speed);
                                break;
                            default:
                                Console.WriteLine($"Unhandled entity type: {message.EntityId}");
                                break;
                        }

                        if (entity != null)
                        {
                            _gameWorld.AddEntity(entity, message.X, message.Y);
                        }
                        break;
                    }
                default:
                    Console.WriteLine($"Unknown message type: {type}");
                    break;
            }
        }

        public void OnPlayerConnect(NetIncomingMessage data)
        {
        }

        public void OnPlayerDisconnect(NetIncomingMessage data)
        {
        }
    }
}
