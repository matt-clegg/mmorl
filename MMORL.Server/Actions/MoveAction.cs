using Lidgren.Network;
using MMORL.Server.Net;
using MMORL.Shared.Entities;
using MMORL.Shared.Net;

namespace MMORL.Server.Actions
{
    public class MoveAction : BaseAction
    {
        public int X { get; }
        public int Y { get; }

        public MoveAction(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override void Perform(Entity entity, GameServer server)
        {
            entity.X = X;
            entity.Y = Y;

            MoveEntityMessage message = new MoveEntityMessage(entity.Id, entity.X, entity.Y);
            server.SendMessageToAll(message, NetDeliveryMethod.ReliableUnordered);
        }
    }
}
