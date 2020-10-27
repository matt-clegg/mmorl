using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MMORL.Client.Util;
using MMORL.Shared;
using MMORL.Shared.Entities;
using static MMORL.Client.Extensions.NumberExtensions;

namespace MMORL.Client.Renderers
{
    public class UiRenderer : Renderer
    {
        private readonly NetPeerStatistics _statistics;
        private readonly GameWorld _gameWorld;
        private readonly Camera _gameCamera;

        private readonly SpriteFont _large;

        public UiRenderer(NetPeerStatistics statistics, GameWorld gameWorld, Camera gameCamera) : base()
        {
            Matrix = Camera.Matrix;
            _statistics = statistics;
            _gameWorld = gameWorld;
            _gameCamera = gameCamera;

            _large = Engine.Assets.GetAsset<SpriteFont>("large");
        }

        protected override void DoRender()
        {
            //Draw.Text(Draw.DefaultFont, "hello world", new Vector2(10, 10), Color.White);
            int y = 3;
            Draw.Text(_large, $"Sent: {_statistics.SentPackets}", new Vector2(5, y += 14), Color.White);
            Draw.Text(_large, $"Received: {_statistics.ReceivedPackets}", new Vector2(5, y += 14), Color.White);
            Draw.Text(_large, $"Sent: {_statistics.SentBytes.ToSize(SizeUnits.KB)}kb", new Vector2(5, y += 14), Color.White);
            Draw.Text(_large, $"Received: {_statistics.ReceivedBytes.ToSize(SizeUnits.KB)}kb", new Vector2(5, y += 14), Color.White);

            Draw.Text(_large, $"Recycle Pool: {_statistics.BytesInRecyclePool.ToSize(SizeUnits.KB)}kb", new Vector2(5, y += 14), Color.White);
            Draw.Text(_large, $"Storage Bytes: {_statistics.StorageBytesAllocated.ToSize(SizeUnits.KB)}kb", new Vector2(5, y += 14), Color.White);

            foreach (Entity entity in _gameWorld.Map.Entities)
            {
                if (!string.IsNullOrWhiteSpace(entity.Name))
                {
                    Vector2 position = _gameCamera.CameraToScreen(new Vector2(entity.X * Game.SpriteWidth, entity.Y * Game.SpriteHeight)) * Engine.ViewScale;
                    Vector2 offset = new Vector2(Game.SpriteWidth / 2, -3) * Engine.ViewScale;

                    Draw.TextCentered(_large, entity.Name, position + offset, Color.White);
                    //Draw.HollowRect(position, 10, 10, Color.White);
                }
            }

        }

    }
}
