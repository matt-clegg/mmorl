using MMORL.Shared.Util;
using System;
using System.Threading;

namespace MMORL.Server
{
    public class ServerRunner
    {
        private readonly Game _game;
        private readonly int _updatesPerSecond;

        public bool Running { get; private set; }


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

            double currentTime = Time.TimeInSeconds();
            double accumulator = 0.0;

            while (Running)
            {
                double newTime = Time.TimeInSeconds();
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

            _game.Shutdown();
        }

        public void Stop()
        {
            Running = false;
            _game.Shutdown();
        }

        private void Update(float delta)
        {
            _game.Update(delta);
        }


    }
}
