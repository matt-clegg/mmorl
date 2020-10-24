using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Toolbox.Assets;
using Toolbox.Graphics;

namespace MMORL.Client.Data
{
    public static class SpritesheetLoader
    {
        public static void Load(AssetStore<string> assets)
        {
            string path = "Textures";

            LoadSpritesheet(path, "terrain", assets);
            LoadSpritesheet(path, "monsters", assets);
        }

        private static void LoadSpritesheet(string sheetPath, string name, AssetStore<string> assets)
        {
            Texture2D texture = DataLoader.LoadTexture(Path.Combine(sheetPath, name));
            Spritesheet spritesheet = new Spritesheet(name, texture);
            assets.AddAsset(name, spritesheet);
        }
    }
}
