using SquareRectangle;
using GameLibrary;
using TolsLibrary;
using System;
using System.Linq;
using GameLibrary.MenuLibrary;
using System.Collections.Generic;
using GameLibrary.SnakeGame;

namespace SnakeReboot
{
    class Program
    {
        static void Main(string[] args)
        {
            GameStart();
        }

        static void GameStart()
        {
            ToolsLibrary.FullScreen.FullScreenOn();
            KeyPress.Start();
            var square = ConsoleWithSquare.CreateConsoleWithSquare();
            var manager = new ManagerConsoleSquare(square.Width, square.Height, square);
            square.Registrated((0, 0), manager, manager.GetCoord());
            var bigChar = new Letters("First");
            CreateStartMeni(manager, bigChar);
        }

        static void CreateStartMeni(ManagerConsoleSquare location, Letters letetrs)
        {
            var menu = new KeyboardMenu<ButtonInConsole>("menu");
            var menuPrinter = new ConsolePrintMenu(location.Width, location.Height, location, menu);
            location.Registrated((0, 0), menuPrinter, menuPrinter.GetCoord());
            var printer = new BigPixelPrint(menuPrinter.Width, 5, menuPrinter, letetrs);
            menuPrinter.Registrated((0, 1), printer, printer.GetCoord());
            menuPrinter.SetWriter(printer);

            var b1 = new ButtonInConsole(menuPrinter.Width - 10, 1, menuPrinter, SignConsole.GetSignConsoles("Snake start"));
            var b2 = new ButtonInConsole(menuPrinter.Width - 10, 1, menuPrinter, SignConsole.GetSignConsoles("Settings"));
            var b3 = new ButtonInConsole(menuPrinter.Width - 10, 1, menuPrinter, SignConsole.GetSignConsoles("Exit"));
            menuPrinter.Registrated((5, 8), b1, b1.GetCoord());
            menuPrinter.Registrated((5, 10), b2, b2.GetCoord());
            menuPrinter.Registrated((5, 12), b3, b3.GetCoord());
            menu.AddLastButton(b1);
            b1.IsPressed += () =>
            {
                menuPrinter.Hide();
                KeyPress.Remove(ConsoleKey.DownArrow);
                KeyPress.Remove(ConsoleKey.UpArrow);
                KeyPress.Remove(ConsoleKey.Enter);
                void act()
                {
                    menuPrinter.Load();
                    KeyPress.Set(ConsoleKey.DownArrow, (obj, ar) => menu.Next());
                    KeyPress.Set(ConsoleKey.UpArrow, (obj, ar) => menu.Previous());
                    KeyPress.Set(ConsoleKey.Enter, (obj, ar) => menu.Press());
                }
                SnakeMenuLoad(location, letetrs, act);
                
            };
            menu.AddLastButton(b2);
            b2.IsPressed += () => Console.Beep(1000, 500);
            menu.AddLastButton(b3);
            b3.IsPressed += () => { 
                menuPrinter.Close();
                location.Unregistrated(menuPrinter);
                KeyPress.Remove(ConsoleKey.DownArrow);
                KeyPress.Remove(ConsoleKey.UpArrow);
                KeyPress.Remove(ConsoleKey.Enter);
                KeyPress.Close();
                return;
            };

            menuPrinter.Load();

            KeyPress.Set(ConsoleKey.DownArrow, (obj, ar) => menu.Next());
            KeyPress.Set(ConsoleKey.UpArrow, (obj, ar) => menu.Previous());
            KeyPress.Set(ConsoleKey.Enter, (obj, ar) => menu.Press());

        }

