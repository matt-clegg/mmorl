using Microsoft.Xna.Framework;
using MMORL.Client.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMORL.Client.Renderers
{
    public class UiRenderer : Renderer
    {
        public UiRenderer() : base()
        {
            Matrix = Camera.Matrix;
        }

        protected override void DoRender()
        {
            Draw.Text(Draw.DefaultFont, "hello world", new Vector2(10, 10), Color.White);
        }
    }
}
