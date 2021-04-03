using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary;

namespace SquareRectangle
{
    public class ManagerConsoleSquare : IPrintInRectangle<SignConsole>
    {
        public int Width { get; }
        public int Height { get; }
        public IPrintInRectangle<SignConsole> Location { get; }
        public ManagerConsoleSquare(int width, int height, IPrintInRectangle<SignConsole> location)
        {
            Width = width;
            Height = height;
            Location = location;
        }

        

        public void AddRectangle(Coord start, IRectangle value)
        {
            throw new NotImplementedException();
        }

        public void Print(Coord coord, SignConsole value, IRectangle initiator)
        {
            throw new NotImplementedException();
        }
    }
}
