using Lidgren.Network;
using MMORL.Shared.Entities;
using MMORL.Shared.Util;

namespace MMORL.Shared.Net
{
    public class SpawnEntityMessage : IMessage
    {
        public MessageType Type => MessageType.SpawnEntity;

        public EntityType EntityType { get; private set; }
        public int EntityId { get; private set; }
        public string Name { get; private set; }
        public string Sprite { get; private set; }
        public GameColor Color { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public SpawnEntityMessage() { }

        public SpawnEntityMessage(Entity entity, int x, int y, EntityType entityType)
            : this(entity.Id, entity.Name, entity.Sprite, entity.Color, x, y, entityType)
        {

        }

        public SpawnEntityMessage(int entityId, string name, string sprite, GameColor color, int x, int y, EntityType entityType)
        {
            EntityId = entityId;
            Name = name;
            Sprite = sprite;
            Color = color;
            X = x;
            Y = y;
            EntityType = entityType;
        }

        public void Read(NetIncomingMessage message)
        {
            EntityId = message.ReadInt32();
            EntityType = (EntityType)message.ReadByte();
            Sprite = message.ReadString();
            Color = (GameColor)message.ReadByte();
            X = message.ReadInt32();
            Y = message.ReadInt32();
        }

        public void Write(NetOutgoingMessage message)
        {
            byte type = (byte)EntityType;

            message.Write(EntityId);
            message.Write(type);
            message.Write(Sprite);
            message.Write((byte)Color);
            message.Write(X);
            message.Write(Y);
        }
    }
}
