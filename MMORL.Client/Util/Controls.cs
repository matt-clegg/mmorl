using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MMORL.Client.Util
{
    public static class Controls
    {
        public static readonly Control North = new Control().Bind(Keys.Up).Bind(Keys.NumPad8).Bind(Keys.W);
        public static readonly Control South = new Control().Bind(Keys.Down).Bind(Keys.NumPad2).Bind(Keys.S);
        public static readonly Control East = new Control().Bind(Keys.Right).Bind(Keys.NumPad6).Bind(Keys.D);
        public static readonly Control West = new Control().Bind(Keys.Left).Bind(Keys.NumPad4).Bind(Keys.A);
        public static readonly Control NorthEast = new Control().Bind(Keys.NumPad9);
        public static readonly Control NorthWest = new Control().Bind(Keys.NumPad7);
        public static readonly Control SouthEast = new Control().Bind(Keys.NumPad3);
        public static readonly Control SouthWest = new Control().Bind(Keys.NumPad1);

        public static readonly Control Rest = new Control().Bind(Keys.Space);

        public static readonly Control SelectItemDown = new Control().Bind(Keys.Q);
        public static readonly Control SelectItemUp = new Control().Bind(Keys.E);

        public static readonly Control Restart = new Control().Bind(Keys.R);

        public static readonly Control Zero = new Control().Bind(Keys.D0);
        public static readonly Control One = new Control().Bind(Keys.D1);
        public static readonly Control Two = new Control().Bind(Keys.D2);
        public static readonly Control Three = new Control().Bind(Keys.D3);
        public static readonly Control Four = new Control().Bind(Keys.D4);
        public static readonly Control Five = new Control().Bind(Keys.D5);
        public static readonly Control Six = new Control().Bind(Keys.D6);
        public static readonly Control Seven = new Control().Bind(Keys.D7);
        public static readonly Control Eight = new Control().Bind(Keys.D8);
        public static readonly Control Nine = new Control().Bind(Keys.D9);

        public static readonly Control Escape = new Control().Bind(Keys.Escape);
        public static readonly Control F11 = new Control().Bind(Keys.F11);
    }

    public class Control
    {
        private readonly HashSet<Keys> _keys = new HashSet<Keys>();

        public bool IsPressed(Keys key) => _keys.Contains(key);

        public Control() { }

        public Control Bind(Keys key)
        {
            _keys.Add(key);
            return this;
        }
    }
}
