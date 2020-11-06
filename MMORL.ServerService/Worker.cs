using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MMORL.Server;
using Sentry;
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

            using (SentrySdk.Init(Settings.SentryDsn))
            {
                game = new Game(port, chunkSize);
                ServerRunner runner = new ServerRunner(game, updateRateMs);
                await Task.Run(() => runner.Run(), stoppingToken);
                game?.Shutdown();
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
