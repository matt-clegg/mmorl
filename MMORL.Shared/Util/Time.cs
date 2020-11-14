using System;

namespace MMORL.Shared.Util
{
    public static class Time
    {
        private static readonly DateTime UnixStart = new DateTime(1970, 1, 1, 0, 0, 0);

        public static double TimeInSeconds()
        {
            TimeSpan timeSpan = DateTime.UtcNow - UnixStart;
            return timeSpan.TotalSeconds;
        }
    }
}
