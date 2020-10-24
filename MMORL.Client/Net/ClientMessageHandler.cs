﻿using Lidgren.Network;
using MMORL.Client.Data;
using MMORL.Client.Entities;
using MMORL.Shared;
using MMORL.Shared.Entities;
using MMORL.Shared.Net;
using MMORL.Shared.World;
using System;
using System.Linq;

namespace MMORL.Client.Net
{
    public class ClientMessageHandler : IMessageHandler
    {
        private readonly GameClient _client;
        private readonly GameWorld _gameWorld;

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
                        ChunkDataMessage message = new ChunkDataMessage();
                        message.Read(data);
                        _gameWorld.Map.LoadChunk(message.Chunk);
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
                                entity = new LocalPlayer(message.EntityId, message.Name, message.Sprite, message.Color);
                                break;
                            case EntityType.Player:
                                entity = new Player(message.EntityId, message.Name, message.Sprite, message.Color);
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
