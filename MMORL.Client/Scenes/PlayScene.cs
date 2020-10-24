using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MMORL.Client.Entities;
using MMORL.Client.Renderers;
using MMORL.Shared;
using MMORL.Shared.Entities;

namespace MMORL.Client.Scenes
{
    public class PlayScene : Scene
    {
        private readonly GameWorld _gameWorld;
        private readonly MapRenderer _mapRenderer;

        private LocalPlayer _player;

        public PlayScene(GameWorld gameWorld)
        {
            _gameWorld = gameWorld;
            _gameWorld.EntityAddedEvent += OnEntityAdded;

            Camera.Origin = new Vector2(Engine.Width / 2, Engine.Height / 2);

            _mapRenderer = new MapRenderer(_gameWorld.Map, Camera);
            Add(_mapRenderer);
        }

        public override void Input(Keys key)
        {
            //_player?.Input(key);
        }

        public override void Update(float delta)
        {
            base.Update(delta);
        }

        public override void Unload()
        {
            base.Unload();
            _gameWorld.EntityAddedEvent -= OnEntityAdded;
        }

        private void OnEntityAdded(object sender, Entity entity)
        {
            if(entity is LocalPlayer player)
            {
                _player = player;
                Camera.X = _player.X * Game.SpriteWidth + (Game.SpriteWidth / 2);
                Camera.Y = _player.Y * Game.SpriteHeight + (Game.SpriteHeight / 2);
            }
        }
    }
}
