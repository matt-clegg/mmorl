using MMORL.Client;
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

            using (Engine engine = new Engine(width, height, windowWidth, windowHeight, scale, title, fullscreen))
            {
                engine.Run();
            }
        }
    }
}
