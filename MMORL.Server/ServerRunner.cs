using System;
using System.Diagnostics;
using System.Threading;

namespace MMORL.Server
{
    public class ServerRunner
    {
        private readonly Game _game;
        private readonly int _updatesPerSecond;

        public bool Running { get; private set; }

        private static readonly DateTime UnixStart = new DateTime(1970, 1, 1, 0, 0, 0);

        public ServerRunner(Game game, int updatesPerSecond)
        {
            _game = game;
            _updatesPerSecond = updatesPerSecond;
        }

        public void Run()
        {
            Running = true;

            double time = 0.0;
            double delta = 1 / (double)_updatesPerSecond;

            double currentTime = TimeInSeconds();
            double accumulator = 0.0;

            while (Running)
            {
                double newTime = TimeInSeconds();
                double frameTime = newTime - currentTime;
                currentTime = newTime;

                accumulator += frameTime;

                while (accumulator >= delta)
                {
                    Update((float)delta);
                    accumulator -= delta;
                    time += delta;
                }

                Thread.Sleep(1);
            }
        }

        private void Update(float delta)
        {
            _game.Update(delta);
        }

        private double TimeInSeconds()
        {
            TimeSpan timeSpan = DateTime.UtcNow - UnixStart;
            return timeSpan.TotalSeconds;
        }
    }
}
