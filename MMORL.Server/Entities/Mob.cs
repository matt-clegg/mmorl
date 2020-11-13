using MMORL.Server.Actions;
using MMORL.Server.Net;
using MMORL.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMORL.Server.Entities
{
    public class Mob : ServerEntity
    {
        public Mob(int id, Race race, GameServer server) : base(id, race, server)
        {
            Type = EntityType.Mob;
        }

        protected override BaseAction OnGetAction()
        {
            // TODO: Return Ai.DecideNextAction();
            return null;
        }
    }
}
