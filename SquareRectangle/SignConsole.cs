using System;

namespace SquareRectangle
{
    public struct SignConsole
    {
        public char FirstLetter { get; }
        public char SecondLetter { get; }
        public ConsoleColor BackColor { get; }
        public ConsoleColor FrontColor { get; }
        public SignConsole(char firstLetter, char secondLetter, ConsoleColor backColor = ConsoleColor.Black, ConsoleColor frontColor = ConsoleColor.White)
        {
            FirstLetter = firstLetter;
            SecondLetter = secondLetter;
            BackColor = backColor;
            FrontColor = frontColor;
        }
        public SignConsole(char letter, ConsoleColor color = ConsoleColor.Black, ConsoleColor frontColor = ConsoleColor.White) : this(letter, letter, color, frontColor) { }
        public static SignConsole[] GetSignConsoles(string value)
        {
            if(value.Length % 2 == 1)
            {
                value += " ";
            }
            var result = new SignConsole[value.Length / 2];
            for(int i = 0; i < value.Length; i++)
            {
                result[i / 2] = new SignConsole(value[i], value[++i], ConsoleColor.Black);
            }
            return result;
        }
        public SignConsole ChangeColor(ConsoleColor back, ConsoleColor front)
        {
            return new SignConsole(FirstLetter, SecondLetter, back, front);
        }
    }
}