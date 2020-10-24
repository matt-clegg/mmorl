using System;
using System.Diagnostics;
using System.Threading;

namespace MMORL.Server
{
    public class ServerRunner
    {
        private readonly Game _game;
        private readonly int _updateRateMs;

        public bool Running { get; private set; }

        public ServerRunner(Game game, int updateRateMs)
        {
            _game = game;
            _updateRateMs = updateRateMs;
        }

        public void Run()
        {
            Running = true;

            float lastTime = NanoTime();

            while (Running)
            {
                float delta = NanoTime() - lastTime;
                lastTime += delta;

                // TODO: Improve this timing loop
                Update(delta / 1000000000f);
                Thread.Sleep(_updateRateMs);
            }
        }

        private void Update(float delta)
        {
            _game.Update(delta);
        }

        private long NanoTime()
        {
            long nano = 10000L * Stopwatch.GetTimestamp();
            nano /= TimeSpan.TicksPerMillisecond;
            nano *= 100L;
            return nano;
        }
    }
}
