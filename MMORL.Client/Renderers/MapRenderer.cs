using Microsoft.Xna.Framework;
using MMORL.Client.Entities;
using MMORL.Client.Extensions;
using MMORL.Client.Util;
using MMORL.Shared.Util;
using MMORL.Shared.World;
using System;
using System.Linq;
using Toolbox;
using Toolbox.Graphics;
using Camera = MMORL.Client.Util.Camera;

namespace MMORL.Client.Renderers
{
    public class MapRenderer : Renderer
    {
        private readonly Map _map;

        private readonly Sprite _tileIndicatorSprite;
        private readonly Color _tileIndicatorColor;

        private readonly Sprite _moveIndicatorSprite;
        private readonly Color _moveIndicatorColor;

        public int MouseX { get; set; }
        public int MouseY { get; set; }

        private bool _creatureWasSelected = false;

        public MapRenderer(Map map, Camera camera) : base(camera)
        {
            _map = map;

            _moveIndicatorSprite = Engine.Assets.GetAsset<Sprite>("moveIndicator");
            _moveIndicatorColor = GameColor.Light.ParseColor() * 0.75f;

            _tileIndicatorSprite = Engine.Assets.GetAsset<Sprite>("tileIndicator");
            _tileIndicatorColor = GameColor.Light.ParseColor() * 0.75f;
        }

        protected override void DoRender()
        {
            Vector2 worldPos = Camera.ScreenToCamera(new Vector2((MouseX - Engine.ViewPaddingX) / Engine.ViewScale, (MouseY - Engine.ViewPaddingY) / Engine.ViewScale));

            int mouseTileX = (int)Math.Floor(worldPos.X / Game.SpriteWidth);
            int mouseTileY = (int)Math.Floor(worldPos.Y / Game.SpriteHeight);

            foreach (Chunk chunk in _map.Chunks)
            {
                RenderChunk(chunk);
            }

            bool creatureSelected = false;

            foreach (Creature creature in _map.Entities.Cast<Creature>())
            {
                bool selected = worldPos.X >= creature.RenderX && worldPos.Y >= creature.RenderY && worldPos.X < creature.RenderX + Game.SpriteWidth && worldPos.Y < creature.RenderY + Game.SpriteHeight;
                creatureSelected |= selected;

                Sprite sprite = Engine.Assets.GetAsset<Sprite>(creature.Sprite);
                Color color = creature.Color.ParseColor();

                // TODO: Move outline code to Draw method
                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        if (x == 0 && y == 0)
                        {
                            continue;
                        }
                        Draw.Sprite(sprite, new Vector2(creature.RenderX + (Game.SpriteWidth / 2) + x, creature.RenderY - creature.RenderZ + y + Game.SpriteHeight), creature.Rotation, selected ? Color.LimeGreen * 0.75f : (Engine.Instance.ClearColor * 0.5f));
                    }
                }

                Draw.Sprite(sprite, new Vector2(creature.RenderX + (Game.SpriteWidth / 2), creature.RenderY - creature.RenderZ + Game.SpriteHeight), creature.Rotation, color);
                //Draw.HollowRect(creature.X * Game.SpriteWidth, creature.Y * Game.SpriteHeight, Game.SpriteWidth, Game.SpriteHeight, Color.Blue);

                if (creature is LocalPlayer player)
                {
                    foreach (Point2D next in player.MovementQueue)
                    {
                        Draw.Sprite(_moveIndicatorSprite, new Vector2(next.X * Game.SpriteWidth, next.Y * Game.SpriteHeight), _moveIndicatorColor);
                    }
                }
            }
            Draw.Sprite(_tileIndicatorSprite, new Vector2(mouseTileX * Game.SpriteWidth, mouseTileY * Game.SpriteHeight), _tileIndicatorColor);

            if (!_creatureWasSelected && creatureSelected)
            {
                Engine.Instance.SetMouseCursor("cursorSelect", 0, 0);
                _creatureWasSelected = true;
            }

            if (_creatureWasSelected && !creatureSelected)
            {
                Engine.Instance.SetMouseCursor("cursorDefault", 0, 0);
                _creatureWasSelected = false;
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
