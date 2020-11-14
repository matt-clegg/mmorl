using Lidgren.Network;
using Microsoft.Xna.Framework.Input;
using MMORL.Client.Auth;
using MMORL.Client.Net;
using MMORL.Client.Scenes;
using MMORL.Shared;
using MMORL.Shared.Net.Messages;
using System;
using System.Threading;

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

        public bool WaitForData { get; set; } = false;

        public Game()
        {
            const string host = "127.0.0.1";
            //const string host = "dev.matt.gd";

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

            if(WaitForData && _messageHandler.SpawnedPlayer && _messageHandler.RegisterdTiles)
            {
                Scene = _playScene;
                WaitForData = false;
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
                    if(Scene is LoadingScene loadingScene)
                    {
                        loadingScene.Text = "Loading...";
                    }

                    WaitForData = true;
                    break;
            }
        }

        private void OnSceneTransition()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Engine.TimeRate = 1f;
        }

        public void Connect(string username, string password)
        {
            // Attempt to login on a separate thread to ensure the http request doesn't block the game.
            Thread thread = new Thread(() =>
            {
                string token = new AuthenticationManager().Login(username, password);
                LoginMessage loginMessage = new LoginMessage(token);
                _client.Connect(loginMessage);
            });
            thread.Start();

            Scene = new LoadingScene("Connecting...");
        }

        public void Dispose()
        {
            _client.Disconnect();
        }
    }
}
