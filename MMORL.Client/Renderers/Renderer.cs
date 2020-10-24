using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MMORL.Client.Util;

namespace MMORL.Client.Renderers
{
    public abstract class Renderer
    {
        public BlendState BlendState { get; set; }
        public SamplerState SamplerState { get; set; }
        public DepthStencilState DepthStencilState { get; set; }
        public RasterizerState RasterizerState { get; set; }
        public SpriteSortMode SortMode { get; set; }

        public Effect Effect { get; set; }
        public Camera Camera { get; set; }

        public Matrix? Matrix { get; set; }

        public bool IsVisible { get; set; }

        public int BoundsX => (int)(Camera.X - Engine.Width / 2);
        public int BoundsY => (int)(Camera.Y - Engine.Height / 2);
        public int BoundsWidth => (int)Camera.Viewport.Width;
        public int BoundsHeight => (int)Camera.Viewport.Height;

        public Renderer(Camera camera)
        {
            BlendState = BlendState.AlphaBlend;
            SamplerState = SamplerState.PointClamp;
            DepthStencilState = DepthStencilState.None;
            RasterizerState = RasterizerState.CullNone;
            SortMode = SpriteSortMode.Immediate;
            Camera = camera;

            IsVisible = true;
        }

        protected abstract void DoRender();

        public virtual void Update(float delta) { }

        public void Begin()
        {
            Draw.SpriteBatch.Begin(SortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, Matrix ?? Camera.Matrix * Engine.ScreenMatrix);
        }

        public void End()
        {
            Draw.SpriteBatch.End();
        }

        public void Render()
        {
            Begin();
            DoRender();
            End();
        }
    }
}
