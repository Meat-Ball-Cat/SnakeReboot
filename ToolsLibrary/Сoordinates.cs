using System;
namespace ToolsLibrary
{
    public struct Coordinates : IEquatable<Coordinates>
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Coordinates(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public static Coordinates operator +(Coordinates x, Coordinates y)
        {
            return new Coordinates(x.X + y.X, x.Y + y.Y);
        }
        public static Coordinates operator -(Coordinates x, Coordinates y)
        {
            return new Coordinates(x.X - y.X, x.Y - y.Y);
        }
        public static implicit operator (int x, int y)(Coordinates value)
        {
            return (value.X, value.Y);
        }
        public static implicit operator Coordinates((int x, int y) value)
        {
            return new Coordinates(value.x, value.y);
        }
        public override string ToString()
        {
            return $"x={X} y={Y}";
        }
        public Coordinates Up() => (X, Y - 1);
        public Coordinates Down() => (X, Y + 1);    
        public Coordinates Left() => (X - 1, Y);
        public Coordinates Right() => (X + 1, Y);
        public bool Equals(Coordinates other)
        {
            return X == other.X && Y == other.Y;
        }
    }
}
