using Microsoft.Xna.Framework;
using MMORL.Client.Util;
using Toolbox.Graphics;

namespace MMORL.Client.Interface
{
    public class SpriteButton : Button
    {
        private readonly Sprite _idleSprite;
        private readonly Sprite _hoverSprite;

        private Sprite _currentSprite;

        public override int Width => _currentSprite.Width;
        public override int Height => _currentSprite.Height;

        public SpriteButton(int x, int y) : base(x, y)
        {
            _idleSprite = Engine.Assets.GetAsset<Sprite>("panel_close");
            _hoverSprite = _idleSprite;

            _currentSprite = _idleSprite;
        }

        public override void OnHoverEnter()
        {
            _currentSprite = _hoverSprite;
            base.OnHoverEnter();
        }

        public override void OnHoverLeave()
        {
            _currentSprite = _idleSprite;
            base.OnHoverLeave();
        }

        public override void OnButtonRelease()
        {
            _currentSprite = MouseHovering ? _hoverSprite : _idleSprite;
            base.OnButtonRelease();
        }

        public override void Render()
        {
            Draw.Sprite(_currentSprite, new Vector2(X, Y), _currentColor);
        }
    }
}
