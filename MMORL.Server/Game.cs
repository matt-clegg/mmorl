﻿using MMORL.Server.Net;
using MMORL.Server.World;
using MMORL.Shared.Loaders;
using MMORL.Shared.World;

namespace MMORL.Server
{
    public class Game
    {
        private readonly GameServer _server;
        private readonly ServerMessageHandler _messageHandler;

        private readonly ServerWorld _gameWorld;

        public Game(int port, int chunkSize)
        {
            _server = new GameServer(port);
            _server.Start();

            Map map = MapLoader.LoadFromFile("Data/export.dat", chunkSize);

            const float turnTime = 0.25f;

            _gameWorld = new ServerWorld(map, turnTime, _server);
            _messageHandler = new ServerMessageHandler(_server, _gameWorld);
        }

        public void Update(float delta)
        {
            _server.Update(_messageHandler);
            _gameWorld.Update(delta);
        }
    }
}
