namespace ToolsLibrary
{
    public struct Coord
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Coord(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public static Coord operator +(Coord x, Coord y)
        {
            return new Coord(x.X + y.X, x.Y + y.Y);
        }
        public static Coord operator -(Coord x, Coord y)
        {
            return new Coord(x.X - y.X, x.Y - y.Y);
        }
        public static implicit operator (int x, int y)(Coord value)
        {
            return (value.X, value.Y);
        }
        public static implicit operator Coord((int x, int y) value)
        {
            return new Coord(value.x, value.y);
        }
        public override string ToString()
        {
            return $"x={X} y={Y}";
        }
        public Coord Up() => (X, Y - 1);
        public Coord Down() => (X, Y + 1);    
        public Coord Left() => (X - 1, Y);
        public Coord Right() => (X + 1, Y);
    }
}
