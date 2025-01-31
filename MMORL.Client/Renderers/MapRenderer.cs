﻿using Microsoft.Xna.Framework;
using MMORL.Client.Entities;
using MMORL.Client.Extensions;
using MMORL.Client.Util;
using MMORL.Shared.Util;
using MMORL.Shared.World;
using System.Linq;
using Toolbox;
using Toolbox.Graphics;
using Camera = MMORL.Client.Util.Camera;

namespace MMORL.Client.Renderers
{
    public class MapRenderer : Renderer
    {
        private readonly Map _map;

        private readonly Sprite _moveIndicatorSprite;
        private readonly Color _moveIndicatorColor;

        public MapRenderer(Map map, Camera camera) : base(camera)
        {
            _map = map;

            _moveIndicatorSprite = Engine.Assets.GetAsset<Sprite>("moveIndicator");
            _moveIndicatorColor = GameColor.Light.ParseColor() * 0.75f;
        }

        protected override void DoRender()
        {
            foreach (Chunk chunk in _map.Chunks)
            {
                RenderChunk(chunk);
            }

            foreach (Creature creature in _map.Entities.Cast<Creature>())
            {
                Sprite sprite = Engine.Assets.GetAsset<Sprite>(creature.Sprite);
                Color color = creature.Color.ParseColor();

                Draw.Sprite(sprite, new Vector2(creature.RenderX, creature.RenderY - creature.RenderZ), color);
                //Draw.HollowRect(creature.X * Game.SpriteWidth, creature.Y * Game.SpriteHeight, Game.SpriteWidth, Game.SpriteHeight, Color.Blue);

                if (creature is LocalPlayer player)
                {
                    foreach(Point2D next in player.MovementQueue)
                    {
                        Draw.Sprite(_moveIndicatorSprite, new Vector2(next.X * Game.SpriteWidth, next.Y * Game.SpriteHeight), _moveIndicatorColor);
                    }
                }
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
