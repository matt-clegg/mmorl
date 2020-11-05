using MMORL.Server.Actions;
using MMORL.Server.Entities;
using MMORL.Server.Net;
using MMORL.Shared;
using MMORL.Shared.Entities;
using MMORL.Shared.World;

namespace MMORL.Server.World
{
    public class ServerWorld : GameWorld
    {
        private readonly float _turnTime;
        private float _time;

        private readonly GameServer _server;

        public ServerWorld(Map map, float turnTime, GameServer server) : base(map)
        {
            _turnTime = turnTime;
            _server = server;
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
                    MoveAction action = _server.Pool.Create<MoveAction>();
                    action.X = x;
                    action.Y = y;

                    serverEntity.QueueAction(action);
                    return;
                }
            }
        }

        public void ClearMoves(int entityId)
        {
            foreach (Entity entity in Map.Entities)
            {
                // TODO: Ain't great, fix perhaps?
                if (entity.Id == entityId && entity is ServerEntity serverEntity)
                {
                    serverEntity.ClearMoves();
                    return;
                }
            }
        }
    }
}
