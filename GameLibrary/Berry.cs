using System;
using ToolsLibrary;

namespace GameLibrary
{
    namespace SnakeGame
    {
        public class Berry
        {
            public static void RandomBerry(SnakeField location)
            {
                var rnd = new Random();
                Coord newCoord;
                while (true)
                {
                    newCoord = new Coord(rnd.Next(location.Width), rnd.Next(location.Height));
                    if (location.ReturnCell(newCoord) == GamesSquareValues.nothing)
                    {
                        location.AddBerry(newCoord);
                        break;
                    }
                }
            }
        }
    }
}
