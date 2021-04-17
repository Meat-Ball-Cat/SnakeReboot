using System;
using SquareRectangle;
using System.Linq;

namespace GameLibrary
{
    namespace MenuLibrary
    {
        public class ButtonInConsole : DrawnRectangle<SignConsole>, IButton
        {
            public ButtonInConsole(int width, int height, IDrawingByCoordinates<SignConsole> location, SignConsole[] value) : base(width, height, location)
            {
                Name = value;
            }

            public event Action IsPressed;
            public SignConsole[] Name { get; }

            public override void Load()
            {
                for (int i = 0; i < Name.Length; i++)
                {
                    var number = i;
                    if (number / Width <= Height)
                    {
                        Location.Draw((number % Width, number / Width), Name[i], this);
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
                        Location.Draw((i, j), new SignConsole(' '), this);
                    }
                }
            }
            public void Press()
            {
                IsPressed?.Invoke();
            }
            public void SetColors(ConsoleColor back, ConsoleColor front)
            {
                for(int i = 0; i < Name.Length; i++)
                {
                    Name[i] = Name[i].ChangeColor(back, front);
                }
                Load();
            }
        }
        public class ButtonInConsoleSetter : ButtonInConsole
        {
            public ButtonInConsoleSetter(int width, int height, IDrawingByCoordinates<SignConsole> location, SignConsole[] name) : base(width, height, location, name) 
            {
                Name = name;
            }

            SignConsole[] _name;
            public new SignConsole[] Name 
            {
                get 
                {
                   return _name;
                }
                private set
                {
                    if (NameLength < value.Length)
                    {
                        _name = value.Reverse().Skip(Name.Length - NameLength).Reverse().ToArray();
                    }
                    else
                    {
                        _name = value;
                    }
                }
            }
            private int NameLength { get { return Width / 2 + Width % 2; } }
            private int ValueLength { get { return Width / 2; } }
            SignConsole[] _value;
            public SignConsole[] Value
            {
                get
                {
                    return _value;
                }
                set
                {
                    if (ValueLength < value.Length)
                    {
                        _value = value.Reverse().Skip(Name.Length - NameLength).Reverse().ToArray();                       
                    }
                    else
                    {
                        _value = value;
                    }
                    Hide();
                    Load();
                }
            }
            public override void Close()
            {
                Hide();
                base.Close();
            }

            public override void Hide()
            {
                for(int i = 0; i < Width; i++)
                {
                    Location.Draw((i, 0), new SignConsole(' '), this);
                }
            }

            public override void Load()
            {
                for(int i = 0; i < Math.Min(NameLength, Name.Length); i++)
                {
                    Location.Draw((i, 0), Name[i], this);
                }
                if (Value != null)
                {
                    for (int i = 0; i < Math.Min(ValueLength, Value.Length); i++)
                    {
                        Location.Draw((i + NameLength, 0), Value[i], this);
                    }
                }
            }
            public new void SetColors(ConsoleColor back, ConsoleColor front)
            {
                for (int i = 0; i < Name.Length; i++)
                {
                    Name[i] = Name[i].ChangeColor(back, front);
                }
                Load();
            }
        }
    }
}
