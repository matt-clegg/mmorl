﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MMORL.Client.Util;
using System;
using Toolbox;

namespace MMORL.Client.Input
{
    public class MouseManager
    {
        private readonly Camera _camera;

        private int _lastMouseX;
        private int _lastMouseY;

        public int MouseX { get; private set; }
        public int MouseY { get; private set; }

        private MouseState _lastState;

        public event EventHandler<Point2D> MouseDownEvent;
        public event EventHandler<Point2D> MouseReleasedEvent;
        public event EventHandler MouseMovedEvent;
        public event EventHandler MouseStoppedEvent;

        public MouseManager(Camera sceneCamera)
        {
            _camera = sceneCamera;
        }

        public void Update()
        {
            if (!Engine.Instance.IsActive)
            {
                return;
            }

            MouseState mouse = Mouse.GetState();
            MouseX = mouse.X;
            MouseY = mouse.Y;

            if (mouse.LeftButton == ButtonState.Pressed && _lastState.LeftButton == ButtonState.Released)
            {
                MouseDownEvent?.Invoke(this, new Point2D(MouseX, MouseY));
            }

            if (mouse.LeftButton == ButtonState.Released && _lastState.LeftButton == ButtonState.Pressed)
            {
                MouseReleasedEvent?.Invoke(this, new Point2D(MouseX, MouseY));
            }

            if (_lastMouseX != MouseX || _lastMouseY != MouseY)
            {
                MouseMovedEvent?.Invoke(this, EventArgs.Empty);
            }

            if (_lastMouseX == MouseX && _lastMouseY == MouseY)
            {
                MouseStoppedEvent?.Invoke(this, EventArgs.Empty);
            }

            _lastState = mouse;
            _lastMouseX = MouseX;
            _lastMouseY = MouseY;
        }

        public Vector2 GetMouseWorldPosition()
        {
            return _camera.ScreenToCamera(new Vector2((MouseX - Engine.ViewPaddingX) / Engine.ViewScale, (MouseY - Engine.ViewPaddingY) / Engine.ViewScale));
        }

        public Point2D GetMouseTile()
        {
            Vector2 worldPos = GetMouseWorldPosition();
            return GetMouseTile(worldPos);
        }

        public Point2D GetMouseTile(Vector2 worldPos)
        {
            return new Point2D((int)Math.Floor(worldPos.X / Game.SpriteWidth), (int)Math.Floor(worldPos.Y / Game.SpriteHeight));
        }
    }
}
