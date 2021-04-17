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
            public SnakeField(int width, int height, IDrawingByCoordinates<GamesSquareValues> location) : base(width, height, location)
            {
                Value = new GamesSquareValues[Width, Height];            
            }
            public GamesSquareValues ReturnCell(Coordinates xy)
            {
                return Value[xy.X, xy.Y];
            }
            private void ChangeCell(Coordinates xy, GamesSquareValues state)
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
                            ChangeCell(new Coordinates(i, j), GamesSquareValues.snakeWall);
                        }
                    }
                }
                Console.SetCursorPosition(0, 0);
            }
            private void DisplayState(Coordinates xy) => Location.Draw(xy, Value[xy.X, xy.Y], this);
            public void AddSnake(Coordinates coord) => ChangeCell(coord, GamesSquareValues.snake);
            public void RemoveSnake(Coordinates coord) => ChangeCell(coord, GamesSquareValues.nothing);
            public void AddBerry(Coordinates coord) => ChangeCell(coord, GamesSquareValues.snakeBerry);

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
                        if (Value[i, j] != GamesSquareValues.nothing)
                        {
                             ChangeCell((i, j), GamesSquareValues.nothing);
                        }                        
                    }
                }
            }
            public override void Hide()
            {
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        if (Value[i, j] != GamesSquareValues.nothing)
                        {
                            Location.Draw((i, j), GamesSquareValues.nothing, this);
                        }
                    }
                }
            }
        }
    }
}
