using System;
using System.Collections.Generic;
using ToolsLibrary;

namespace SquareRectangle
{
    public class Rectangle<T> : IPrintInRectangle<T>
    {
        public Rectangle(int width, int height, IPrintInRectangle<T> location)
        {
            Width = width;
            Height = height;
            Location = location;
        }

        public int Width { get; }

        public int Height { get; }
        public IPrintInRectangle<T> Location { get; }
        Dictionary<ILoadRectangle, Coord> Rectangles { get; set; }

        public virtual void AddRectangle(Coord start, ILoadRectangle value)
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

        public virtual void Close()
        {
            foreach (var rectangle in Rectangles.Keys)
            {
                rectangle.Close();
            }
            Rectangles = null;
        }

        public virtual void Load()
        {
            foreach (var rectangle in Rectangles.Keys)
            {
                rectangle.Load();
            }
        }

        public virtual void Print(Coord coord, T value, ILoadRectangle initiator)
        {
            Location.Print(coord + Rectangles[initiator], value, this);
        }
    }
}
