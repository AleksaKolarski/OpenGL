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
            game = new Game(800, 600, "igra");

            game.Run(60.0);
        }
    }
}
