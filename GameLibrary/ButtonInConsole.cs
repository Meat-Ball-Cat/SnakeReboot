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
                    var number = i;
                    if (number / Width <= Height)
                    {
                        Location.Print((number % Width, number / Width), Value[i], this);
                    }
                }
            }
            public override void Close()
            {
                Hide();
            }
            public override void Hide()
            {
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        Location.Print((i, j), new SignConsole(' '), this);
                    }
                }
            }
            public void Press()
            {
                IsPressed?.Invoke();
            }
            public void SetColors(ConsoleColor back, ConsoleColor front)
            {
                for(int i = 0; i < Value.Length; i++)
                {
                    Value[i] = Value[i].ChangeColor(back, front);
                }
                Load();
            }
        }
    }
}
