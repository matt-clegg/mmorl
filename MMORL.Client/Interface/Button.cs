using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace MMORL.Client.Interface
{
    public abstract class Button : UiElement
    {
        public virtual int Width { get; protected set; }
        public virtual int Height { get; protected set; }

        protected Color _idleColor = Color.White;
        protected Color _hoverColor = Color.LightGray;
        protected Color _pressColor = Color.Gray;

        protected Color _currentColor;

        public bool MouseHovering { get; private set; }
        public bool ButtonPressed { get; private set; }

        private MouseState _lastState;

        public event EventHandler ButtonClickedEvent;

        public Button(int x, int y)
        {
            X = x;
            Y = y;
            MouseHovering = false;

            _currentColor = _idleColor;
        }

        public override void Update(float delta)
        {
            MouseState currentState = Mouse.GetState();

            if (Intersects(currentState.X, currentState.Y))
            {
                if (!MouseHovering && !ButtonPressed)
                {
                    OnHoverEnter();
                }
                MouseHovering = true;
            }
            else
            {
                if (MouseHovering)
                {
                    if (!ButtonPressed)
                    {
                        OnHoverLeave();
                    }
                    MouseHovering = false;
                }
            }

            if (currentState.LeftButton == ButtonState.Pressed && _lastState.LeftButton == ButtonState.Released)
            {
                if (MouseHovering)
                {
                    OnButtonPress();
                    ButtonPressed = true;
                }
            }

            if (currentState.LeftButton == ButtonState.Released && _lastState.LeftButton == ButtonState.Pressed)
            {
                OnButtonRelease();
                ButtonPressed = false;

                if (MouseHovering)
                {
                    ButtonClickedEvent?.Invoke(this, EventArgs.Empty);
                }
            }

            _lastState = currentState;
        }

        public virtual void OnHoverEnter()
        {
            _currentColor = _hoverColor;
        }

        public virtual void OnHoverLeave()
        {
            _currentColor = _idleColor;
        }

        public virtual void OnButtonPress()
        {
            _currentColor = _pressColor;
        }

        public virtual void OnButtonRelease()
        {
            _currentColor = MouseHovering ? _hoverColor : _idleColor;
        }

        protected virtual bool Intersects(int x, int y)
        {
            return x >= X && y >= Y && x < X + Width && y < Y + Height;
        }
    }
}
