using Lidgren.Network;
using MMORL.Server.Entities;
using MMORL.Server.Net;
using MMORL.Shared.Net;

namespace MMORL.Server.Actions
{
    public class MoveAction : BaseAction
    {
        public int X { get; set; }
        public int Y { get; set; } 

        public MoveAction() { }

        public MoveAction(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override void Perform(ServerEntity entity, GameServer server)
        {
            entity.Move(X, Y);

            MoveEntityMessage message = server.Pool.Create<MoveEntityMessage>();
            message.Id = entity.Id;
            message.X = (short)entity.X;
            message.Y = (short)entity.Y;
            server.SendMessageToAll(message, NetDeliveryMethod.ReliableUnordered);
            server.Pool.Recycle(message);
        }

        public override void Recycle()
        {
            X = 0;
            Y = 0;
        }
    }
}
