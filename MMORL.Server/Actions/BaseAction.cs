using MMORL.Server.Entities;
using MMORL.Server.Net;
using Toolbox;

namespace MMORL.Server.Actions
{
    public abstract class BaseAction
    {
        public abstract void Perform(ServerEntity entity, GameServer server);
    }
}
