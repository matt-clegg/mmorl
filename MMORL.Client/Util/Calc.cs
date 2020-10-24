using Microsoft.Xna.Framework;
using System;

namespace MMORL.Client.Util
{
    public static class Calc
    {
        public static Vector2 Floor(this Vector2 val)
        {
            return new Vector2((int)Math.Floor(val.X), (int)Math.Floor(val.Y));
        }
    }
}
