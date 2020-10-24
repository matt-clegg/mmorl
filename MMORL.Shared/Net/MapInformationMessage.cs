using Lidgren.Network;
using MMORL.Shared.World;
using System.Collections.Generic;
using Toolbox;

namespace MMORL.Shared.Net
{
    public class MapInformationMessage : IMessage
    {
        public MessageType Type => MessageType.MapInformation;

        public List<Tile> Tiles { get; private set; }

        public MapInformationMessage() { }

        public MapInformationMessage(List<Tile> tiles)
        {
            Tiles = tiles;
        }

        public void Read(NetIncomingMessage message)
        {
            int tileCount = message.ReadInt32();
            Tiles = new List<Tile>(tileCount);

            for (int i = 0; i < tileCount; i++)
            {
                ushort id = message.ReadUInt16();
                bool isSolid = message.ReadBoolean();
                bool isTransparent = message.ReadBoolean();
                Tiles.Add(new Tile(id, isSolid, isTransparent));
            }
        }

        public void Write(NetOutgoingMessage message)
        {
            message.Write(Tiles.Count);
            foreach (Tile tile in Tiles)
            {
                message.Write(tile.Id);
                message.Write(tile.IsSolid);
                message.Write(tile.IsTransparent);
            }
        }
    }
}
