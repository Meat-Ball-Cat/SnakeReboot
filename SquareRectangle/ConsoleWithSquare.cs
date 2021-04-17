using System;
using System.Collections.Generic;
using ToolsLibrary;

namespace SquareRectangle
{
    public class ConsoleWithSquare : Rectangle, ISecuredDrawing<SignConsole>
    {
        private static ConsoleWithSquare Value;
        Dictionary<object, Coordinates> ObjectInRectangle { get; }
        private ConsoleWithSquare() : base(Console.WindowWidth / 2, Console.WindowHeight)
        {
            Value = this;
            ObjectInRectangle = new Dictionary<object, Coordinates>();
        }
        public static ConsoleWithSquare CreateConsoleWithSquare()
        {
            if(Value == null)
            {
                Value = new ConsoleWithSquare(); 
            }
            return Value;
        }
        public void Draw(Coordinates coord, SignConsole sign, object initiator)
        {
            if (ObjectInRectangle.ContainsKey(initiator))
            {
                coord += ObjectInRectangle[initiator];
                Console.SetCursorPosition(coord.X * 2, coord.Y);
                if (Console.BackgroundColor != sign.BackColor)
                {
                    Console.BackgroundColor = sign.BackColor;
                }
                if (Console.ForegroundColor != sign.FrontColor)
                {
                    Console.ForegroundColor = sign.FrontColor;
                }
                Console.Write("" + sign.FirstLetter + sign.SecondLetter);
            }
        }

        public bool Register(Coordinates O, object initiator, Coordinates[] values = null)
        {
            ObjectInRectangle.Add(initiator, O);
            return true;
        }

        public void CancelRegistration(object initiator)
        {
            if (ObjectInRectangle.ContainsKey(initiator))
            {
                ObjectInRectangle.Remove(initiator);
            }
        }
    }
}
