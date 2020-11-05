using Microsoft.Xna.Framework;
using MMORL.Client.Util;

namespace MMORL.Client.Interface
{
    public class TextButton : Button
    {
        public string Text { get; set; }

        public override int Width => (int)Draw.DefaultFont.MeasureString(Text).X;
        public override int Height => (int)Draw.DefaultFont.MeasureString(Text).Y;


        private readonly Panel _panel;

        public TextButton(int x, int y, string text) : base(x, y)
        {
            Text = text;

            int panelX = X;
            int panelY = Y;

            _panel = new Panel(panelX, panelY, Width + Game.SpriteWidth * 2, Height + Game.SpriteHeight * 2 - (Game.SpriteHeight / 2), "c");
        }

        public override void OnHoverEnter()
        {
            _panel.BorderColor = _hoverColor;
            base.OnHoverEnter();
        }

        public override void OnHoverLeave()
        {
            _panel.BorderColor = _idleColor;
            base.OnHoverLeave();
        }

        public override void OnButtonPress()
        {
            _panel.BorderColor = _pressColor;
            base.OnButtonPress();
        }

        public override void OnButtonRelease()
        {
            _panel.BorderColor = MouseHovering ? _hoverColor : _idleColor;
            base.OnButtonRelease();
        }

        public override void Render()
        {
            _panel.Render();
            Draw.Text(Draw.DefaultFont, Text, new Vector2(X + Game.SpriteWidth, Y + Game.SpriteHeight - (Game.SpriteHeight / 4)), _currentColor);
        }

        protected override bool Intersects(int x, int y)
        {
            return _panel.Intersects(x, y, (Game.SpriteWidth / 2) - 2, (Game.SpriteHeight / 2) - 2);
        }
    }
}
