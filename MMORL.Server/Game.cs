using MMORL.Server.Net;
using MMORL.Server.World;
using MMORL.Shared.Loaders;
using MMORL.Shared.World;
using System;

namespace MMORL.Server
{
    public class Game
    {
        private readonly GameServer _server;
        private readonly ServerMessageHandler _messageHandler;

        private readonly ServerWorld _gameWorld;

        public Game()
        {
            int port = Settings.Port;
            int chunkSize = Settings.ChunkSize;

            if (chunkSize < 8 || chunkSize > 128)
            {
                throw new ArgumentOutOfRangeException($"Invalid chunk size: {chunkSize}. Value must be between 8 and 128.");
            }

            _server = new GameServer(port);
            _server.Start();

            Map map = MapLoader.LoadFromFile("Data/export.dat", chunkSize);

            float turnTime = Settings.TurnTime;

            if (turnTime < 0)
            {
                throw new InvalidOperationException($"Invalid turn time: {turnTime}. Value must be greater than zero.");
            }

            _gameWorld = new ServerWorld(map, turnTime, _server);
            _messageHandler = new ServerMessageHandler(_server, _gameWorld);
        }

        public void Update(float delta)
        {
            _server.Update(_messageHandler);
            _gameWorld.Update(delta);
        }

        public void Shutdown()
        {
            _server.Shutdown();
        }
    }
}
