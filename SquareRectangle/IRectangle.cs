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
        void Print(Coord coord, T value, IRectangle initiator);
    }
    public interface IPrintInRectangle<T> : IPrint<T>, IRectangle
    { 
        void AddRectangle(Coord start, IRectangle value);
    }
}
