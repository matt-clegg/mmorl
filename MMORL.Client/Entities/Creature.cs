using MMORL.Client.Events;
using MMORL.Shared.Entities;
using MMORL.Shared.Util;
using System.Collections.Generic;

namespace MMORL.Client.Entities
{
    public class Creature : Entity
    {
        public float RenderX { get; set; }
        public float RenderY { get; set; }
        public float RenderZ { get; set; }

        private readonly Queue<GameEvent> _queuedEvents = new Queue<GameEvent>();
        private GameEvent _currentEvent;

        public Creature(int id, string name, string sprite, GameColor color, int speed) : base(id, name, sprite, color, speed)
        {
        }

        public override void Update(float delta)
        {
            if (_queuedEvents.Count > 0)
            {
                while (_currentEvent == null && _queuedEvents.Count > 0)
                {
                    _currentEvent = _queuedEvents.Dequeue();
                    if (!_currentEvent.CanStart())
                    {
                        _currentEvent = null;
                    }
                }
            }

            if (_currentEvent != null)
            {
                if (_currentEvent.Update(delta))
                {
                    _currentEvent = null;
                }
            }

            base.Update(delta);
        }

        public override void Move(int x, int y)
        {
            MoveEvent moveEvent = new MoveEvent(this, X, Y, x, y, 1f, 0.3f);
            _queuedEvents.Enqueue(moveEvent);
            base.Move(x, y);
        }
    }
}
