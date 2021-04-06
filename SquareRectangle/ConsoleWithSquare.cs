using System;
using System.Collections.Generic;
using ToolsLibrary;

namespace SquareRectangle
{
    public class ConsoleWithSquare : Rectangle, ISecuredPrinter<SignConsole>
    {
        private static ConsoleWithSquare Value;
        Dictionary<object, Coord> Objects { get; }
        private ConsoleWithSquare() : base(Console.WindowWidth / 2, Console.WindowHeight)
        {
            Value = this;
            Objects = new Dictionary<object, Coord>();
        }
        public static ConsoleWithSquare CreateConsoleWithSquare()
        {
            if(Value == null)
            {
                Value = new ConsoleWithSquare(); 
            }
            return Value;
        }
        public void Print(Coord coord, SignConsole sign, object initiator)
        {
            if (Objects.ContainsKey(initiator))
            {
                coord += Objects[initiator];
                Console.SetCursorPosition(coord.X * 2, coord.Y);
                Console.BackgroundColor = sign.BackColor;
                Console.Write("" + sign.FirstLetter + sign.SecondLetter);
            }
        }

        public bool Registrated(Coord O, object initiator, Coord[] values = null)
        {
            Objects.Add(initiator, O);
            return true;
        }
    }
}
