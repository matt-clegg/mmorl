using Lidgren.Network;
using Toolbox;

namespace MMORL.Shared.Extensions
{
    public static class Point2DExtensions
    {
        public static void Write(this Point2D point, NetOutgoingMessage message)
        {
            message.Write(point.X);
            message.Write(point.Y);
        }
    }
}
