using MMORL.Server.Actions;
using MMORL.Server.Entities.Ai;
using MMORL.Server.Net;
using MMORL.Shared.Entities;

namespace MMORL.Server.Entities
{
    public class Mob : ServerEntity
    {
        public MobAi Ai { get; set; }

        public Mob(int id, Race race, GameServer server) : base(id, race, server)
        {
            Type = EntityType.Mob;
        }

        protected override BaseAction OnGetAction()
        {
            return Ai.DecideNextAction();
        }
    }
}
