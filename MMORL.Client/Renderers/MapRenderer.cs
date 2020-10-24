using Microsoft.Xna.Framework;
using MMORL.Client.Extensions;
using MMORL.Client.Util;
using MMORL.Shared.Entities;
using MMORL.Shared.World;
using Toolbox.Graphics;

namespace MMORL.Client.Renderers
{
    public class MapRenderer : Renderer
    {
        private readonly Map _map;

        public MapRenderer(Map map, Camera camera) : base(camera)
        {
            _map = map;
        }

        protected override void DoRender()
        {
            foreach (Chunk chunk in _map.Chunks)
            {
                RenderChunk(chunk, true);
            }

            foreach (Entity entity in _map.Entities)
            {
                Sprite sprite = Engine.Assets.GetAsset<Sprite>(entity.Sprite);
                Color color = entity.Color.ParseColor();

                Draw.Sprite(sprite, new Vector2(entity.X * Game.SpriteWidth, entity.Y * Game.SpriteHeight), color);

            }
        }

        private void RenderChunk(Chunk chunk, bool drawDebugBorder = false)
        {
            int chunkWorldX = chunk.X * Game.SpriteWidth * chunk.Size;
            int chunkWorldY = chunk.Y * Game.SpriteHeight * chunk.Size;

            for (int x = 0; x < chunk.Size; x++)
            {
                for (int y = 0; y < chunk.Size; y++)
                {
                    Tile tile = chunk.GetTile(x, y);
                    if (tile != null)
                    {
                        Sprite sprite = Engine.Assets.GetAsset<Sprite>(tile.Sprite);
                        Color color = chunk.GetColor(x, y).ParseColor();

                        int renderX = (x * Game.SpriteWidth) + chunkWorldX;
                        int renderY = (y * Game.SpriteHeight) + chunkWorldY;

                        Draw.Sprite(sprite, new Vector2(renderX, renderY), color);
                    }
                }
            }

            if (drawDebugBorder)
            {
                Draw.HollowRect(chunkWorldX, chunkWorldY, chunk.Size * Game.SpriteWidth, chunk.Size * Game.SpriteHeight, Color.Red);
            }
        }
    }
}
