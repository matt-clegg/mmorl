﻿using Microsoft.Xna.Framework;
using MMORL.Client.Entities;

namespace MMORL.Client.Events
{
    public class MoveEvent : GameEvent
    {
        private readonly Creature _creature;
        private readonly float _moveTime;
        private readonly float _bounceHeight;

        private readonly int _startX;
        private readonly int _startY;
        private readonly int _newX;
        private readonly int _newY;

        private Vector2 _start;
        private Vector2 _mid;
        private Vector2 _end;

        private float _time;

        public MoveEvent(Creature creature, int startX, int startY, int newX, int newY, float moveTime, float bounceHeight) : base(true)
        {
            _creature = creature;
            _moveTime = 1f / moveTime;
            _bounceHeight = bounceHeight;
            _startX = startX;
            _startY = startY;
            _newX = newX;
            _newY = newY;

            Id = creature.Id;
        }

        public override bool CanStart()
        {
            // Rudimentary lag compensation.
            // Check if the current creature position does not match the target move position.
            // If the position does not match, cancel this action.
            return _newX == _creature.X && _newY == _creature.Y;
        }

        protected override void OnStart()
        {
            _start = new Vector2(_startX * Game.SpriteWidth, _startY * Game.SpriteHeight);
            _end = new Vector2(_newX * Game.SpriteWidth, _newY * Game.SpriteHeight);
            _mid = _start + (_end - _start) / 2 + new Vector2(0, -(Game.SpriteHeight * _bounceHeight));
        }

        protected override bool Process(float delta)
        {
            if (_time < 1f)
            {
                _time += _moveTime * delta;
                if (_time > 1)
                {
                    _time = 1f;
                }

                Vector2 m1 = Vector2.Lerp(_start, _mid, _time);
                Vector2 m2 = Vector2.Lerp(_mid, _end, _time);
                Vector2 jumpPosition = Vector2.Lerp(m1, m2, _time);

                Vector2 groundPosition = Vector2.Lerp(_start, _end, _time);

                _creature.RenderX = jumpPosition.X;
                _creature.RenderY = groundPosition.Y;
                _creature.RenderZ = groundPosition.Y - jumpPosition.Y;
            }

            return _time >= 1f;
        }
    }
}
