using System;
using System.Collections.Generic;
using ToolsLibrary;

namespace SquareRectangle
{
    public class Rectangle : IRectangle
    {
        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }
        public int Width { get; }
        public int Height { get; }
        public Coord[] GetCoord()
        {
            var result = new List<Coord>();
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    result.Add((i, j));
                }
            }
            return result.ToArray();
        }
    }
    public abstract class DrawnRectangle<T> : Rectangle, ILoad
    {
        protected ICoordPrint<T> Location { get; }
        public DrawnRectangle(int width, int height, ICoordPrint<T> location) : base(width, height)
        {
            Location = location ?? throw new ArgumentNullException();
        }
        public abstract void Load();
        public abstract void Close();      
    }
    public abstract class DrawingRectangle<T, R> : DrawnRectangle<T>, ISecuredPrinter<R>
    {
        public DrawingRectangle(int width, int height, ICoordPrint<T> location) : base(width, height, location) 
        {
            ObjectInRectangles = new Dictionary<object, Coord>();
            Values = new object[Width, Height];
        }
        protected Dictionary<object, Coord> ObjectInRectangles { get; set; }
        protected object[,] Values { get; }
        public override void Close()
        {
            foreach (var rectangle in ObjectInRectangles.Keys)
            {
                ((ILoad)rectangle).Close();
            }
            ObjectInRectangles = null;
        }
        public override void Load()
        {
            foreach (var rectangle in ObjectInRectangles.Keys)
            {
                ((ILoad)rectangle).Load();
            }
        }
        public virtual void Print(Coord coord, R value, object initiator)
        {
            coord += ObjectInRectangles[initiator];
            if(Values[coord.X, coord.Y] == initiator)
            Location.Print(coord, Convert(value), this);
        }
        protected abstract T Convert(R value);

        public bool Registrated(Coord O, object initiator, Coord[] values)
        {
            bool sucsess = true;
            foreach(var coord in values)
            {
                var currentCoord = coord + O;
                if (currentCoord.X < 0 || Width <= currentCoord.X || currentCoord.Y < 0 || Height <= currentCoord.Y)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            ObjectInRectangles.Add(initiator, O);
            foreach (var coord in values)
            {
                var currentCoord = coord + O;
                if (Values[currentCoord.X, currentCoord.Y] == null)
                {
                    Values[currentCoord.X, currentCoord.Y] = initiator;
                }
                else
                {
                    sucsess = false;
                }
            }
            return sucsess;
        }
    }
    public class DrawingRectangle<T> : DrawingRectangle<T, T>
    {
        public DrawingRectangle(int width, int height, ICoordPrint<T> location) : base(width, height, location) { }

        protected override T Convert(T value) => value;
    }
    public interface IWriter
    {
        void WriteLine(string value);
        int Length { get; }
    }
}
