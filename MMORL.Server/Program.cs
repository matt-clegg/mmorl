using System;

namespace MMORL.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int port = Settings.Port;
            int chunkSize = Settings.ChunkSize;

            if (chunkSize < 8 || chunkSize > 128)
            {
                throw new InvalidOperationException($"Invalid chunk size: {chunkSize}. Value must be between 8 and 128.");
            }

            const int updateRateMs = 60;

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

            Console.ReadLine();
        }
    }
}
