using System;

namespace SquareRectangle
{
    public struct SignConsole
    {
        public char FirstLetter { get; }
        public char SecondLetter { get; }
        public ConsoleColor BackColor { get; set; }
        public SignConsole(char firstLetter, char secondLetter, ConsoleColor color = ConsoleColor.Black)
        {
            FirstLetter = firstLetter;
            SecondLetter = secondLetter;
            BackColor = color;
        }
        public SignConsole(char letter, ConsoleColor color = ConsoleColor.Black) : this(letter, letter, color) { }
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
    }
}