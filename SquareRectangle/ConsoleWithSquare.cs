using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary;

namespace SquareRectangle
{
    public class ConsoleWithSquare : IPrintInRectangle<SignConsole>
    {
        private static ConsoleWithSquare Value;
        public int Width { get; }
        public int Height { get; }
        Dictionary<ILoadRectangle, Coord> Rectangles;
        private ConsoleWithSquare()
        {
            Width = Console.WindowWidth / 2;
            Height = Console.WindowHeight;
            Value = this;
            Rectangles = new Dictionary<ILoadRectangle, Coord>();
        }
        public static ConsoleWithSquare CreateConsoleWithSquare()
        {
            if(Value == null)
            {
                Value = new ConsoleWithSquare(); 
            }
            return Value;
        }
        public void AddRectangle(Coord start, ILoadRectangle value)
        {
            if(start.X + value.Width <= Width && start.Y + value.Height <= Height)
            {
                Rectangles.Add(value, start);
            }
            else
            {
                throw new Exception("Невозможные координаты вписываемого прямоугольника");
            }            
        }
        public void Print(Coord coord, SignConsole sign, ILoadRectangle initiator)
        {
            if (Rectangles.ContainsKey(initiator))
            {
                coord += Rectangles[initiator];
                Console.SetCursorPosition(coord.X * 2, coord.Y);
                Console.BackgroundColor = sign.BackColor;
                Console.Write("" + sign.FirstLetter + sign.SecondLetter);
            }
        }

        public void Load()
        {
            foreach(var rectangle in Rectangles.Keys)
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
            Console.Clear();
        }
    }
}
