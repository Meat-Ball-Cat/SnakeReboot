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
    public interface ICoordPrint<T>
    {
        void Print(Coord coord, T value, object initiator);       
    }
    public interface ISecuredPrinter<T> : ICoordPrint<T>
    { 
        bool Registrated(Coord O, object initiator, Coord[] values);
    }
    public static class RectangleTools
    {
        public static bool Intersect(Coord startFirst, IRectangle first, Coord startSecond, IRectangle second)
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

}
