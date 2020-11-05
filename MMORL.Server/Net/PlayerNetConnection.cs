using Lidgren.Network;
using MMORL.Server.Entities;

namespace MMORL.Server.Net
{
    public class PlayerNetConnection
    {
        public NetConnection NetConnection { get; }
        public ServerEntity Player { get; }

        public PlayerNetConnection(NetConnection netConnection, ServerEntity player)
        {
            NetConnection = netConnection;
            Player = player;
        }
    }
}
