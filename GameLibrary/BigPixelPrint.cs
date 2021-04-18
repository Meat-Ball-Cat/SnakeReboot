using SquareRectangle;
using System;
using ToolsLibrary;

namespace GameLibrary
{
    public abstract class ConsoleWriter : DrawnRectangle<SignConsole>, IWriter
    {
        public abstract int Length { get; }

        public ConsoleWriter(int width, int height, IDrawingByCoordinates<SignConsole> location) : base(width, height, location) { }
        public abstract void WriteLine(string value);
    }
    public class BigPixelPrint : ConsoleWriter
    {
        string LastValue { get; set; }
        Letters Library { get; }
        public override int Length { get; }
        public BigPixelPrint(int width, int height, IDrawingByCoordinates<SignConsole> location, Letters library) : base(width, height, location)
        {
            if(Height < 5 || Width < 5)
            {
                throw new Exception("неверные размеры формы");
            }
            Library = library;
            Length = Width / 6;
        }
        void PrintChar(char value, int position)
        {
            Coordinates coord = (position * 6, 0);
            for(int i = 0; i < 5; i++)
            {
                var charValue = Library[value];
                for (int j = 0; j < 5; j++)
                {
                    Location.Draw(coord + (1 + i, j), charValue[i + j * 5] ? new SignConsole(' ', ConsoleColor.White) : new SignConsole(' '), this);
                }
            }
        }
        public override void WriteLine(string str)
        {
            str = str.ToUpper();
            LastValue = str;
            for(int i = 0; i < Math.Min(str.Length, Length); i++)
            {
                PrintChar(str[i], i);
            }
        }

        public override void Load() 
        {
            if(LastValue != null)
            {
                WriteLine(LastValue);
            }
        }

        public override void Close()
        {
            var nullValue = new string(' ', Length);
            WriteLine(nullValue);
        }

        public override void Hide()
        {
            var nullValue = new string(' ', Length);
            WriteLine(nullValue);
        }
    }
}
