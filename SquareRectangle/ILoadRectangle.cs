using ToolsLibrary;

namespace SquareRectangle
{
    public interface IRectangle
    {
        int Width { get; }
        int Height { get; }
    }
    public interface ILoad
    {
        void Load();
        void Close();
    }
    public interface ILoadRectangle : IRectangle, ILoad { }
    public static class RectangleTools
    {
        public static bool Intersect(Coord startFirst, ILoadRectangle first, Coord startSecond, ILoadRectangle second)
        {
            var start = startFirst;
            var end = startFirst + (first.Width, first.Height);
            for (int i = 0; i < second.Width; i++)
            {
                for (int j = 0; j < second.Height; j++)
                {
                    var currentCoord = startSecond + (i, j);
                    if ((start.X <= currentCoord.X && currentCoord.X < end.X) && (start.Y <= currentCoord.Y && currentCoord.Y < end.Y))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
    public interface IPrint<T>
    {
        void Print(Coord coord, T value, ILoadRectangle initiator);       
    }
    public interface IPrintInRectangle<T> : IPrint<T>, ILoadRectangle
    { 
        void AddRectangle(Coord start, ILoadRectangle value);
    }
}
