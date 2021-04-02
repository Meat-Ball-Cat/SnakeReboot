using System;

namespace SquareRectangle
{
    public struct SignConsole
    {
        public char FirstLetter { get; }
        public char SecondLetter { get; }
        public ConsoleColor BackColor { get; set; }
        public SignConsole(char firstLetter, char secondLetter, ConsoleColor color)
        {
            FirstLetter = firstLetter;
            SecondLetter = secondLetter;
            BackColor = color;
        }
        public SignConsole(char letter, ConsoleColor color) : this(letter, letter, color) { }
    }
}