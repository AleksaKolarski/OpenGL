using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondApp
{
    public static class Keyboard
    {
        private static KeyboardState currentState;


        public static void updateState()
        {
            currentState = OpenTK.Input.Keyboard.GetState();
        }

        public static bool checkKey(Key key)
        {
            return (currentState[key]);
        }
    }
}
