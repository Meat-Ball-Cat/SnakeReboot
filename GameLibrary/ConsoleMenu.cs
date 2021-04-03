using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SquareRectangle;
using ToolsLibrary;

namespace GameLibrary
{
    namespace MenuLibrary
    {
        class ConsoleMenu : Menu<ButtonInConsole>, IPrintInRectangle<SignConsole>
        {
            public int Width { get; }
            public int Height { get; }
            public IPrintInRectangle<SignConsole> Location { get; }
            Dictionary<ILoadRectangle, Coord> Rectangles { get; set; }
            public ConsoleMenu(string name, IPrintInRectangle<SignConsole> location, bool curcule = false) : base(name, curcule)
            {
                Location = location;
            }
            public void AddRectangle(Coord start, ILoadRectangle value)
            {
                if (start.X + value.Width <= Width && start.Y + value.Height <= Height)
                {
                    foreach(var rectangle in Rectangles)
                    {
                        if(RectangleTools.Intersect(rectangle.Value, rectangle.Key, start, value))
                        {
                            break;
                        }
                    }
                    Rectangles.Add(value, start);
                }
                else
                {
                    throw new Exception("Невозможные координаты вписываемого прямоугольника");
                }
            }
            public void AddButton(Coord start, ButtonInConsole value)
            {
                AddRectangle(start, value);
                Buttons.AddLast(value);
            }

            public void Close()
            {
                foreach (var button in Buttons)
                {
                    Buttons.Remove(button);
                }
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        Location.Print((i, j), new SignConsole(' ', ConsoleColor.Black), this);
                    }
                }
            }

            public void Load()
            {
                var coord = (Coord)(0, 0);
                foreach(var sign in SignConsole.GetSignConsoles(Name))
                {
                    if (coord.X < Width && coord.Y < Height)
                    {
                        Location.Print(coord, sign, this);
                        coord = coord.Right();
                    }
                    else { break; }
                }
            }

            public void Print(Coord coord, SignConsole value, ILoadRectangle initiator)
            {
                Location.Print(coord + Rectangles[initiator], value, this);
            }
        }
    }
}
