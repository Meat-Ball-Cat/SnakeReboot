using System;
using System.Collections.Generic;
using System.Linq;
using ToolsLibrary;

namespace SquareRectangle
{
    public class ManagerConsoleSquare : DrawnRectangle<SignConsole>, ISecuredPrinter<SignConsole>
    {
        Dictionary<object, Coord> ObjectInRectangles { get; set; }
        Stack<object>[,] Values { get; }
        public ManagerConsoleSquare(int width, int height, ICoordPrint<SignConsole> location) : base(width, height, location)
        {
            ObjectInRectangles = new Dictionary<object, Coord>();
            Values = new Stack<object>[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Values[i, j] = new Stack<object>();
                }
            }
        }
        public bool Registrated(Coord O, object initiator, Coord[] values)
        {
            bool sucsess = true;
            foreach (var coord in values)
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
                Values[currentCoord.X, currentCoord.Y].Push(initiator);
            }
            return sucsess;
        }
        public void Unregistrated(object value)
        {
            if (ObjectInRectangles.ContainsKey(value))
            {
                var coordStart = ObjectInRectangles[value];
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        Values[i, j] = new Stack<object>(Values[i, j].Where(x => x != value).Reverse());
                    }
                }
                ObjectInRectangles.Remove(value);
            }
        }
        public void Print(Coord coord, SignConsole value, object initiator)
        {
            coord += ObjectInRectangles[initiator];
            if (Values[coord.X, coord.Y].Peek() == initiator)
            {
                Location.Print(coord, value, this);
            }
        }
        public override void Hide()
        {
            foreach (var rectangle in ObjectInRectangles.Keys)
            {
                ((ILoad)rectangle).Hide();
            }
        }
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
    }
}
