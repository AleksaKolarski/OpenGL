using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondApp
{
    class Program
    {
        static Game game;
        static void Main(string[] args)
        {
            game = new Game(1600, 900, "igra");

            game.VSync = OpenTK.VSyncMode.On;
            game.WindowState = OpenTK.WindowState.Normal;
            game.CursorVisible = false;
            game.Run();
        }
    }
}
