using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SquareRectangle;

namespace GameLibrary
{
    namespace MenuLibrary
    {
        public class ButtonInConsole : IButton, ILoadRectangle
        {
            public ButtonInConsole(int width, int height, IPrintInRectangle<SignConsole> location)
            {
                Width = width;
                Height = height;
                Location = location;
            }

            public int Width { get; }

            public int Height { get; }
            public IPrintInRectangle<SignConsole> Location { get; }

            public event Action IsPressed;

            public void Close()
            {
                throw new NotImplementedException();
            }

            public void Load()
            {
                throw new NotImplementedException();
            }

            public void Press()
            {
                IsPressed?.Invoke();
            }
        }
    }
}
