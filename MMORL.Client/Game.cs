using Lidgren.Network;
using Microsoft.Xna.Framework.Input;
using MMORL.Client.Net;
using MMORL.Client.Scenes;
using MMORL.Shared;
using System;

namespace MMORL.Client
{
    public class Game
    {
        public const int SpriteWidth = 16;
        public const int SpriteHeight = 24;

        private readonly GameClient _client;
        private readonly ClientMessageHandler _messageHandler;

        private readonly GameWorld _gameWorld;

        private static Scene _scene;
        private static Scene _nextScene;

        public static Scene Scene {
            get => _scene;
            set => _nextScene = value;
        }

        private NetConnectionStatus _lastStatus;

        private readonly PlayScene _playScene;

        public Game()
        {
            //const string host = "127.0.0.1";
            const string host = "dev.matt.gd";

            const int port = 25501;
            const int chunkSize = 16;

            _client = new GameClient(host, port);

            _gameWorld = new GameWorld(chunkSize);
            _messageHandler = new ClientMessageHandler(_client, _gameWorld);

            // Create the play scene instance to ensure the OnEntityAdded event is subscribed to
            // before an entity is added to the game world.
            _playScene = new PlayScene(_gameWorld, _client);
            Scene = new LoginScene(this);
        }

        public void Input(Keys key)
        {
            Scene?.Input(key);
        }

        public void Update(float delta)
        {
            _client.Update(_messageHandler);
            Scene?.Update(delta);

            if (_scene != _nextScene)
            {
                _scene?.Unload();
                _scene = _nextScene;
                OnSceneTransition();
            }

            if (_lastStatus != _client.ClientStatus)
            {
                OnClientStatusChange(_client.ClientStatus);
            }
            _lastStatus = _client.ClientStatus;
        }

        public void Render()
        {
            Scene?.Render();
        }

        private void OnClientStatusChange(NetConnectionStatus status)
        {
            switch (status)
            {
                case NetConnectionStatus.Connected:
                    Scene = _playScene;
                    break;
            }
        }

        private void OnSceneTransition()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Engine.TimeRate = 1f;
        }

        public void Connect()
        {
            _client.Connect();
            Scene = new LoadingScene();
        }

        public void Dispose()
        {
            _client.Disconnect();
        }

    }
}
