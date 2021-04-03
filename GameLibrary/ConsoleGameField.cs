using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SquareRectangle;
using ToolsLibrary;

namespace GameLibrary
{
    public class ConsoleGameField : IPrintInRectangle<GamesSquareValues>
    {
        public int Width { get; }
        public int Height { get; }
        IPrintInRectangle<SignConsole> Location { get; }

        Dictionary<IRectangle, Coord> Rectangles;

        Dictionary<GamesSquareValues, SignConsole> SquareValue;
        public ConsoleGameField(int width, int height, IPrintInRectangle<SignConsole> locatoin, Dictionary<GamesSquareValues, SignConsole> squareValue)
        {
            Width = width;
            Height = height;
            Location = locatoin;
            SquareValue = squareValue;
            Rectangles = new Dictionary<IRectangle, Coord>();
        }
        public void AddRectangle(Coord start, IRectangle value)
        {
            if (start.X + value.Width <= Width && start.Y + value.Height <= Height)
            {
                Rectangles.Add(value, start);
            }
            else
            {
                throw new Exception("Невозможные координаты вписываемого прямоугольника");
            }
        }

        public void Print(Coord coord, GamesSquareValues value, IRectangle initiator)
        {
            Location.Print(coord + Rectangles[initiator], SquareValue[value], this);
        }
    }
}
