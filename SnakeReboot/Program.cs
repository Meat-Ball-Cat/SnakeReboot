using SquareRectangle;
using GameLibrary;
using TolsLibrary;
using System;

namespace SnakeReboot
{
    class Program
    {
        static void Main(string[] args)
        {
            ToolsLibrary.FullScreen.FullScreenOn();
            var square = ConsoleWithSquare.CreateConsoleWithSquare();
            var ch = new Letters("First");
           
            ch.Add(new PixelLetter('A', new bool[] { false,  true,  true,  true, false,
                                                       true, false, false, false,  true,
                                                       true,  true,  true,  true,  true,
                                                       true, false, false, false,  true,
                                                       true, false, false, false,  true,}));
            var r = new DrawingRectangle<SignConsole>(square.Width, square.Height - 1, square);
            var sq = new BigPixelPrint(r.Width, r.Height, r, ch);
            square.Registrated((0, 1), r, r.GetCoord());
            r.Registrated((0, 0), sq, sq.GetCoord());
            sq.Print("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            KeyPress.Start();

            
            //var x = ConsoleWithSquare.CreateConsoleWithSquare();
            //var m = new ManagerConsoleSquare(x.Width - 1, x.Height - 1, x);
            //x.Registrated((1, 1), m, m.GetCoord().Select(t => t + (1, 1)).ToArray());

            //var menu = new KeyboardMenu<ButtonInConsole>("menu");
            //var menuPrinter = new ConsolePrintMenu(m.Width, m.Height, m, menu);
            //m.Registrated((0, 0), menuPrinter, menuPrinter.GetCoord());
            //var b1 = new ButtonInConsole(menuPrinter.Width - 2, 1, menuPrinter, SignConsole.GetSignConsoles("Button_1"));
            //var b2 = new ButtonInConsole(menuPrinter.Width - 2, 1, menuPrinter, SignConsole.GetSignConsoles("Button_2"));
            //var b3 = new ButtonInConsole(menuPrinter.Width - 2, 1, menuPrinter, SignConsole.GetSignConsoles("Button_3"));
            //menuPrinter.Registrated((2, 5), b1, b1.GetCoord());
            //menuPrinter.Registrated((2, 7), b2, b2.GetCoord());
            //menuPrinter.Registrated((2, 10), b3, b3.GetCoord());
            //menu.AddLastButton(b1);
            //menu.AddLastButton(b2);
            //menu.AddLastButton(b3);
            //menuPrinter.Load();
            //KeyPress.Set(ConsoleKey.DownArrow, (obj, ar) => menu.Next());
            //KeyPress.Set(ConsoleKey.UpArrow, (obj, ar) => menu.Previous());
            //KeyPress.Set(ConsoleKey.Enter, (obj, ar) => menu.Press());
            //b1.IsPressed += () => Console.Beep(700, 100);
            //b2.IsPressed += () => Console.Beep(1000, 1000);
            //b3.IsPressed += () => Console.Beep(500, 500);


            //    var d = new Dictionary<GamesSquareValues, SignConsole>();
            //    d.Add(GamesSquareValues.snake, new SignConsole(' ', ConsoleColor.White));
            //    d.Add(GamesSquareValues.snakeBerry, new SignConsole(' ', ConsoleColor.Red));
            //    d.Add(GamesSquareValues.snakeWall, new SignConsole('X', ConsoleColor.Black));
            //    d.Add(GamesSquareValues.nothing, new SignConsole(' ', ConsoleColor.Black));
            //    var y = new ConsoleGameField(x.Width, x.Height, x, d);
            //    x.Registrated((0, 0), y);

            //    var snakeMove = new GameLibrary.SnakeGame.SnakeMove(250);

            //    Console.CursorVisible = false;
            //    var sf = new GameLibrary.SnakeGame.SnakeField(y.Width, y.Height, y);
            //    y.Registrated((0, 0), sf, sf.GetCoord());
            //    sf.Inicializated();


            //    var snake = new GameLibrary.SnakeGame.Snake(sf);
            //    GameLibrary.SnakeGame.Berry.RandomBerry(sf);
            //    //var python = new Snake(myField, new coord(myField.Width / 3, myField.Height / 2), Direction.direction.down);

            //    snake.IsEat += (Snake) => GameLibrary.SnakeGame.Berry.RandomBerry(Snake.Location);
            //    snake.IsEat += (Snake) => snakeMove.Acceleration(0.9);
            //    //snake.IsEat += (Snake) => score.Add();
            //    snake.Die += (Snake) => snakeMove.Stop();
            //    snakeMove.Add(snake.Move);

            //    #region snake control
            //    KeyPress.Start();
            //    KeyPress.Set(ConsoleKey.UpArrow, (obj, ar) => snake.Up());
            //    KeyPress.Set(ConsoleKey.DownArrow, (obj, ar) => snake.Down());
            //    KeyPress.Set(ConsoleKey.LeftArrow, (obj, ar) => snake.Left());
            //    KeyPress.Set(ConsoleKey.RightArrow, (obj, ar) => snake.Right());
            KeyPress.Set(ConsoleKey.Escape, (obj, ar) => {  KeyPress.Close(); });



            //    //control.Add(new aaaa(ConsoleKey.W, python.ChangeDirection, Direction.direction.up));
            //    //control.Add(new aaaa(ConsoleKey.S, python.ChangeDirection, Direction.direction.down));
            //    //control.Add(new aaaa(ConsoleKey.A, python.ChangeDirection, Direction.direction.left));
            //    //control.Add(new aaaa(ConsoleKey.D, python.ChangeDirection, Direction.direction.right));
            //    #endregion

            //    try
            //    {
            //        snakeMove.Start();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //    finally
            //    {
            //        KeyPress.Close();
            //        Console.SetCursorPosition(Console.WindowWidth / 2 - 22, Console.WindowHeight / 2 + 1);
            //    }
        }
    }
}
