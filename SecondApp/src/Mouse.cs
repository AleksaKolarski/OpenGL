using OpenTK.Input;

namespace SecondApp
{
    class Mouse
    {
        private static MouseState currentState = OpenTK.Input.Mouse.GetState();
        private static MouseState previousState;

        public static void updateState(int width, int height)
        {
            previousState = currentState;
            currentState = OpenTK.Input.Mouse.GetState();
            OpenTK.Input.Mouse.SetPosition(width, height);
        }

        public static bool checkKey(MouseButton button)
        {
            return (currentState[button]);
        }

        public static int X()
        {
            return currentState.X;
        }
        public static int Y()
        {
            return currentState.Y;
        }

        public static int offsetX()
        {
            return currentState.X - previousState.X;
        }
        public static int offsetY()
        {
            return previousState.Y - currentState.Y;
        }

        public static float scrollOffset()
        {
            return currentState.WheelPrecise - previousState.WheelPrecise;
        }
    }
}
