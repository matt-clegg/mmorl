using System.Collections.Generic;
using Toolbox.Assets;
using Toolbox.Graphics;

namespace MMORL.Client.Data
{
    public static class SpriteLoader
    {
        public static void Load(AssetStore<string> assets)
        {
            LoadSprite("player", 0, 0, 16, 24, "monsters", assets);
            LoadSprite("moveIndicator", 15, 9, 16, 24, "terrain", assets);

            LoadSprite("loading", 2, 10, 16, 24, "terrain", assets);
        }

        public static void LoadTiles(List<ushort> tileIds, AssetStore<string> assets)
        {
            foreach (int tileId in tileIds)
            {
                Spritesheet sheet = assets.GetAsset<Spritesheet>("terrain");
                int x = tileId % (sheet.Width / Game.SpriteWidth);
                int y = tileId / (sheet.Width / Game.SpriteWidth);
                string name = $"tile_{tileId}";
                Sprite sprite = sheet.CutSprite(x, y, Game.SpriteWidth, Game.SpriteHeight, name);
                assets.AddAsset(name, sprite);
            }
        }

        private static void LoadSprite(string name, int x, int y, int width, int height, string sheetName, AssetStore<string> assets)
        {
            Spritesheet sheet = assets.GetAsset<Spritesheet>(sheetName);
            Sprite sprite = sheet.CutSprite(x, y, width, height, name);
            assets.AddAsset(name, sprite);
        }
    }
}
