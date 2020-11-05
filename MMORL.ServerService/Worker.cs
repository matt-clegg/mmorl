using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MMORL.Server;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MMORL.ServerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
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

                game.Shutdown();

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
