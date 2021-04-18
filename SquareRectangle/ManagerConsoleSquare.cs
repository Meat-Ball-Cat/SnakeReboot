using System;
using System.Collections.Generic;
using System.Linq;
using ToolsLibrary;

namespace SquareRectangle
{
    public class ManagerConsoleSquare : DrawnRectangle<SignConsole>, ISecuredDrawing<SignConsole>, IDrawningRectangle<SignConsole>
    {
        Dictionary<object,Coordinates> ObjectInRectangles { get; set; }
        Stack<object>[,] ObjectValueOfCordinates { get; }
        public ManagerConsoleSquare(int width, int height, IDrawingByCoordinates<SignConsole> location) : base(width, height, location)
        {
            ObjectInRectangles = new Dictionary<object, Coordinates>();
            ObjectValueOfCordinates = new Stack<object>[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    ObjectValueOfCordinates[i, j] = new Stack<object>();
                }
            }
        }
        public bool Register(Coordinates O, object initiator, Coordinates[] values)
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
                ObjectValueOfCordinates[currentCoord.X, currentCoord.Y].Push(initiator);
            }
            return sucsess;
        }
        public void CancelRegistration(object value)
        {
            if (ObjectInRectangles.ContainsKey(value))
            {
                var coordStart = ObjectInRectangles[value];
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        ObjectValueOfCordinates[i, j] = new Stack<object>(ObjectValueOfCordinates[i, j].Where(x => x != value).Reverse());
                    }
                }
                ObjectInRectangles.Remove(value);
            }
        }
        public void Draw(Coordinates coord, SignConsole value, object initiator)
        {
            coord += ObjectInRectangles[initiator];
            if (ObjectValueOfCordinates[coord.X, coord.Y].Peek() == initiator)
            {
                Location.Draw(coord, value, this);
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
            var visableObject = new List<object>();
            foreach (var obj in ObjectValueOfCordinates)
            {
                if (!visableObject.Contains(obj.Peek()) && obj.Peek() != null)
                {
                    visableObject.Add(obj.Peek());
                }
            }
            foreach(var loadObject in visableObject)
            {
                ((ILoad)loadObject).Load();
            }
        }
    }
}
