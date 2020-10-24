using MMORL.Server.Actions;
using MMORL.Server.Net;
using MMORL.Shared.Entities;
using MMORL.Shared.Util;
using System.Collections.Generic;

namespace MMORL.Server.Entities
{
    public class ServerEntity : Entity
    {
        public Energy Energy { get; }

        private readonly Queue<BaseAction> _actions = new Queue<BaseAction>();

        private readonly GameServer _server;

        public ServerEntity(int id, string name, string sprite, GameColor color, int speed, GameServer server) : base(id, name, sprite, color, speed)
        {
            Energy = new Energy();
            _server = server;
        }

        public override void ProcessTurn()
        {
            if (Energy.CanTakeTurn || Energy.Gain(Speed))
            {
                if (_actions.Count > 0)
                {
                    _actions.Dequeue().Perform(this, _server);
                }

                Energy.Spend();
            }

        }

        public void QueueAction(BaseAction action)
        {
            _actions.Enqueue(action);
        }
    }
}
