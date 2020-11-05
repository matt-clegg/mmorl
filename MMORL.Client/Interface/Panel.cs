using Microsoft.Xna.Framework;
using MMORL.Client.Util;
using Toolbox.Graphics;

namespace MMORL.Client.Interface
{
    public class Panel : UiElement
    {
        private readonly Sprite _topLeft;
        private readonly Sprite _topRight;
        private readonly Sprite _bottomLeft;
        private readonly Sprite _bottomRight;
        private readonly Sprite _horizontal;
        private readonly Sprite _vertical;

        public int Width { get; set; }
        public int Height { get; set; }

        public Color BorderColor { get; set; }
        public Color BackgroundColor { get; set; }

        public Panel(int x, int y, int width, int height, string type)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;

            _topLeft = Engine.Assets.GetAsset<Sprite>($"panel_tl_{type}");
            _topRight = Engine.Assets.GetAsset<Sprite>($"panel_tr_{type}");
            _bottomLeft = Engine.Assets.GetAsset<Sprite>($"panel_bl_{type}");
            _bottomRight = Engine.Assets.GetAsset<Sprite>($"panel_br_{type}");
            _horizontal = Engine.Assets.GetAsset<Sprite>($"panel_h_{type}");
            _vertical = Engine.Assets.GetAsset<Sprite>($"panel_v_{type}");

            BorderColor = Color.White;
            BackgroundColor = Engine.Instance.ClearColor;
        }

        public override void Update(float delta)
        {

        }

        public override void Render()
        {
            Vector2 position = new Vector2(X, Y);

            const int halfWidth = Game.SpriteWidth / 2;
            const int halfHeight = Game.SpriteHeight / 2;

            Draw.Rect(X + halfWidth - 2, Y + halfHeight - 2, Width - Game.SpriteWidth + 4, Height - Game.SpriteHeight + 4, BackgroundColor);

            Draw.Sprite(_topLeft, position, BorderColor);
            Draw.Sprite(_topRight, position + new Vector2(Width - Game.SpriteWidth, 0), BorderColor);
            Draw.Sprite(_bottomLeft, position + new Vector2(0, Height - Game.SpriteHeight), BorderColor);
            Draw.Sprite(_bottomRight, position + new Vector2(Width - Game.SpriteWidth, Height - Game.SpriteHeight), BorderColor);

            Draw.Sprite(_horizontal, new Rectangle(X + Game.SpriteWidth, Y, Width - Game.SpriteWidth * 2, Game.SpriteHeight), BorderColor);
            Draw.Sprite(_horizontal, new Rectangle(X + Game.SpriteWidth, Y + Height - Game.SpriteHeight, Width - Game.SpriteWidth * 2, Game.SpriteHeight), BorderColor);

            Draw.Sprite(_vertical, new Rectangle(X, Y + Game.SpriteHeight, Game.SpriteWidth, Height - Game.SpriteHeight * 2), BorderColor);
            Draw.Sprite(_vertical, new Rectangle(X + Width - Game.SpriteWidth, Y + Game.SpriteHeight, Game.SpriteWidth, Height - Game.SpriteHeight * 2), BorderColor);
        }

        public bool Intersects(int x, int y, int paddingX = 0, int paddingY = 0)
        {
            return x >= X + paddingX && y >= Y + paddingY && x < X + Width - paddingX && y < Y + Height - paddingY;
        }
    }
}

