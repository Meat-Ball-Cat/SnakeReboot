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
        public Coordinates[] GetCoordinates()
        {
            var result = new List<Coordinates>();
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
        protected IDrawingByCoordinates<T> Location { get; }
        public DrawnRectangle(int width, int height, IDrawingByCoordinates<T> location) : base(width, height)
        {
            Location = location ?? throw new ArgumentNullException();
        }
        public abstract void Load();
        public abstract void Close();
        public abstract void Hide();
        public void Fill(T value)
        {
            var coords = GetCoordinates();
            foreach(var coord in coords)
            {
                Location.Draw(coord, value, this);
            }
        }
    }
    public class Frame<T> : ILoad
    {
        Coordinates[] Coord{ get; }
        int Width { get; set; }
        int Height { get; set; }
        T Value { get; set; }
        IDrawingByCoordinates<T> Location { get; }
        public Frame(int width, int height, T value, IDrawingByCoordinates<T> location)
        {
            Width = width;
            Height = height;
            Value = value;
            Location = location;
            Coord = new Coordinates[Width * 2 + Height * 2 - 4];
            var t = 0;
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (i == 0 || i == Width - 1 || j == 0 || j == Height - 1)
                    {
                        Coord[t++] = (i, j);
                    }
                }
            }
        }
        public Coordinates[] GetCoordinates()
        {
            return (Coordinates[])Coord.Clone();
        }

        public void Load()
        {
            foreach(var coord in Coord)
            {
                Location.Draw(coord, Value, this);
            }
        }

        public void Close()
        {
            Hide();
        }

        public void Hide()
        {
            foreach (var coord in Coord)
            {
                Location.Draw(coord, default, this);
            }
        }
    }
    public abstract class DrawingRectangle<T, R> : DrawnRectangle<T>, ISecuredDrawing<R>, IDrawningRectangle<R>
    {
        public DrawingRectangle(int width, int height, IDrawingByCoordinates<T> location) : base(width, height, location) 
        {
            ObjectInRectangles = new Dictionary<object, Coordinates>();
            ObjectValueOfCordinates = new object[Width, Height];
        }
        protected Dictionary<object, Coordinates> ObjectInRectangles { get; set; }
        protected object[,] ObjectValueOfCordinates { get; }
        public override void Close()
        {
            if (ObjectInRectangles != null)
            {
                foreach (var rectangle in ObjectInRectangles.Keys)
                {
                    ((ILoad)rectangle).Close();
                }
                ObjectInRectangles = null;
            }
        }
        public override void Load()
        {
            foreach (var rectangle in ObjectInRectangles.Keys)
            {
                ((ILoad)rectangle).Load();
            }
        }
        public override void Hide()
        {
            foreach (var rectangle in ObjectInRectangles.Keys)
            {
                ((ILoad)rectangle).Hide();
            }
        }
        public virtual void Draw(Coordinates coord, R value, object initiator)
        {
            coord += ObjectInRectangles[initiator];
            if(ObjectValueOfCordinates[coord.X, coord.Y] == initiator)
            Location.Draw(coord, Convert(value), this);
        }

        protected abstract T Convert(R value);

        public virtual bool Register(Coordinates O, object initiator, Coordinates[] initiatorCordinates)
        {
            bool success = true;
            foreach(var coord in initiatorCordinates)
            {
                var currentCoord = coord + O;
                if (currentCoord.X < 0 || Width <= currentCoord.X || currentCoord.Y < 0 || Height <= currentCoord.Y)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            ObjectInRectangles.Add(initiator, O);
            foreach (var coord in initiatorCordinates)
            {
                var currentCoord = coord + O;
                if (ObjectValueOfCordinates[currentCoord.X, currentCoord.Y] == null)
                {
                    ObjectValueOfCordinates[currentCoord.X, currentCoord.Y] = initiator;
                }
                else
                {
                    success = false;
                }
            }
            return success;
        }
        public void Frame(R value)
        {
            var frame = new Frame<R>(Width, Height, value, (IDrawingByCoordinates<R>)this);
            Register((0, 0), frame, frame.GetCoordinates());

        }
        public virtual void CancelRegistration(object initiator)
        {
            for(int i = 0; i < Width; i++)
            {
                for(int j = 0; j < Height; j++)
                {
                    if(ObjectValueOfCordinates[i, j] == initiator)
                    {
                        ObjectValueOfCordinates[i, j] = null;
                    }
                }
            }
        }
    }
    public class DrawingRectangle<T> : DrawingRectangle<T, T>
    {
        public DrawingRectangle(int width, int height, IDrawingByCoordinates<T> location) : base(width, height, location) { }

        protected override T Convert(T value) => value;
    }

}
