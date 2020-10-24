using Microsoft.Xna.Framework.Input;
using MMORL.Client.Renderers;
using MMORL.Client.Util;
using System.Collections.Generic;
using System.Linq;

namespace MMORL.Client.Scenes
{
    public abstract class Scene
    {
        private readonly List<Renderer> _renderers = new List<Renderer>();

        protected Camera Camera { get; }

        public Scene()
        {
            Camera = new Camera();
            Camera.Zoom = 1f;
        }

        public abstract void Input(Keys key);

        public virtual void Update(float delta)
        {
            foreach (Renderer renderer in _renderers)
            {
                renderer.Update(delta);
            }
        }

        public virtual void Render()
        {
            foreach (Renderer renderer in _renderers.Where(r => r.IsVisible))
            {
                renderer.Render();
            }
        }

        protected void Add(Renderer renderer)
        {
            _renderers.Add(renderer);
        }

        public virtual void Unload()
        {

        }
    }
}
