using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MMORL.Client.Extensions;
using MMORL.Client.Renderers;
using MMORL.Client.Util;
using MMORL.Shared.Util;
using System;
using Toolbox.Graphics;

namespace MMORL.Client.Scenes
{
    public class LoadingScene : Scene
    {
        private readonly Sprite _loadingSprite;
        private readonly Color _color;

        // Start off screen
        private int _x = -100;
        private int _y = -100;

        private float _bounce;
        private float _time;

        private readonly DumbRenderer _renderer;

        public LoadingScene()
        {
            _renderer = new DumbRenderer(Camera);

            _loadingSprite = Engine.Assets.GetAsset<Sprite>("loading");
            _color = GameColor.Blood.ParseColor();
        }

        public override void Input(Keys key)
        {
        }

        public override void Update(float delta)
        {
            _time += delta;
            _bounce = (float)Math.Sin(_time * 1.5f + 0.5f) * 10f;

            _x = Camera.Viewport.Width / 2 - (Game.SpriteWidth / 2);
            _y = (int)(Camera.Viewport.Height / 2 - (Game.SpriteHeight / 2) + _bounce);

            base.Update(delta);
        }

        public override void Render()
        {
            _renderer.Begin();
            Draw.Sprite(_loadingSprite, new Vector2(_x, _y), _color);
            _renderer.End();
        }
    }
}
