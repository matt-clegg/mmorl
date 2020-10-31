using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MMORL.Client.Renderers;
using Toolbox.Graphics;

namespace MMORL.Client.Util
{
    public static class Draw
    {
        private static Rectangle _rect = new Rectangle();

        public static Renderer Renderer { get; internal set; }
        public static SpriteBatch SpriteBatch { get; private set; }
        public static SpriteFont DefaultFont { get; private set; }

        public static Texture2D Pixel { get; set; }

        public static int SpriteDraws = 0;

        public static void UseDebugPixelTexture(GraphicsDevice graphicsDevice)
        {
            Pixel = new Texture2D(graphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });
        }

        public static void Sprite(Sprite sprite, Vector2 position, Color color, SpriteEffects spriteEffect = SpriteEffects.None)
        {
            SpriteBatch.Draw(sprite.Texture, position, sprite.Bounds, color, 0, Vector2.Zero, 1f, spriteEffect, 0);
            SpriteDraws++;
        }

        public static void Sprite(Sprite sprite, Rectangle rectangle, Color color, SpriteEffects spriteEffect = SpriteEffects.None)
        {
            SpriteBatch.Draw(sprite.Texture, rectangle, sprite.Bounds, color, 0, Vector2.Zero, spriteEffect, 0);
        }

        public static void HollowRect(float x, float y, float width, float height, Color color)
        {
            _rect.X = (int)x;
            _rect.Y = (int)y;
            _rect.Width = (int)width;
            _rect.Height = 1;

            SpriteBatch.Draw(Pixel, _rect, Pixel.Bounds, color);

            _rect.Y += (int)height - 1;

            SpriteBatch.Draw(Pixel, _rect, Pixel.Bounds, color);

            _rect.Y -= (int)height - 1;
            _rect.Width = 1;
            _rect.Height = (int)height;

            SpriteBatch.Draw(Pixel, _rect, Pixel.Bounds, color);

            _rect.X += (int)width - 1;

            SpriteBatch.Draw(Pixel, _rect, Pixel.Bounds, color);
        }

        public static void HollowRect(Vector2 position, float width, float height, Color color)
        {
            HollowRect(position.X, position.Y, width, height, color);
        }

        public static void HollowRect(Rectangle rect, Color color)
        {
            HollowRect(rect.X, rect.Y, rect.Width, rect.Height, color);
        }

        public static void Rect(float x, float y, float width, float height, Color color)
        {
            _rect.X = (int)x;
            _rect.Y = (int)y;
            _rect.Width = (int)width;
            _rect.Height = (int)height;
            SpriteBatch.Draw(Pixel, _rect, Pixel.Bounds, color);
        }

        public static void Rect(Vector2 position, float width, float height, Color color)
        {
            Rect(position.X, position.Y, width, height, color);
        }

        public static void Rect(Rectangle rect, Color color)
        {
            _rect = rect;
            SpriteBatch.Draw(Pixel, _rect, Pixel.Bounds, color);
        }

        public static void Text(SpriteFont font, string text, Vector2 position, Color color)
        {
            SpriteBatch.DrawString(font, text, Calc.Floor(new Vector2(position.X + 1, position.Y + 1)), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
            SpriteBatch.DrawString(font, text, Calc.Floor(position), color, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }

        public static void TextCentered(SpriteFont font, string text, Vector2 position)
        {
            Text(font, text, position - (font.MeasureString(text) * 0.5f), Color.White);
        }

        public static void TextCentered(SpriteFont font, string text, Vector2 position, Color color)
        {
            Vector2 dif = font.MeasureString(text);

            Text(font, text, position - (font.MeasureString(text) * 0.5f), color);
        }

        internal static void Initialize(GraphicsDevice graphicsDevice)
        {
            SpriteBatch = new SpriteBatch(graphicsDevice);
            DefaultFont = Engine.Instance.Content.Load<SpriteFont>("Fonts/Default");
            UseDebugPixelTexture(graphicsDevice);
        }
    }
}
