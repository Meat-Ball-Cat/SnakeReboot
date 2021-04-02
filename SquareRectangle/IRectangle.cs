using ToolsLibrary;

namespace SquareRectangle
{
    public interface IRectangle
    {
        int Width { get; }
        int Height { get; }
        
    }
    public interface IPrint<T>
    {
        void Print(Coord coord, T value);
    }
    public interface IPrintInRectangle<T> : IPrint<T>, IRectangle 
    {
        IPrintInRectangle<T> GetRectangle(Coord start, Coord end);
    }
}
