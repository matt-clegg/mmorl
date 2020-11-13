using MMORL.Server.Actions;
using MMORL.Server.Net;
using MMORL.Shared.Entities;
using MMORL.Shared.Util;

namespace MMORL.Server.Entities
{
    public abstract class ServerEntity : Entity
    {
        public Energy Energy { get; }

        protected GameServer Server { get; }

        public ServerEntity(int id, Race race, GameServer server) : this(id, race.Name, race.Sprite, race.Color, race.Speed, server)
        {

        }

        public ServerEntity(int id, string name, string sprite, GameColor color, int speed, GameServer server) : base(id, name, sprite, color, speed)
        {
            Energy = new Energy();
            Server = server;
        }

        protected abstract BaseAction OnGetAction();

        public override void ProcessTurn()
        {
            if (Energy.CanTakeTurn || Energy.Gain(Speed))
            {
                //if (_actions.Count > 0)
                //{
                //    BaseAction action = _actions.Dequeue();
                //    action.Perform(this, Server);
                //}
                BaseAction action = OnGetAction();
                action?.Perform(this, Server);
                Energy.Spend();
            }
        }
    }
}
