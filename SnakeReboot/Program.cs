using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLibrary;
using SquareRectangle;
using TolsLibrary;
using GameLibrary.MenuLibrary;

namespace SnakeReboot
{
    class Program
    {
        static void Main(string[] args)
        {
            ToolsLibrary.FullScreen.FullScreenOn();
            var square = ConsoleWithSquare.CreateConsoleWithSquare();
            KeyPress.Start();

            
            var x = ConsoleWithSquare.CreateConsoleWithSquare();
            var d = new Dictionary<GamesSquareValues, SignConsole>();
            d.Add(GamesSquareValues.snake, new SignConsole(' ', ConsoleColor.White));
            d.Add(GamesSquareValues.snakeBerry, new SignConsole(' ', ConsoleColor.Red));
            d.Add(GamesSquareValues.snakeWall, new SignConsole('X', ConsoleColor.Black));
            d.Add(GamesSquareValues.nothing, new SignConsole(' ', ConsoleColor.Black));
            var y = new ConsoleGameField(x.Width, x.Height, x, d);
            x.Registrated((0, 0), y);

            var snakeMove = new GameLibrary.SnakeGame.SnakeMove(250);

            Console.CursorVisible = false;
            var sf = new GameLibrary.SnakeGame.SnakeField(y.Width, y.Height, y);
            y.Registrated((0, 0), sf, sf.GetCoord());
            sf.Inicializated();


            var snake = new GameLibrary.SnakeGame.Snake(sf);
            GameLibrary.SnakeGame.Berry.RandomBerry(sf);
            //var python = new Snake(myField, new coord(myField.Width / 3, myField.Height / 2), Direction.direction.down);

            snake.IsEat += (Snake) => GameLibrary.SnakeGame.Berry.RandomBerry(Snake.Location);
            snake.IsEat += (Snake) => snakeMove.Acceleration(0.9);
            //snake.IsEat += (Snake) => score.Add();
            snake.Die += (Snake) => snakeMove.Stop();
            snakeMove.Add(snake.Move);

            #region snake control
            KeyPress.Start();
            KeyPress.Set(ConsoleKey.UpArrow, (obj, ar) => snake.Up());
            KeyPress.Set(ConsoleKey.DownArrow, (obj, ar) => snake.Down());
            KeyPress.Set(ConsoleKey.LeftArrow, (obj, ar) => snake.Left());
            KeyPress.Set(ConsoleKey.RightArrow, (obj, ar) => snake.Right());
            KeyPress.Set(ConsoleKey.Escape, (obj, ar) => y.Close());



            //control.Add(new aaaa(ConsoleKey.W, python.ChangeDirection, Direction.direction.up));
            //control.Add(new aaaa(ConsoleKey.S, python.ChangeDirection, Direction.direction.down));
            //control.Add(new aaaa(ConsoleKey.A, python.ChangeDirection, Direction.direction.left));
            //control.Add(new aaaa(ConsoleKey.D, python.ChangeDirection, Direction.direction.right));
            #endregion

            try
            {
                snakeMove.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                KeyPress.Close();
                Console.SetCursorPosition(Console.WindowWidth / 2 - 22, Console.WindowHeight / 2 + 1);
            }
        }
    }
}
