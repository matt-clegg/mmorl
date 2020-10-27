﻿using Microsoft.Xna.Framework.Graphics;

namespace MMORL.Client.Data
{
    public static class DataLoader
    {
        public static void Load()
        {
            SpritesheetLoader.Load(Engine.Assets);
            SpriteLoader.Load(Engine.Assets);

            SpriteFont large = Engine.Instance.Content.Load<SpriteFont>("Fonts/Large");
            Engine.Assets.AddAsset("large", large);
        }

        public static Texture2D LoadTexture(string path)
        {
            return Engine.Instance.Content.Load<Texture2D>(path);
        }
    }
}
