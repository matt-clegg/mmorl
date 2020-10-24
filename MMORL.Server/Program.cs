using System;

namespace MMORL.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const int port = 25565;
            const int chunkSize = 16;

            const int updateRateMs = 16;

            Game game = new Game(port, chunkSize);

            try
            {
                ServerRunner runner = new ServerRunner(game, updateRateMs);
                runner.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server exception: {ex.Message}");
            }
        }
    }
}
