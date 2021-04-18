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
            KeyPress.Set("Snake", ConsoleKey.Escape, Keys[needOption.snakePause]);
        }
        static void GameStart()
        {
            ToolsLibrary.FullScreen.FullScreenOn();
            KeyPress.Start();
            var square = ConsoleWithSquare.CreateConsoleWithSquare();
            var manager = new ManagerConsoleSquare(square.Width, square.Height, square);
            square.Register((0, 0), manager, manager.GetCoordinates());
            letters = new Letters("First");
            CreateStartMeni(manager);
        }
        static void CreateStartMeni(IDrawningRectangle<SignConsole> location)
        {
            KeyPress.SetControl("Menu");
            var menu = new KeyboardMenu<ButtonInConsole>("menu");
            var menuPrinter = new ConsolePrintMenu(location.Width, location.Height, location, menu);
            location.Register((0, 0), menuPrinter, menuPrinter.GetCoordinates());
            var printer = new BigPixelPrint(menuPrinter.Width, 5, menuPrinter, letters);
            menuPrinter.Register((0, 1), printer, printer.GetCoordinates());
            menuPrinter.SetWriter(printer);
            void SetControl()
            {
                Keys[needOption.menuDown].Set((obj, ar) => menu.Next());
                Keys[needOption.menuUp].Set((obj, ar) => menu.Previous());
                Keys[needOption.menuPress].Set((obj, ar) => menu.Press());
            }
            SetControl();

            var b1 = new ButtonInConsole(menuPrinter.Width - 10, 1, menuPrinter, SignConsole.GetSignConsoles("Snake start"));
            var b2 = new ButtonInConsole(menuPrinter.Width - 10, 1, menuPrinter, SignConsole.GetSignConsoles("Settings"));
            var b3 = new ButtonInConsole(menuPrinter.Width - 10, 1, menuPrinter, SignConsole.GetSignConsoles("Exit"));
            menuPrinter.Register((5, 8), b1, b1.GetCoordinates());
            menuPrinter.Register((5, 10), b2, b2.GetCoordinates());
            menuPrinter.Register((5, 12), b3, b3.GetCoordinates());

            menu.AddLastButton(b1);
            b1.IsPressed += () =>
            {
                menuPrinter.Hide();
                void act()
                {
                    menuPrinter.Load();
                    SetControl();
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
                    SetControl();
                }
                CommonSettings(location, act);
            };

            menu.AddLastButton(b3);
            b3.IsPressed += () => 
            { 
                menuPrinter.Close();
                location.CancelRegistration(menuPrinter);
                KeyPress.Close();
                return;
            };

            menuPrinter.Load();
        }
        static void SnakeMenuLoad(IDrawningRectangle<SignConsole> location, Action startWithClose)
        {
            
            var menu = new KeyboardMenu<ButtonInConsole>("Snake");
            var snakeMenu = new ConsolePrintMenu(location.Width, location.Height, location, menu);
            location.Register((0, 0), snakeMenu, snakeMenu.GetCoordinates());
            var printer = new BigPixelPrint(snakeMenu.Width, 5, snakeMenu, letters);
            snakeMenu.Register((0, 1), printer, printer.GetCoordinates());
            snakeMenu.SetWriter(printer);
            void SetControl() 
            {
                Keys[needOption.menuDown].Set((obj, ar) => menu.Next());
                Keys[needOption.menuUp].Set((obj, ar) => menu.Previous());
                Keys[needOption.menuPress].Set((obj, ar) => menu.Press(), true);
            }
            SetControl();

            var b1 = new ButtonInConsole(snakeMenu.Width - 10, 1, snakeMenu, SignConsole.GetSignConsoles("Start"));
            var b2 = new ButtonInConsole(snakeMenu.Width - 10, 1, snakeMenu, SignConsole.GetSignConsoles("Mode"));
            var b3 = new ButtonInConsole(snakeMenu.Width - 10, 1, snakeMenu, SignConsole.GetSignConsoles("Settings"));
            var b4 = new ButtonInConsole(snakeMenu.Width - 10, 1, snakeMenu, SignConsole.GetSignConsoles("Exit"));
            snakeMenu.Register((5, 8), b1, b1.GetCoordinates());
            snakeMenu.Register((5, 10), b2, b2.GetCoordinates());
            snakeMenu.Register((5, 12), b3, b3.GetCoordinates());
            snakeMenu.Register((5, 14), b4, b4.GetCoordinates());

            menu.AddLastButton(b1);
            b1.IsPressed += () =>
            {
                snakeMenu.Hide();
                void act()
                {
                    snakeMenu.Load();
                    SetControl();
                }
                SnakeStart(location, act);
                KeyPress.SetControl("Menu");
                snakeMenu.Load();
            };

            menu.AddLastButton(b2);
            b2.IsPressed += () => Console.Beep(500, 500);

            menu.AddLastButton(b3);
            b3.IsPressed += () =>
            {
                snakeMenu.Hide();
                void act()
                {
                    snakeMenu.Load();
                    SetControl();
                }
                SnakeSettings(location, act);
            };

            menu.AddLastButton(b4);
            b4.IsPressed += () => {
                snakeMenu.Close();
                location.CancelRegistration(snakeMenu);
                Keys[needOption.menuDown].Set(null);
                Keys[needOption.menuUp].Set(null);
                Keys[needOption.menuPress].Set(null);
                startWithClose?.Invoke();
            };
            snakeMenu.Load();           
        }
        static void SnakeStart(IDrawningRectangle<SignConsole> location, Action startWithClose)
        {
            var GameDictionary = new Dictionary<GamesSquareValues, SignConsole>
            {
                { GamesSquareValues.snake, new SignConsole(' ', ConsoleColor.White) },
                { GamesSquareValues.snakeBerry, new SignConsole(' ', ConsoleColor.Red) },
                { GamesSquareValues.snakeWall, new SignConsole('X', ConsoleColor.Black) },
                { GamesSquareValues.nothing, new SignConsole(' ', ConsoleColor.Black) }
            };
            var gameField = new ConsoleGameField(location.Width, location.Height, location, GameDictionary);
            location.Register((0, 0), gameField, gameField.GetCoordinates());

            var snakeMove = new SnakeMove(250);

            var snakeField = new SnakeField(gameField.Width, gameField.Height - 5, gameField);
            gameField.Register((0, 5), snakeField, snakeField.GetCoordinates());
            snakeField.Inicializated();
            var bigChar = new Letters("First");
            var scoreField = new BigPixelPrint(18, 5, location, bigChar);
            location.Register((location.Width / 2 - 9, 0), scoreField, scoreField.GetCoordinates());
            var score = new Score(scoreField);
            KeyPress.SetControl("Snake");

            var snake = new Snake(snakeField);
            Berry.RandomBerry(snakeField);

            snake.IsEat += (Snake) => Berry.RandomBerry(Snake.Location);
            snake.IsEat += (Snake) => snakeMove.Acceleration(0.9);
            snake.IsEat += (Snake) => score.Add();
            snake.Die += (Snake) => snakeMove.Stop();
            snakeMove.Add(snake.Move);

            Keys[needOption.snakeUp].Set((obj, ar) => snake.Up());
            Keys[needOption.snakeDown].Set((obj, ar) => snake.Down());
            Keys[needOption.snakeLeft].Set((obj, ar) => snake.Left());
            Keys[needOption.snakeRigh].Set((obj, ar) => snake.Right());
            Keys[needOption.snakePause].Set((obj, ar) =>
            {
                snakeMove.Pause();
                void cont()
                {
                    gameField.Load();
                    score.Add(0);
                    snakeMove.Continue();
                    KeyPress.SetControl("Snake");
                }
                Pause(location, cont, exit);
            });

            snakeMove.Start();
            Keys[needOption.snakeUp].Set(null);
            Keys[needOption.snakeDown].Set(null);
            Keys[needOption.snakeLeft].Set(null);
            Keys[needOption.snakeRigh].Set(null);
            Console.ReadKey(true);
            void exit()
            {
                snakeMove.Stop();
                gameField.Close();
                scoreField.Close();               
                location.CancelRegistration(gameField);
                startWithClose();
            }
            exit();
        }
        static void CommonSettings(IDrawningRectangle<SignConsole> location, Action startWithClose)
        {
            var menu = new KeyboardMenu<ButtonInConsole>("Settings");
            var sattingsMenu = new ConsolePrintMenu(location.Width, location.Height, location, menu);
            location.Register((0, 0), sattingsMenu, sattingsMenu.GetCoordinates());
            var printer = new BigPixelPrint(sattingsMenu.Width, 5, sattingsMenu, letters);
            sattingsMenu.Register((0, 1), printer, printer.GetCoordinates());
            sattingsMenu.SetWriter(printer);

            var b1 = new ButtonInConsoleSetter(sattingsMenu.Width - 10, 1, sattingsMenu, SignConsole.GetSignConsoles("Menu Up"));
            var b2 = new ButtonInConsoleSetter(sattingsMenu.Width - 10, 1, sattingsMenu, SignConsole.GetSignConsoles("Menu Down"));
            var b3 = new ButtonInConsoleSetter(sattingsMenu.Width - 10, 1, sattingsMenu, SignConsole.GetSignConsoles("Menu Select"));
            var b4 = new ButtonInConsole(6, 1, sattingsMenu, SignConsole.GetSignConsoles("Exit"));
            sattingsMenu.Register((5, 8), b1, b1.GetCoordinates());
            sattingsMenu.Register((5, 10), b2, b2.GetCoordinates());
            sattingsMenu.Register((5, 12), b3, b3.GetCoordinates());
            sattingsMenu.Register((5, 14), b4, b4.GetCoordinates());
            void setValue()
            {
                b1.Value = SignConsole.GetSignConsoles(KeyPress.GetKey("Menu", Keys[needOption.menuUp]).ToString());
                b2.Value = SignConsole.GetSignConsoles(KeyPress.GetKey("Menu", Keys[needOption.menuDown]).ToString());
                b3.Value = SignConsole.GetSignConsoles(KeyPress.GetKey("Menu", Keys[needOption.menuPress]).ToString()); 
            }
            setValue();

            menu.AddLastButton(b1);
            b1.IsPressed += () =>
            {
                var newKey = Console.ReadKey(true);
                KeyPress.Remove("Menu", KeyPress.GetKey("Menu", Keys[needOption.menuUp]));
                KeyPress.Reset("Menu", newKey.Key, Keys[needOption.menuUp]);
                setValue();
            };

            menu.AddLastButton(b2);
            b2.IsPressed += () =>
            {
                var newKey = Console.ReadKey(true);
                KeyPress.Remove("Menu", KeyPress.GetKey("Menu", Keys[needOption.menuDown]));
                KeyPress.Reset("Menu", newKey.Key, Keys[needOption.menuDown]);
                setValue();
            };

            menu.AddLastButton(b3);
            b3.IsPressed += () =>
            {
                var newKey = Console.ReadKey(true);
                KeyPress.Remove("Menu", KeyPress.GetKey("Menu", Keys[needOption.menuPress]));
                KeyPress.Reset("Menu", newKey.Key, Keys[needOption.menuPress]);
                setValue();
            };

            menu.AddLastButton(b4);
            b4.IsPressed += () =>
            {
                sattingsMenu.Close();
                location.CancelRegistration(sattingsMenu);
                Keys[needOption.menuDown].Set(null);
                Keys[needOption.menuUp].Set(null);
                Keys[needOption.menuPress].Set(null);
                startWithClose?.Invoke();
            };

            sattingsMenu.Load();
            Keys[needOption.menuDown].Set((obj, ar) => menu.Next());
            Keys[needOption.menuUp].Set((obj, ar) => menu.Previous());
            Keys[needOption.menuPress].Set((obj, ar) => menu.Press());
        }
        static void SnakeSettings(IDrawningRectangle<SignConsole> location, Action startWithClose, bool clear = false)
        {
            var menu = new KeyboardMenu<ButtonInConsole>("Snake Settings");
            var sattingsMenu = new ConsolePrintMenu(location.Width, location.Height, location, menu);
            location.Register((0, 0), sattingsMenu, sattingsMenu.GetCoordinates());
            var printer = new BigPixelPrint(sattingsMenu.Width, 5, sattingsMenu, letters);
            sattingsMenu.Register((0, 1), printer, printer.GetCoordinates());
            sattingsMenu.SetWriter(printer);
            if (clear)
            {
                sattingsMenu.Fill(new SignConsole(' '));
            }

            var b1 = new ButtonInConsoleSetter(sattingsMenu.Width - 10, 1, sattingsMenu, SignConsole.GetSignConsoles("Up"));
            var b2 = new ButtonInConsoleSetter(sattingsMenu.Width - 10, 1, sattingsMenu, SignConsole.GetSignConsoles("Down"));
            var b3 = new ButtonInConsoleSetter(sattingsMenu.Width - 10, 1, sattingsMenu, SignConsole.GetSignConsoles("Left"));
            var b4 = new ButtonInConsoleSetter(sattingsMenu.Width - 10, 1, sattingsMenu, SignConsole.GetSignConsoles("Right"));
            var b5 = new ButtonInConsoleSetter(sattingsMenu.Width - 10, 1, sattingsMenu, SignConsole.GetSignConsoles("Pause"));
            var b6 = new ButtonInConsole(6, 1, sattingsMenu, SignConsole.GetSignConsoles("Exit"));
            sattingsMenu.Register((5, 8), b1, b1.GetCoordinates());
            sattingsMenu.Register((5, 10), b2, b2.GetCoordinates());
            sattingsMenu.Register((5, 12), b3, b3.GetCoordinates());
            sattingsMenu.Register((5, 14), b4, b4.GetCoordinates());
            sattingsMenu.Register((5, 16), b5, b5.GetCoordinates());
            sattingsMenu.Register((5, 19), b6, b6.GetCoordinates());
            void setValue()
            {
                b1.Value = SignConsole.GetSignConsoles(KeyPress.GetKey("Snake", Keys[needOption.snakeUp]).ToString());
                b2.Value = SignConsole.GetSignConsoles(KeyPress.GetKey("Snake", Keys[needOption.snakeDown]).ToString());
                b3.Value = SignConsole.GetSignConsoles(KeyPress.GetKey("Snake", Keys[needOption.snakeLeft]).ToString());
                b4.Value = SignConsole.GetSignConsoles(KeyPress.GetKey("Snake", Keys[needOption.snakeRigh]).ToString());
                b5.Value = SignConsole.GetSignConsoles(KeyPress.GetKey("Snake", Keys[needOption.snakePause]).ToString());
            }
            setValue();

            menu.AddLastButton(b1);
            b1.IsPressed += () =>
            {
                var newKey = Console.ReadKey(true);
                KeyPress.Remove("Snake", KeyPress.GetKey("Snake", Keys[needOption.snakeUp]));
                KeyPress.Reset("Snake", newKey.Key, Keys[needOption.snakeUp]);
                setValue();
            };

            menu.AddLastButton(b2);
            b2.IsPressed += () =>
            {
                var newKey = Console.ReadKey(true);
                KeyPress.Remove("Snake", KeyPress.GetKey("Snake", Keys[needOption.snakeDown]));
                KeyPress.Reset("Snake", newKey.Key, Keys[needOption.snakeDown]);
                setValue();
            };

            menu.AddLastButton(b3);
            b3.IsPressed += () =>
            {
                var newKey = Console.ReadKey(true);
                KeyPress.Remove("Snake", KeyPress.GetKey("Snake", Keys[needOption.snakeLeft]));
                KeyPress.Reset("Snake", newKey.Key, Keys[needOption.snakeLeft]);
                setValue();
            };

            menu.AddLastButton(b4);
            b4.IsPressed += () =>
            {
                var newKey = Console.ReadKey(true);
                KeyPress.Remove("Snake", KeyPress.GetKey("Snake", Keys[needOption.snakeRigh]));
                KeyPress.Reset("Snake", newKey.Key, Keys[needOption.snakeRigh]);
                setValue();
            };

            menu.AddLastButton(b5);
            b5.IsPressed += () =>
            {
                var newKey = Console.ReadKey(true);
                KeyPress.Remove("Snake", KeyPress.GetKey("Snake", Keys[needOption.snakePause]));
                KeyPress.Reset("Snake", newKey.Key, Keys[needOption.snakePause]);
                setValue();
            };

            menu.AddLastButton(b6);
            b6.IsPressed += () =>
            {
                sattingsMenu.Close();
                location.CancelRegistration(sattingsMenu);
                Keys[needOption.menuDown].Set(null);
                Keys[needOption.menuUp].Set(null);
                Keys[needOption.menuPress].Set(null);
                startWithClose?.Invoke();
            };

            sattingsMenu.Load();
            Keys[needOption.menuDown].Set((obj, ar) => menu.Next());
            Keys[needOption.menuUp].Set((obj, ar) => menu.Previous());
            Keys[needOption.menuPress].Set((obj, ar) => menu.Press());
        }
        static void Pause(IDrawningRectangle<SignConsole> location, Action contin, Action exit)
        {
            var menu = new KeyboardMenu<ButtonInConsole>("Pause");
            var pauseMenu = new ConsolePrintMenu(location.Width / 2, location.Height / 3 * 2, location, menu);
            location.Register((location.Width / 4, location.Height / 6), pauseMenu, pauseMenu.GetCoordinates());
            var printer = new BigPixelPrint(pauseMenu.Width - 2, 5, pauseMenu, letters);
            pauseMenu.Frame(new SignConsole('0'));
            pauseMenu.Register((2, 2), printer, printer.GetCoordinates());
            pauseMenu.SetWriter(printer);
           
            KeyPress.SetControl("Menu");
            void SetControl()
            {
                Keys[needOption.menuDown].Set((obj, ar) => menu.Next());
                Keys[needOption.menuUp].Set((obj, ar) => menu.Previous());
                Keys[needOption.menuPress].Set((obj, ar) => menu.Press());
            }
            SetControl();

            var pauseContinue = new ButtonInConsole(pauseMenu.Width - 10, 1, pauseMenu, SignConsole.GetSignConsoles("Continue"));
            var pauseSetting = new ButtonInConsole(pauseMenu.Width - 10, 1, pauseMenu, SignConsole.GetSignConsoles("Settings"));
            var pauseExit = new ButtonInConsole(pauseMenu.Width - 10, 1, pauseMenu, SignConsole.GetSignConsoles("Exit"));
            pauseMenu.Register((5, 8), pauseContinue, pauseContinue.GetCoordinates());
            pauseMenu.Register((5, 10), pauseSetting, pauseSetting.GetCoordinates());
            pauseMenu.Register((5, 12), pauseExit, pauseExit.GetCoordinates());

            menu.AddLastButton(pauseContinue);
            pauseContinue.IsPressed += () =>
            {
                pauseMenu.Close();
                location.CancelRegistration(pauseMenu);
                Keys[needOption.menuDown].Set(null);
                Keys[needOption.menuUp].Set(null);
                Keys[needOption.menuPress].Set(null);
                contin?.Invoke();
            };

            menu.AddLastButton(pauseSetting);
            pauseSetting.IsPressed += () =>
            {
                pauseMenu.Hide();
                void act()
                {
                    location.Load();
                    SetControl();
                }
                SnakeSettings(location, act, true);
            };

            menu.AddLastButton(pauseExit);
            pauseExit.IsPressed += () =>
            {
                pauseMenu.Close();
                location.CancelRegistration(pauseMenu);
                Keys[needOption.menuDown].Set(null);
                Keys[needOption.menuUp].Set(null);
                Keys[needOption.menuPress].Set(null);
                exit?.Invoke();
            };
            pauseMenu.Fill(new SignConsole(' '));
            pauseMenu.Load();
        }
    }
}
