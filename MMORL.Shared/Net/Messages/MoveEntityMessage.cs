using Lidgren.Network;
using Toolbox;

namespace MMORL.Shared.Net
{
    public class MoveEntityMessage : Poolable, IMessage
    {
        public MessageType Type => MessageType.MoveEntity;

        public int Id { get; set; }
        public short X { get; set; }
        public short Y { get; set; }

        public MoveEntityMessage() { }

        public MoveEntityMessage(int id, int x, int y)
        {
            Id = id;
            X = (short)x;
            Y = (short)y;
        }

        public void Read(NetIncomingMessage message)
        {
            Id = message.ReadInt32();
            X = message.ReadInt16();
            Y = message.ReadInt16();
        }

        public void Write(NetOutgoingMessage message)
        {
            message.Write(Id);
            message.Write(X);
            message.Write(Y);
        }

        public override void Recycle()
        {
            Id = 0;
            X = 0;
            Y = 0;
        }
    }
}
