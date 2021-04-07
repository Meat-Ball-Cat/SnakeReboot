using System;
using SquareRectangle;

namespace GameLibrary
{
    namespace MenuLibrary
    {
        public class ButtonInConsole : DrawnRectangle<SignConsole>, IButton
        {
            public ButtonInConsole(int width, int height, ICoordPrint<SignConsole> location, SignConsole[] value) : base(width, height, location)
            {
                Value = value;
            }

            public event Action IsPressed;
            public SignConsole[] Value { get; }

            public override void Load()
            {
                for (int i = 0; i < Value.Length; i++)
                {
                    var number = 1 + i;
                    if (number / Width <= Height)
                    {
                        Location.Print((number % Width, number / Width), Value[i], this);
                    }
                }
            }
            public override void Close()
            {
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        Location.Print((i, j), new SignConsole(' ', ConsoleColor.Black), this);
                    }
                }
            }
            public void Press()
            {
                IsPressed?.Invoke();
            }
        }
    }
}
