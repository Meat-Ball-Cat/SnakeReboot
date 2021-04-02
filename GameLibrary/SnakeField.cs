using SquareRectangle;
using System;
using ToolsLibrary;

namespace GameLibrary
{
    namespace SnakeGame
    {
        public class SnakeField
        {
            GamesSquareValues[,] value { get; }
            public int Width { get; }
            public int Height { get; }
            IPrintInRectangle<GamesSquareValues> Location { get; }
            Coord start { get; }
            public SnakeField(Coord start, Coord end, IPrintInRectangle<GamesSquareValues> locatoin)
            {
                Width = end.X - start.X;
                Height = end.Y - start.Y;
                value = new GamesSquareValues[Width, Height];
                Location = locatoin;
                this.start = start;
                if (Width > Location.Width || Height > Location.Height)
                {
                    throw new Exception("Размер поля больше возможного отображения");
                }
                Inicializated();
            }
            public GamesSquareValues ReturnCell(Coord xy)
            {
                return value[xy.X, xy.Y];
            }
            private void ChangeCell(Coord xy, GamesSquareValues state)
            {
                value[xy.X, xy.Y] = state;
                DisplayState(xy);
            }
            public void Inicializated()
            {
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        if (i == 0 || i == Width - 1 || j == 0 || j == Height - 1)
                        {
                            ChangeCell(new Coord(i, j), GamesSquareValues.snakeWall);
                        }
                    }
                }
                Console.SetCursorPosition(0, 0);
            }
            public void DisplayAll()
            {
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        DisplayState((i, j));
                    }
                }
            }
            private void DisplayState(Coord xy)
            {

                var thisValue = value[xy.X, xy.Y];
                xy += start;
                Location.Print(xy, thisValue);                
            }
            public void AddSnake(Coord coord) => ChangeCell(coord, GamesSquareValues.snake);
            public void RemoveSnake(Coord coord) => ChangeCell(coord, GamesSquareValues.nothing);
            public void AddBerry(Coord coord) => ChangeCell(coord, GamesSquareValues.snakeBerry);
        }
    }
}
