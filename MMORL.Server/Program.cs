#if !DEBUG
using Sentry;
#endif
using System;

namespace MMORL.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if !DEBUG
            using (SentrySdk.Init(Settings.SentryDsn))
            {
#endif
                const int updateRateMs = 60;

                Game game = new Game();

                try
                {
                    ServerRunner runner = new ServerRunner(game, updateRateMs);
                    runner.Run();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Server exception: {ex.Message}");
                }
#if !DEBUG
            }
#endif
        }
    }
}
