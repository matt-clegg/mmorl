using Lidgren.Network;
using MMORL.Shared;
using MMORL.Shared.Entities;
using MMORL.Shared.Net;
using MMORL.Shared.Util;
using MMORL.Shared.World;
using System;
using System.Collections.Generic;
using System.Linq;
using Toolbox;

namespace MMORL.Server.Net
{
    public class ServerMessageHandler : IMessageHandler
    {
        private readonly GameServer _server;
        private readonly GameWorld _gameWorld;

        public ServerMessageHandler(GameServer server, GameWorld gameWorld)
        {
            _server = server;
            _gameWorld = gameWorld;
        }

        public void OnDataReceived(MessageType type, NetIncomingMessage data)
        {
            switch (type)
            {
                default:
                    Console.WriteLine($"Unknown message type: {type}");
                    break;
            }
        }

        public void OnPlayerConnect(NetIncomingMessage data)
        {

            /**
             * TODO
             * 
             */

            Map map = _gameWorld.Map;

            Entity player = new Entity(map.Entities.Count, "player", "player", GameColor.Light);
            _gameWorld.AddEntity(player, 0, 2);

            SpawnEntityMessage spawnLocalPlayer = new SpawnEntityMessage(player, player.X, player.Y, EntityType.LocalPlayer);
            _server.SendMessage(spawnLocalPlayer, data.SenderConnection, NetDeliveryMethod.ReliableUnordered);

            List<Tile> tiles = Tile.RegisteredTiles.ToList();

            MapInformationMessage mapInformationMessage = new MapInformationMessage(tiles);
            _server.SendMessage(mapInformationMessage, data.SenderConnection, NetDeliveryMethod.ReliableUnordered);

            Point2D chunkPos = map.ToChunkCoords(player.X, player.Y);

            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    Chunk chunk = map.GetChunk(chunkPos.X + x, chunkPos.Y + y);
                    if (chunk != null)
                    {
                        SendChunk(chunk, data);
                    }
                }
            }
        }

        public void OnPlayerDisconnect(NetIncomingMessage data)
        {
        }

        private void SendChunk(Chunk chunk, NetIncomingMessage data)
        {
            ChunkDataMessage chunkDataMessage = new ChunkDataMessage(chunk);
            _server.SendMessage(chunkDataMessage, data.SenderConnection, NetDeliveryMethod.ReliableUnordered);
        }
    }
}
