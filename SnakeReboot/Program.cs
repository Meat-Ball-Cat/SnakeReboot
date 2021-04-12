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
        static Keys Keys;
        static Letters letters;
        static void Main(string[] args)
        {
            Keys = new Keys();
            KeyMenuInitialise();
            GameStart();
        }

        static void KeyMenuInitialise()
        {
            KeyPress.AddControl("Menu");
            KeyPress.Set("Menu", ConsoleKey.DownArrow, Keys[needOption.menuDown]);
            KeyPress.Set("Menu", ConsoleKey.UpArrow, Keys[needOption.menuUp]);
            KeyPress.Set("Menu", ConsoleKey.Enter, Keys[needOption.menuPress]);
            KeyPress.AddControl("Snake");
            KeyPress.Set("Snake", ConsoleKey.UpArrow, Keys[needOption.snakeUp]);
            KeyPress.Set("Snake", ConsoleKey.DownArrow, Keys[needOption.snakeDown]);
            KeyPress.Set("Snake", ConsoleKey.LeftArrow, Keys[needOption.snakeLeft]);
            KeyPress.Set("Snake", ConsoleKey.RightArrow, Keys[needOption.snakeRigh]);
        }

        static void GameStart()
        {
            ToolsLibrary.FullScreen.FullScreenOn();
            KeyPress.Start();
            var square = ConsoleWithSquare.CreateConsoleWithSquare();
            var manager = new ManagerConsoleSquare(square.Width, square.Height, square);
            square.Registrated((0, 0), manager, manager.GetCoord());
            letters = new Letters("First");
            CreateStartMeni(manager);
        }
        static void CreateStartMeni(ManagerConsoleSquare location)
        {
            KeyPress.SetControl("Menu");
            var menu = new KeyboardMenu<ButtonInConsole>("menu");
            var menuPrinter = new ConsolePrintMenu(location.Width, location.Height, location, menu);
            location.Registrated((0, 0), menuPrinter, menuPrinter.GetCoord());
            var printer = new BigPixelPrint(menuPrinter.Width, 5, menuPrinter, letters);
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
                void act()
                {
                    menuPrinter.Load();
                    Keys[needOption.menuDown].Handler = ((obj, ar) => menu.Next());
                    Keys[needOption.menuUp].Handler = (obj, ar) => menu.Previous();
                    Keys[needOption.menuPress].Handler = (obj, ar) => menu.Press();
                }
                SnakeMenuLoad(location, act);
                
            };
            menu.AddLastButton(b2);
            b2.IsPressed += () =>
            {
                menuPrinter.Hide();
                void act()
                {
                    menuPrinter.Load();
                    Keys[needOption.menuDown].Handler = ((obj, ar) => menu.Next());
                    Keys[needOption.menuUp].Handler = (obj, ar) => menu.Previous();
                    Keys[needOption.menuPress].Handler = (obj, ar) => menu.Press();
                }
                CommonSettings(location, act);
            };
            menu.AddLastButton(b3);
            b3.IsPressed += () => 
            { 
                menuPrinter.Close();
                location.Unregistrated(menuPrinter);
                KeyPress.Close();
                return;
            };

            menuPrinter.Load();

            Keys[needOption.menuDown].Handler = ((obj, ar) => menu.Next());
            Keys[needOption.menuUp].Handler = (obj, ar) => menu.Previous();
            Keys[needOption.menuPress].Handler = (obj, ar) => menu.Press();

        }
        private static void SnakeMenuLoad(ManagerConsoleSquare location, Action startWithClose)
        {
            var menu = new KeyboardMenu<ButtonInConsole>("Snake");
            var snakeMenu = new ConsolePrintMenu(location.Width, location.Height, location, menu);
            location.Registrated((0, 0), snakeMenu, snakeMenu.GetCoord());
            var printer = new BigPixelPrint(snakeMenu.Width, 5, snakeMenu, letters);
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
                SnakeStart(location);
                KeyPress.SetControl("Menu");
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
                Keys[needOption.menuUp].Handler = null;
                Keys[needOption.menuDown].Handler = null;
                Keys[needOption.menuPress].Handler = null;
                startWithClose?.Invoke();
            };

            snakeMenu.Load();
            Keys[needOption.menuDown].Handler = ((obj, ar) => menu.Next());
            Keys[needOption.menuUp].Handler = (obj, ar) => menu.Previous();
            Keys[needOption.menuPress].Handler = (obj, ar) => menu.Press();
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
            KeyPress.SetControl("Snake");

            var snake = new Snake(snakeField);
            Berry.RandomBerry(snakeField);

            snake.IsEat += (Snake) => Berry.RandomBerry(Snake.Location);
            snake.IsEat += (Snake) => snakeMove.Acceleration(0.9);
            snake.IsEat += (Snake) => score.Add();
            snake.Die += (Snake) => snakeMove.Stop();
            snakeMove.Add(snake.Move);

            Keys[needOption.snakeUp].Handler = (obj, ar) => snake.Up();
            Keys[needOption.snakeDown].Handler = (obj, ar) => snake.Down();
            Keys[needOption.snakeLeft].Handler = (obj, ar) => snake.Left();
            Keys[needOption.snakeRigh].Handler = (obj, ar) => snake.Right();

            snakeMove.Start();
            Keys[needOption.snakeUp].Handler = null;
            Keys[needOption.snakeDown].Handler = null;
            Keys[needOption.snakeLeft].Handler = null;
            Keys[needOption.snakeRigh].Handler = null;
            Console.ReadKey();
            gameField.Close();
            scoreField.Close();
            
            location.Unregistrated(gameField);
        }
        static void CommonSettings(ManagerConsoleSquare location, Action startWithClose)
        {
            var menu = new KeyboardMenu<ButtonInConsole>("Settings");
            var sattingsMenu = new ConsolePrintMenu(location.Width, location.Height, location, menu);
            location.Registrated((0, 0), sattingsMenu, sattingsMenu.GetCoord());
            var printer = new BigPixelPrint(sattingsMenu.Width, 5, sattingsMenu, letters);
            sattingsMenu.Registrated((0, 1), printer, printer.GetCoord());
            sattingsMenu.SetWriter(printer);

            var b1 = new ButtonInConsoleSetter(sattingsMenu.Width - 10, 1, sattingsMenu, SignConsole.GetSignConsoles("Menu Up"));
            b1.Value = SignConsole.GetSignConsoles(KeyPress.GetKey("Menu", Keys[needOption.menuUp]).ToString());
            var b2 = new ButtonInConsoleSetter(sattingsMenu.Width - 10, 1, sattingsMenu, SignConsole.GetSignConsoles("Menu Down"));
            b2.Value = SignConsole.GetSignConsoles(KeyPress.GetKey("Menu", Keys[needOption.menuDown]).ToString());
            var b3 = new ButtonInConsoleSetter(sattingsMenu.Width - 10, 1, sattingsMenu, SignConsole.GetSignConsoles("Menu Select"));
            b3.Value = SignConsole.GetSignConsoles(KeyPress.GetKey("Menu", Keys[needOption.menuPress]).ToString());
            var b4 = new ButtonInConsole(6, 1, sattingsMenu, SignConsole.GetSignConsoles("Exit"));
            sattingsMenu.Registrated((5, 8), b1, b1.GetCoord());
            sattingsMenu.Registrated((5, 10), b2, b2.GetCoord());
            sattingsMenu.Registrated((5, 12), b3, b3.GetCoord());
            sattingsMenu.Registrated((sattingsMenu.Width / 2 - 3, 14), b4, b4.GetCoord());
            menu.AddLastButton(b1);
            b1.IsPressed += () =>
            {
                var newKey = Console.ReadKey();
                KeyPress.Remove("Menu", KeyPress.GetKey("Menu", Keys[needOption.menuUp]));
                KeyPress.Reset("Menu", newKey.Key, Keys[needOption.menuUp]);
                b1.Value = SignConsole.GetSignConsoles(KeyPress.GetKey("Menu", Keys[needOption.menuUp]).ToString());
            };

            menu.AddLastButton(b2);
            b2.IsPressed += () =>
            {
                var newKey = Console.ReadKey();
                KeyPress.Remove("Menu", KeyPress.GetKey("Menu", Keys[needOption.menuDown]));
                KeyPress.Reset("Menu", newKey.Key, Keys[needOption.menuDown]);
                b2.Value = SignConsole.GetSignConsoles(KeyPress.GetKey("Menu", Keys[needOption.menuDown]).ToString());
            };

            menu.AddLastButton(b3);
            b3.IsPressed += () =>
            {
                var newKey = Console.ReadKey();
                KeyPress.Remove("Menu", KeyPress.GetKey("Menu", Keys[needOption.menuPress]));
                KeyPress.Reset("Menu", newKey.Key, Keys[needOption.menuPress]);
                b3.Value = SignConsole.GetSignConsoles(KeyPress.GetKey("Menu", Keys[needOption.menuPress]).ToString());
            };

            menu.AddLastButton(b4);
            b4.IsPressed += () =>
            {
                sattingsMenu.Close();
                location.Unregistrated(sattingsMenu);
                Keys[needOption.menuDown].Handler = null;
                Keys[needOption.menuUp].Handler = null;
                Keys[needOption.menuPress].Handler = null;
                startWithClose?.Invoke();
            };

            sattingsMenu.Load();
            Keys[needOption.menuDown].Handler = ((obj, ar) => menu.Next());
            Keys[needOption.menuUp].Handler = (obj, ar) => menu.Previous();
            Keys[needOption.menuPress].Handler = (obj, ar) => menu.Press();

        }

    }
}
