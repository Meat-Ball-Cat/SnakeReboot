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
        void Hide();
    }
    public interface IDrawingByCoordinates<T>
    {
        void Draw(Coordinates coord, T value, object initiator);       
    }
    public interface ISecuredDrawing<T> : IDrawingByCoordinates<T>
    { 
        bool Register(Coordinates O, object initiator, Coordinates[] values);
        void CancelRegistration(object initiator);
    }
    public interface IDrawningRectangle<T> : IRectangle, ISecuredDrawing<T> { }
    public interface IWriter
    {
        void WriteLine(string value);
        int Length { get; }
    }
}
