using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SquareRectangle;
using ToolsLibrary;

namespace GameLibrary
{
    class ConsoleGemeField : IPrintInRectangle<GamesSquareValues>
    {
        public int Width => throw new NotImplementedException();

        public int Height => throw new NotImplementedException();
        public ConsoleGemeField(IPrintInRectangle<SignConsole> location)
        {

        }

        public void Print(Coord coord, GamesSquareValues value)
        {
            throw new NotImplementedException();
        }
    }
}
