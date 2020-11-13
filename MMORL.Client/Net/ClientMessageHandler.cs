using Lidgren.Network;
using MMORL.Client.Data;
using MMORL.Client.Entities;
using MMORL.Shared;
using MMORL.Shared.Entities;
using MMORL.Shared.Net;
using MMORL.Shared.Net.Messages;
using MMORL.Shared.World;
using System;
using System.Linq;

namespace MMORL.Client.Net
{
    public class ClientMessageHandler : IMessageHandler
    {
        private readonly GameClient _client;
        private readonly GameWorld _gameWorld;

        // Checks to see if we have the minimum data needed to play. Needs polishing.
        public bool SpawnedPlayer { get; private set; }
        public bool RegisterdTiles { get; private set; }

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

                        RegisterdTiles = true;
                        SpriteLoader.LoadTiles(Tile.RegisteredTiles.Select(t => t.Id).ToList(), Engine.Assets);
                        break;
                    }
                case MessageType.ChunkData:
                    {
                        ChunkDataMessage message = new ChunkDataMessage();
                        message.Read(data);
                        _gameWorld.Map.LoadChunk(message.Chunk);
                        break;
                    }
                case MessageType.MoveEntity:
                    {
                        MoveEntityMessage message = new MoveEntityMessage();
                        message.Read(data);
                        _gameWorld.Map.MoveEntity(message.Id, message.X, message.Y);
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
                                SpawnedPlayer = true;
                                break;
                            case EntityType.Player:
                                entity = new Player(message.EntityId, message.Name, message.Sprite, message.Color, message.Speed);
                                break;
                            case EntityType.Mob:
                                entity = new Creature(message.EntityId, message.Name, message.Sprite, message.Color, message.Speed);
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
                case MessageType.RemoveEntity:
                    {
                        RemoveEntityMessage message = new RemoveEntityMessage();
                        message.Read(data);

                        _gameWorld.RemoveEntity(message.EntityId);

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
            string reason = data.ReadString();
            Console.WriteLine("disconnected: " + reason);
        }
    }
}
