using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MMORL.Client.Entities;
using MMORL.Client.Input;
using MMORL.Client.Net;
using MMORL.Client.Renderers;
using MMORL.Client.Util;
using MMORL.Shared;
using MMORL.Shared.Entities;
using System.Collections.Generic;
using System.Linq;
using Toolbox;

namespace MMORL.Client.Scenes
{
    public class PlayScene : Scene
    {
        private readonly GameWorld _gameWorld;
        private readonly MapRenderer _mapRenderer;
        private readonly MouseManager _mouseManager;

        private LocalPlayer _player;

        public PlayScene(GameWorld gameWorld, GameClient client)
        {
            _gameWorld = gameWorld;
            _gameWorld.EntityAddedEvent += OnEntityAdded;

            Camera.Origin = new Vector2(Engine.Width / 2, Engine.Height / 2);

            _mouseManager = new MouseManager(Camera);

            _mapRenderer = new MapRenderer(_gameWorld.Map, Camera, _mouseManager);
            Add(_mapRenderer);
            //Add(new UiRenderer(client.Statistics, Camera));
        }

        public override void Input(Keys key)
        {
            _player?.Input(key);
        }

        public override void Update(float delta)
        {
            _mouseManager.Update();

            base.Update(delta);

            if (_player != null)
            {
                Camera.Approach(new Vector2(_player.RenderX + (Game.SpriteWidth / 2), _player.RenderY + (Game.SpriteHeight / 2)), 0.1f);
            }

            _gameWorld.Update(delta);
        }

        public override void Unload()
        {
            base.Unload();
            _gameWorld.EntityAddedEvent -= OnEntityAdded;
            if (_player != null)
            {
                _player.ChunkChangedEvent -= OnPlayerChunkChange;
            }
        }

        private void OnEntityAdded(object sender, Entity entity)
        {
            if (entity is LocalPlayer player)
            {
                _player = player;
                _player.MoveEvent += OnPlayerChunkChange;
                Camera.X = _player.X * Game.SpriteWidth + (Game.SpriteWidth / 2);
                Camera.Y = _player.Y * Game.SpriteHeight + (Game.SpriteHeight / 2);
            }

            if (entity is Creature creature)
            {
                creature.RenderX = entity.X * Game.SpriteWidth;
                creature.RenderY = entity.Y * Game.SpriteHeight;
            }
        }

        private void OnPlayerChunkChange(object sender, Point2D position)
        {
            Point2D chunkPos = _gameWorld.Map.ToChunkCoords(position.X, position.Y);

            HashSet<Point2D> safeChunks = new HashSet<Point2D>();

            for (int x = -3; x <= 3; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    safeChunks.Add(chunkPos + new Point2D(x, y));
                }
            }

            foreach (Point2D allChunks in _gameWorld.Map.Chunks.Select(c => new Point2D(c.X, c.Y)))
            {
                if (!safeChunks.Contains(allChunks))
                {
                    _gameWorld.Map.UnloadChunk(allChunks.X, allChunks.Y);
                }
            }
        }
    }
}

