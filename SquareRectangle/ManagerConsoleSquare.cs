using System;
using System.Collections.Generic;
using System.Linq;
using ToolsLibrary;

namespace SquareRectangle
{
    public class ManagerConsoleSquare : IPrintInRectangle<SignConsole>
    {
        public int Width { get; }
        public int Height { get; }
        public IPrintInRectangle<SignConsole> Location { get; }
        Dictionary<ILoadRectangle, Coord> Rectangles { get; set; }
        Stack<ILoadRectangle>[,] Values { get; }
        public ManagerConsoleSquare(int width, int height, IPrintInRectangle<SignConsole> location)
        {
            Width = width;
            Height = height;
            Location = location;
            Rectangles = new Dictionary<ILoadRectangle, Coord>();
            Values = new Stack<ILoadRectangle>[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Values[i, j] = new Stack<ILoadRectangle>();
                }
            }
        }
        public void AddRectangle(Coord start, ILoadRectangle value)
        {
            if (start.X + value.Width <= Width && start.Y + value.Height <= Height)
            {
                Rectangles.Add(value, start);
                for (int i = 0; i < value.Width; i++)
                {
                    for (int j = 0; j < value.Height; j++)
                    {
                        Values[i = start.X, j + start.Y].Push(value);
                    }
                }
            }
            else
            {
                throw new Exception("Невозможные координаты вписываемого прямоугольника");
            }
        }
        public void RemoveLastRectangle(ILoadRectangle value)
        {
            if (Rectangles.ContainsKey(value))
            {
                var coordStart = Rectangles[value];
                for (int i = coordStart.X; i < coordStart.X + value.Width; i++)
                {
                    for (int j = coordStart.Y; j < coordStart.Y + value.Height; j++)
                    {
                        Values[i, j] = new Stack<ILoadRectangle>(Values[i, j].Where( x => x != value));
                    }
                }
                Rectangles.Remove(value);                
            }
        }
        public void Print(Coord coord, SignConsole value, ILoadRectangle initiator)
        {
            coord += Rectangles[initiator];
            if(Values[coord.X, coord.Y].Peek() == initiator)
            {
                Location.Print(coord, value, this);
            }
        }

        public void Load()
        {
            foreach (var rectangle in Rectangles.Keys)
            {
                rectangle.Load();
            }
        }

        public void Close()
        {
            foreach (var rectangle in Rectangles.Keys)
            {
                rectangle.Close();
            }
            Rectangles = null;
        }
    }
}