        private static void SnakeMenuLoad(ManagerConsoleSquare location, Letters letetrs, Action startWithClose)
        {
            var menu = new KeyboardMenu<ButtonInConsole>("Snake");
            var snakeMenu = new ConsolePrintMenu(location.Width, location.Height, location, menu);
            location.Registrated((0, 0), snakeMenu, snakeMenu.GetCoord());
            var printer = new BigPixelPrint(snakeMenu.Width, 5, snakeMenu, letetrs);
            snakeMenu.Registrated((0, 1), printer, printer.GetCoord());
            snakeMenu.SetWriter(printer);

            var b1 = new ButtonInConsole(snakeMenu.Width - 10, 1, snakeMenu, SignConsole.GetSignConsoles("Start"));
            var b2 = new ButtonInConsole(snakeMenu.Width - 10, 1, snakeMenu, SignConsole.GetSignConsoles("Mode"));
            var b3 = new ButtonInConsole(snakeMenu.Width - 10, 1, snakeMenu, SignConsole.GetSignConsoles("Settings"));
            var b4 = new ButtonInConsole(snakeMenu.Width - 10, 1, snakeMenu, SignConsole.GetSignConsoles("Exit"));
            snakeMenu.Registrated((5, 8), b1, b1.GetCoord());
            snakeMenu.Registrated((5, 10), b2, b2.GetCoord());
            snakeMenu.Registrated((5, 12), b3, b3.GetCoord());
            snakeMenu.Registrated((5, 14), b4, b4.GetCoord());
            menu.AddLastButton(b1);
            b1.IsPressed += () =>
            {
                snakeMenu.Hide();
                KeyPress.Remove(ConsoleKey.DownArrow);
                KeyPress.Remove(ConsoleKey.UpArrow);
                KeyPress.Remove(ConsoleKey.Enter);
                SnakeStart(location);
                KeyPress.Set(ConsoleKey.DownArrow, (obj, ar) => menu.Next());
                KeyPress.Set(ConsoleKey.UpArrow, (obj, ar) => menu.Previous());
                KeyPress.Set(ConsoleKey.Enter, (obj, ar) => menu.Press());
                snakeMenu.Load();
            };
            menu.AddLastButton(b2);
            b2.IsPressed += () => Console.Beep(500, 500);
            menu.AddLastButton(b3);
            b3.IsPressed += () => Console.Beep(1000, 500);
            menu.AddLastButton(b4);
            b4.IsPressed += () => {
                snakeMenu.Close();
                location.Unregistrated(snakeMenu);
                KeyPress.Remove(ConsoleKey.DownArrow);
                KeyPress.Remove(ConsoleKey.UpArrow);
                KeyPress.Remove(ConsoleKey.Enter);
                startWithClose?.Invoke();
            };

            snakeMenu.Load();
            KeyPress.Set(ConsoleKey.DownArrow, (obj, ar) => menu.Next());
            KeyPress.Set(ConsoleKey.UpArrow, (obj, ar) => menu.Previous());
            KeyPress.Set(ConsoleKey.Enter, (obj, ar) => menu.Press());
        }
        static void SnakeStart(ManagerConsoleSquare location)
        {
            var GameDictionary = new Dictionary<GamesSquareValues, SignConsole>
            {
                { GamesSquareValues.snake, new SignConsole(' ', ConsoleColor.White) },
                { GamesSquareValues.snakeBerry, new SignConsole(' ', ConsoleColor.Red) },
                { GamesSquareValues.snakeWall, new SignConsole('X', ConsoleColor.Black) },
                { GamesSquareValues.nothing, new SignConsole(' ', ConsoleColor.Black) }
            };
            var gameField = new ConsoleGameField(location.Width, location.Height, location, GameDictionary);
            location.Registrated((0, 0), gameField, gameField.GetCoord());

            var snakeMove = new GameLibrary.SnakeGame.SnakeMove(250);

            var snakeField = new SnakeField(gameField.Width, gameField.Height - 5, gameField);
            gameField.Registrated((0, 5), snakeField, snakeField.GetCoord());
            snakeField.Inicializated();
            var bigChar = new Letters("First");
            var scoreField = new BigPixelPrint(18, 5, location, bigChar);
            location.Registrated((location.Width / 2 - 9, 0), scoreField, scoreField.GetCoord());
            var score = new Score(scoreField); 


            var snake = new Snake(snakeField);
            Berry.RandomBerry(snakeField);

            snake.IsEat += (Snake) => Berry.RandomBerry(Snake.Location);
            snake.IsEat += (Snake) => snakeMove.Acceleration(0.9);
            snake.IsEat += (Snake) => score.Add();
            snake.Die += (Snake) => snakeMove.Stop();
            snakeMove.Add(snake.Move);

            KeyPress.Set(ConsoleKey.UpArrow, (obj, ar) => snake.Up());
            KeyPress.Set(ConsoleKey.DownArrow, (obj, ar) => snake.Down());
            KeyPress.Set(ConsoleKey.LeftArrow, (obj, ar) => snake.Left());
            KeyPress.Set(ConsoleKey.RightArrow, (obj, ar) => snake.Right());

            snakeMove.Start();
            KeyPress.Remove(ConsoleKey.UpArrow);
            KeyPress.Remove(ConsoleKey.DownArrow);
            KeyPress.Remove(ConsoleKey.LeftArrow);
            KeyPress.Remove(ConsoleKey.RightArrow);
            Console.ReadKey();
            gameField.Close();
            scoreField.Close();
            
            location.Unregistrated(gameField);
        }
    }
}
