using MMORL.Server.Actions;
using MMORL.Server.Entities;
using MMORL.Shared;
using MMORL.Shared.Entities;
using MMORL.Shared.World;
using System;

namespace MMORL.Server.World
{
    public class ServerWorld : GameWorld
    {
        private readonly float _turnTime;
        private float _time;

        public ServerWorld(Map map, float turnTime) : base(map)
        {
            _turnTime = turnTime;
        }

        public override void Update(float delta)
        {
            _time += delta;

            if (_time >= _turnTime)
            {
                _time -= _time;
                DoTurn();
            }

            base.Update(delta);
        }

        private void DoTurn()
        {
            foreach (Entity entity in Map.Entities)
            {
                entity.ProcessTurn();
            }
        }

        public void QueueMovement(int entityId, int x, int y)
        {
            foreach (Entity entity in Map.Entities)
            {
                // TODO: Ain't great, fix perhaps?
                if (entity.Id == entityId && entity is ServerEntity serverEntity)
                {
                    serverEntity.QueueAction(new MoveAction(x, y));
                }
            }
        }
    }
}
