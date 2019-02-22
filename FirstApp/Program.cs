using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace FirstApp
{
    class Program
    {
        static Game game;
        public static void Main()
        {
            game = new Game(800, 600, "igra");

            game.Run(60.0);
        }
    }
}
