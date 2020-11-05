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

            LoadSprite("panel_tl_a", 0, 0, 16, 24, "interface", assets);
            LoadSprite("panel_tr_a", 1, 0, 16, 24, "interface", assets);
            LoadSprite("panel_bl_a", 2, 0, 16, 24, "interface", assets);
            LoadSprite("panel_br_a", 3, 0, 16, 24, "interface", assets);
            LoadSprite("panel_h_a", 4, 0, 16, 24, "interface", assets);
            LoadSprite("panel_v_a", 5, 0, 16, 24, "interface", assets);

            LoadSprite("panel_tl_b", 0, 1, 16, 24, "interface", assets);
            LoadSprite("panel_tr_b", 1, 1, 16, 24, "interface", assets);
            LoadSprite("panel_bl_b", 2, 1, 16, 24, "interface", assets);
            LoadSprite("panel_br_b", 3, 1, 16, 24, "interface", assets);
            LoadSprite("panel_h_b", 4, 1, 16, 24, "interface", assets);
            LoadSprite("panel_v_b", 5, 1, 16, 24, "interface", assets);

            LoadSprite("panel_tl_c", 0, 2, 16, 24, "interface", assets);
            LoadSprite("panel_tr_c", 1, 2, 16, 24, "interface", assets);
            LoadSprite("panel_bl_c", 2, 2, 16, 24, "interface", assets);
            LoadSprite("panel_br_c", 3, 2, 16, 24, "interface", assets);
            LoadSprite("panel_h_c", 4, 2, 16, 24, "interface", assets);
            LoadSprite("panel_v_c", 5, 2, 16, 24, "interface", assets);

            LoadSprite("panel_close", 6, 2, 16, 24, "interface", assets);
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
