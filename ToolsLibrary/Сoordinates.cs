using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsLibrary
{
    public struct coord
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static coord operator +(coord x, coord y)
        {
            return new coord(x.x + y.x, x.y + y.y);
        }
        public static coord operator -(coord x, coord y)
        {
            return new coord(x.x - y.x, x.y - y.y);
        }
        public static implicit operator (int x, int y)(coord value)
        {
            return (value.x, value.y);
        }
        public static implicit operator coord((int x, int y) value)
        {
            return new coord(value.x, value.y);
        }
        public coord up() => (x, y - 1);
        public coord down() => (x, y + 1);    
        public coord left() => (x - 1, y);
        public coord right() => (x + 1, y);
    }
}
