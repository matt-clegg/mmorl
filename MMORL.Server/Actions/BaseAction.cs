using MMORL.Server.Net;
using MMORL.Shared.Entities;

namespace MMORL.Server.Actions
{
    public abstract class BaseAction
    {
        public abstract void Perform(Entity entity, GameServer server);
    }
}
