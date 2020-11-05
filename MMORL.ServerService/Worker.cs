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
            int port = Settings.Port;
            int chunkSize = Settings.ChunkSize;

            const int updateRateMs = 60;

            Game game = null;

            try
            {
                game = new Game(port, chunkSize);
                ServerRunner runner = new ServerRunner(game, updateRateMs);
                runner.Run();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server exception: {ex.Message}");
            }
            finally
            {
                game?.Shutdown();
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
