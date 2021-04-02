using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary;

namespace SquareRectangle
{
    class ConsoleWithSquare : IPrintInRectangle<SignConsole>
    {
        public int Width => throw new NotImplementedException();

        public int Height => throw new NotImplementedException();

        public IPrintInRectangle<SignConsole> GetRectangle(Coord start, Coord end)
        {
            throw new NotImplementedException();
        }

        public void Print(Coord coord, SignConsole value)
        {
            throw new NotImplementedException();
        }
    }
}
