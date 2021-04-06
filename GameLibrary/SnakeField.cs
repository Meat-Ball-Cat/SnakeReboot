using SquareRectangle;
using System;
using ToolsLibrary;

namespace GameLibrary
{
    namespace SnakeGame
    {
        public class SnakeField : DrawnRectangle<GamesSquareValues>
        {
            GamesSquareValues[,] Value { get; }
            public SnakeField(int width, int height, ICoordPrint<GamesSquareValues> location) : base(width, height, location)
            {
                Value = new GamesSquareValues[Width, Height];            
            }
            public GamesSquareValues ReturnCell(Coord xy)
            {
                return Value[xy.X, xy.Y];
            }
            private void ChangeCell(Coord xy, GamesSquareValues state)
            {
                Value[xy.X, xy.Y] = state;
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
            private void DisplayState(Coord xy) => Location.Print(xy, Value[xy.X, xy.Y], this);
            public void AddSnake(Coord coord) => ChangeCell(coord, GamesSquareValues.snake);
            public void RemoveSnake(Coord coord) => ChangeCell(coord, GamesSquareValues.nothing);
            public void AddBerry(Coord coord) => ChangeCell(coord, GamesSquareValues.snakeBerry);

            public override void Load()
            {
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        DisplayState((i, j));
                    }
                }
            }

            public override void Close()
            {
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        ChangeCell((i, j), GamesSquareValues.nothing);
                    }
                }
            }
        }
    }
}
