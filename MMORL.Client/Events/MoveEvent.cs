using Microsoft.Xna.Framework;
using MMORL.Client.Entities;
using System;

namespace MMORL.Client.Events
{
    public class MoveEvent : GameEvent
    {
        private readonly Creature _creature;
        private readonly float _moveTime;
        private readonly float _bounceHeight;
        private readonly float _rotateAmount;

        private readonly int _startX;
        private readonly int _startY;
        private readonly int _newX;
        private readonly int _newY;

        private Vector2 _start;
        private Vector2 _mid;
        private Vector2 _end;

        private float _time;
        private float _targetRotation;

        public MoveEvent(Creature creature, int startX, int startY, int newX, int newY, float moveTime, float bounceHeight, float rotateAmount, float startOffset = 0)
        {
            _creature = creature;
            _moveTime = 1f / moveTime;
            _bounceHeight = bounceHeight;
            _rotateAmount = rotateAmount;
            _startX = startX;
            _startY = startY;
            _newX = newX;
            _newY = newY;
            _time = startOffset;

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

            _targetRotation = _creature.LastRotation > 0 ? -_rotateAmount : _rotateAmount;
            _creature.LastRotation = _targetRotation;
        }

        protected override bool Process(float delta)
        {
            if (_time < _moveTime)
            {
                _time += delta;
                if (_time > _moveTime)
                {
                    _time = _moveTime;
                }

                float normalized = _time / _moveTime;

                if (_rotateAmount != 0)
                {
                    float rotNormalized = 1 - Math.Abs((normalized * 2) - 1);
                    float rot = MathHelper.Lerp(0, _targetRotation, rotNormalized);
                    _creature.Rotation = rot;
                }

                Vector2 m1 = Vector2.Lerp(_start, _mid, normalized);
                Vector2 m2 = Vector2.Lerp(_mid, _end, normalized);
                Vector2 jumpPosition = Vector2.Lerp(m1, m2, normalized);

                Vector2 groundPosition = Vector2.Lerp(_start, _end, normalized);

                _creature.RenderX = jumpPosition.X;
                _creature.RenderY = groundPosition.Y;
                _creature.RenderZ = groundPosition.Y - jumpPosition.Y;
            }

            return _time >= _moveTime;
        }
    }
}
