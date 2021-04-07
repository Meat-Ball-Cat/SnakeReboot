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
        public interface IButton
        {
            event Action IsPressed;
            void Press();
        }
    }
}
