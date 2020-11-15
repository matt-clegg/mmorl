using MMORL.Client;
#if !DEBUG
using Sentry;
#endif
using System;

namespace MMORL.OpenGL
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            const int scale = 3;
            const int width = 640;
            const int height = 360;
            const int windowWidth = width * scale;
            const int windowHeight = height * scale;
            const bool fullscreen = false;
            const string title = "MMO RL";

#if !DEBUG
            using (SentrySdk.Init("https://39540b889915409bb2e1725c2523247d@o472714.ingest.sentry.io/5507880"))
            {
                SentrySdk.ConfigureScope(scope =>
                {
                    scope.SetTag("graphics.api", "OpenGL");
                });
#endif
                using (Engine engine = new Engine(width, height, windowWidth, windowHeight, scale, title, fullscreen))
                {
                    engine.Run();
                }
#if !DEBUG
            }
#endif
        }
    }
}
