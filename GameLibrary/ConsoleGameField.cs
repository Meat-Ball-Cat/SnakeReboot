using System.Collections.Generic;
using SquareRectangle;

namespace GameLibrary
{
    public class ConsoleGameField : DrawingRectangle<SignConsole, GamesSquareValues>
    {

        Dictionary<GamesSquareValues, SignConsole> SquareValue;
        public ConsoleGameField(int width, int height, IDrawingByCoordinates<SignConsole> location, Dictionary<GamesSquareValues, SignConsole> squareValue) 
            : base(width, height, location)
        {
            SquareValue = squareValue;
        }
        protected override SignConsole Convert(GamesSquareValues value) => SquareValue[value];
    }
}
