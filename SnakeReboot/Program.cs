using SquareRectangle;
using GameLibrary;
using TolsLibrary;
using System;
using System.Linq;
using GameLibrary.MenuLibrary;
using System.Collections.Generic;
using GameLibrary.SnakeGame;
using System.Threading;
using ToolsLibrary;

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

            KeyPress.Set("Snake", ConsoleKey.W, Keys[needOption.secondSnakeUp]);
            KeyPress.Set("Snake", ConsoleKey.S, Keys[needOption.secondSnakeDown]);
            KeyPress.Set("Snake", ConsoleKey.A, Keys[needOption.secondSnakeLeft]);
            KeyPress.Set("Snake", ConsoleKey.D, Keys[needOption.secondSnakeRigh]);

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
            MenuKeySet(menu);

            var buttons = MenuInicialisation(menu, menuPrinter, new string[] { "Snake start", "Settings", "Exit" });

            buttons[0].IsPressed += () =>
            {
                menuPrinter.Hide();
                void act()
                {
                    menuPrinter.Load();
                    MenuKeySet(menu);
                }
                SnakeMenuLoad(location, act);
                
            };

            buttons[1].IsPressed += () =>
            {
                menuPrinter.Hide();
                void act()
                {
                    menuPrinter.Load();
                    MenuKeySet(menu);
                }
                CommonSettings(location, act);
            };

            buttons[2].IsPressed += () => 
            { 
                menuPrinter.Close();
                location.CancelRegistration(menuPrinter);
                KeyPress.Close();
                MenuKeyClose();
                return;
            };

            menuPrinter.Load();
        }
        static void CommonSettings(IDrawningRectangle<SignConsole> location, Action startWithClose)
        {
            var menu = new KeyboardMenu<ButtonInConsole>("Settings");
            var sattingsMenu = new ConsolePrintMenu(location.Width, location.Height, location, menu);
            location.Register((0, 0), sattingsMenu, sattingsMenu.GetCoordinates());
            var printer = new BigPixelPrint(sattingsMenu.Width, 5, sattingsMenu, letters);
            sattingsMenu.Register((0, 1), printer, printer.GetCoordinates());
            sattingsMenu.SetWriter(printer);

            var buttons = SetControl("Menu", menu, sattingsMenu,
                new string[] { "Menu Up", "Menu Down", "Menu Select" },
                new needOption[] { needOption.menuUp, needOption.menuDown, needOption.menuPress});
            var b4 = new ButtonInConsole(6, 1, sattingsMenu, SignConsole.GetSignConsoles("Exit"));
            sattingsMenu.Register((5, 9 + buttons.Length * 2), b4, b4.GetCoordinates());

            menu.AddLastButton(b4);
            b4.IsPressed += () =>
            {
                sattingsMenu.Close();
                location.CancelRegistration(sattingsMenu);
                MenuKeyClose();
                startWithClose?.Invoke();
            };

            sattingsMenu.Load();
            MenuKeySet(menu);
        }
        static void SnakeMenuLoad(IDrawningRectangle<SignConsole> location, Action startWithClose)
        {
            var menu = new KeyboardMenu<ButtonInConsole>("Snake");
            var snakeMenu = new ConsolePrintMenu(location.Width, location.Height, location, menu);
            location.Register((0, 0), snakeMenu, snakeMenu.GetCoordinates());
            var printer = new BigPixelPrint(snakeMenu.Width, 5, snakeMenu, letters);
            snakeMenu.Register((0, 1), printer, printer.GetCoordinates());
            snakeMenu.SetWriter(printer);

            void act()
            {
                snakeMenu.Load();
                MenuKeySet(menu, new bool[] { false, false, true });
            }

            var buttons = MenuInicialisation(menu, snakeMenu, new string[] { "Start", "Mode", "Settings", "Exit" });
            var snakeSettings = new SnakeSettings();

            buttons[0].IsPressed += () =>
            {
                snakeMenu.Hide();
                SnakeStart(location, snakeSettings, act);
                KeyPress.SetControl("Menu");
                snakeMenu.Load();
            };

            buttons[1].IsPressed += () => 
            {
                snakeMenu.Hide();
                SnakeMode(location, act, snakeSettings);
            };

            buttons[2].IsPressed += () =>
            {
                snakeMenu.Hide();
                SnakeSettings(location, act);
            };

            buttons[3].IsPressed += () => {
                snakeMenu.Close();
                location.CancelRegistration(snakeMenu);
                MenuKeyClose();
                startWithClose?.Invoke();
            };
            act();          
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
            var buttons = SetControl("Snake", menu, sattingsMenu,
                new string[] { "Snake Up", "Snake Down", "Snake Left", "Snake Right", "Second Snake Up", "Second Snake Down", "Second Snake Left", "Second Snake Right" },
                new needOption[] { needOption.snakeUp, needOption.snakeDown, needOption.snakeLeft, needOption.snakeRigh,
                needOption.secondSnakeUp, needOption.secondSnakeDown, needOption.secondSnakeLeft, needOption.secondSnakeRigh});
            var exit = new ButtonInConsoleSetter(6, 1, sattingsMenu, SignConsole.GetSignConsoles("Exit"));
            sattingsMenu.Register((5, 9 + buttons.Length * 2), exit, exit.GetCoordinates());

            menu.AddLastButton(exit);
            exit.IsPressed += () =>
            {
                sattingsMenu.Close();
                location.CancelRegistration(sattingsMenu);
                MenuKeyClose();
                startWithClose?.Invoke();
            };

            sattingsMenu.Load();
            MenuKeySet(menu);
        }
        static void SnakeMode(IDrawningRectangle<SignConsole> location, Action startWithClose, SnakeSettings settings)
        {
            var menu = new KeyboardMenu<ButtonInConsole>("Snake Mode");
            var modeMenu = new ConsolePrintMenu(location.Width, location.Height, location, menu);
            location.Register((0, 0), modeMenu, modeMenu.GetCoordinates());
            var printer = new BigPixelPrint(modeMenu.Width, 5, modeMenu, letters);
            modeMenu.Register((0, 1), printer, printer.GetCoordinates());
            modeMenu.SetWriter(printer);
            void act()
            {
                modeMenu.Load();
                MenuKeySet(menu, new bool[] { false, false, false });
            }
            act();

            var buttons = SetMobe(menu, modeMenu, new string[]{"Start speed", "Acceleration", "Multiplayer", "Special berries", "Few special berries " });

            buttons[0].Value = SignConsole.GetSignConsoles(settings.StartSpeed.ToString());
            buttons[1].Value = SignConsole.GetSignConsoles(settings.Acceleration.ToString());
            buttons[2].Value = SignConsole.GetSignConsoles(settings.Multy.ToString());
            buttons[3].Value = SignConsole.GetSignConsoles(settings.Event.ToString());
            buttons[4].Value = SignConsole.GetSignConsoles(settings.EventMulty.ToString());
            
            buttons[0].IsPressed += () => 
            {
                settings.StartSpeed = (SnakeStartSpeed)EnumValues.Next(settings.StartSpeed);
                buttons[0].Value = SignConsole.GetSignConsoles(settings.StartSpeed.ToString());
            };

            buttons[1].IsPressed += () =>
            {
                settings.Acceleration = (SnakeAcceleration)EnumValues.Next(settings.Acceleration);
                buttons[1].Value = SignConsole.GetSignConsoles(settings.Acceleration.ToString());
            };

            buttons[2].IsPressed += () =>
            {
                settings.Multy = !settings.Multy;
                buttons[2].Value = SignConsole.GetSignConsoles(settings.Multy.ToString());
            };
            buttons[3].IsPressed += () =>
            {
                settings.Event = !settings.Event;
                buttons[3].Value = SignConsole.GetSignConsoles(settings.Event.ToString());
            };
            buttons[4].IsPressed += () =>
            {
                settings.EventMulty = !settings.EventMulty;
                buttons[4].Value = SignConsole.GetSignConsoles(settings.EventMulty.ToString());
            };

            var exit = new ButtonInConsoleSetter(6, 1, modeMenu, SignConsole.GetSignConsoles("Exit"));
            modeMenu.Register((5, 8 + buttons.Length * 2), exit, exit.GetCoordinates());
            menu.AddLastButton(exit);
            exit.IsPressed += () =>
            {
                modeMenu.Close();
                location.CancelRegistration(modeMenu);
                MenuKeyClose();
                startWithClose?.Invoke();
            };
            modeMenu.Load();
        }    
        static void SnakeStart(IDrawningRectangle<SignConsole> location, SnakeSettings settings, Action startWithClose)
        {
            settings.Event = true;
            settings.EventChance = 0.50;
            Action stop = default;
            var GameDictionary = new Dictionary<GamesSquareValues, SignConsole>
            {
                { GamesSquareValues.snake, new SignConsole(' ', ConsoleColor.White) },
                { GamesSquareValues.snakeBerry, new SignConsole(' ', ConsoleColor.Red) },
                { GamesSquareValues.snakeWall, new SignConsole('X', ConsoleColor.Black) },
                { GamesSquareValues.nothing, new SignConsole(' ', ConsoleColor.Black) },
                { GamesSquareValues.snakeEventBerry, new SignConsole(' ', ConsoleColor.Green) }
            };
            var gameField = new ConsoleGameField(location.Width, location.Height, location, GameDictionary);
            location.Register((0, 0), gameField, gameField.GetCoordinates());

            var snakeMove = new SnakeMove((int)settings.StartSpeed);

            var snakeField = new SnakeField(gameField.Width, gameField.Height - 5, gameField);
            gameField.Register((0, 5), snakeField, snakeField.GetCoordinates());
            snakeField.Inicializated();
            var bigChar = new Letters("First");
            BigPixelPrint scoreField, scoreField2 = null;
            Score score, score2 = null;
            KeyPress.SetControl("Snake");
            Snake snake, snake2 = null;
            if (!settings.Multy)
            {
               
                snake = new Snake(snakeField);
                scoreField = new BigPixelPrint(18, 5, location, bigChar);
                location.Register((location.Width / 2 - 9, 0), scoreField, scoreField.GetCoordinates());
                score = new Score(scoreField); 
                
            }
            else
            {
                snake = new Snake(snakeField, (snakeField.Width / 3, snakeField.Height / 2), Direction.direction.up);
                scoreField = new BigPixelPrint(18, 5, location, bigChar);
                location.Register((location.Width / 2 - 20, 0), scoreField, scoreField.GetCoordinates());
                score = new Score(scoreField);

                snake2 = new Snake(snakeField, (snakeField.Width / 3 * 2, snakeField.Height / 2), Direction.direction.down);
                scoreField2 = new BigPixelPrint(18, 5, location, bigChar);
                location.Register((location.Width / 2 + 2, 0), scoreField2, scoreField2.GetCoordinates());
                score2 = new Score(scoreField2);
                Berry.RandomBerry(snakeField);
            }
            Berry.RandomBerry(snakeField);

            snake.IsEat += (Snake) => Berry.RandomBerry(Snake.Location);
            snake.IsEat += (Snake) => snakeMove.Acceleration(((double)settings.Acceleration) / 100.0);
            snake.IsEat += (Snake) => score.Add();
            if (settings.Event)
            {
                snake.IsVeryEat += (Snake) => snakeMove.Acceleration(((double)settings.Acceleration) / 100.0);
                snake.IsVeryEat += (Snake) => score.Add(5);
                snake.IsEat += (Snake) => EventBerry.RandomEventBerry(settings.EventMulty, snakeField, settings.EventChance, snakeMove, 50);
            }
            snake.Die += (Snake) => stop?.Invoke();
            snakeMove.Add(snake.Move);
            stop += () => snakeMove.Stop();
            stop += () => KeyPress.ResetControl();
            

            Keys[needOption.snakeUp].Set((obj, ar) => snake.Up());
            Keys[needOption.snakeDown].Set((obj, ar) => snake.Down());
            Keys[needOption.snakeLeft].Set((obj, ar) => snake.Left());
            Keys[needOption.snakeRigh].Set((obj, ar) => snake.Right());
            if (settings.Multy)
            {
                snake2.IsEat += (Snake) => Berry.RandomBerry(Snake.Location);
                snake2.IsEat += (Snake) => snakeMove.Acceleration((double)settings.Acceleration / 100);
                snake2.IsEat += (Snake) => score2.Add();
                if (settings.Event)
                {
                    snake2.IsVeryEat += (Snake) => snakeMove.Acceleration(((double)settings.Acceleration) / 100.0);
                    snake2.IsVeryEat += (Snake) => score2.Add(5);
                }
                snake2.Die += (Snake) => stop?.Invoke();
                snakeMove.Add(snake2.Move);

                Keys[needOption.secondSnakeUp].Set((obj, ar) => snake2.Up());
                Keys[needOption.secondSnakeDown].Set((obj, ar) => snake2.Down());
                Keys[needOption.secondSnakeLeft].Set((obj, ar) => snake2.Left());
                Keys[needOption.secondSnakeRigh].Set((obj, ar) => snake2.Right());
            }

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
            if (settings.Multy)
            {
                Keys[needOption.secondSnakeUp].Set(null);
                Keys[needOption.secondSnakeDown].Set(null);
                Keys[needOption.secondSnakeLeft].Set(null);
                Keys[needOption.secondSnakeRigh].Set(null);
            }
            Console.ReadKey(true);
            void exit()
            {
                snakeMove.Stop();
                gameField.Close();
                scoreField.Close();
                scoreField2?.Close();
                location.CancelRegistration(gameField);
                startWithClose();
                EventBerry.Clear(snakeField);
            }
            exit();
        }      
        static void Pause(IDrawningRectangle<SignConsole> location, Action contin, Action exit)
        {
            var menu = new KeyboardMenu<ButtonInConsole>("Pause");
            var pauseMenu = new ConsolePrintMenu(location.Width / 2, location.Height / 3 * 2, location, menu);
            location.Register((location.Width / 4, location.Height / 6), pauseMenu, pauseMenu.GetCoordinates());
            var printer = new BigPixelPrint(pauseMenu.Width - 4, 5, pauseMenu, letters);          
            pauseMenu.Register((2, 2), printer, printer.GetCoordinates());
            pauseMenu.SetWriter(printer);
            pauseMenu.Frame(new SignConsole('0'));
           
            MenuKeySet(menu);

            var pauseButtons = MenuInicialisation(menu, pauseMenu, new string[] { "Continue", "Settings", "Exit"});

            pauseButtons[0].IsPressed += () =>
            {
                pauseMenu.Close();
                location.CancelRegistration(pauseMenu);
                MenuKeyClose();
                contin?.Invoke();
            };

            pauseButtons[1].IsPressed += () =>
            {
                pauseMenu.Hide();
                void act()
                {
                    location.Load();
                    MenuKeySet(menu);
                }
                SnakeSettings(location, act, true);
            };

            pauseButtons[2].IsPressed += () =>
            {
                pauseMenu.Close();
                location.CancelRegistration(pauseMenu);
                MenuKeyClose();
                exit?.Invoke();
            };
            pauseMenu.Fill(new SignConsole(' '));
            pauseMenu.Load();
        }



        static ButtonInConsole[] MenuInicialisation(KeyboardMenu<ButtonInConsole> menu, IDrawningRectangle<SignConsole> menuPrinter, string[] buttonsName)
        {
            var buttons = new ButtonInConsole[buttonsName.Length];
            for(int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = new ButtonInConsole(menuPrinter.Width - 10, 1, menuPrinter, SignConsole.GetSignConsoles(buttonsName[i]));
                menuPrinter.Register((5, 8 + i * 2), buttons[i], buttons[i].GetCoordinates());
                menu.AddLastButton(buttons[i]);
            }
            return buttons;
        }
        static void MenuKeySet(KeyboardMenu<ButtonInConsole> menu, bool[] mul = null)
        {
            KeyPress.SetControl("Menu");

            Keys[needOption.menuDown].Set((obj, ar) => menu.Next(), mul != null && mul[0]);
            Keys[needOption.menuUp].Set((obj, ar) => menu.Previous(), mul != null && mul[1]);
            Keys[needOption.menuPress].Set((obj, ar) => menu.Press(), mul != null && mul[2]);
        }
        static void MenuKeyClose()
        {
            Keys[needOption.menuDown].Set(null);
            Keys[needOption.menuUp].Set(null);
            Keys[needOption.menuPress].Set(null);
        }
        static ButtonInConsoleSetter[] SetControl(string name, KeyboardMenu<ButtonInConsole> menu, ConsolePrintMenu printer, string[] buttonsName, needOption[] buttonsOptions)
        {
            if(buttonsName.Length != buttonsOptions.Length)
            {
                throw new Exception();
            }
            var buttons = new ButtonInConsoleSetter[buttonsName.Length];
            for(int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = new ButtonInConsoleSetter(printer.Width - 10, 1, printer, SignConsole.GetSignConsoles(buttonsName[i]));
                printer.Register((5, 8 + i * 2), buttons[i], buttons[i].GetCoordinates());
                menu.AddLastButton(buttons[i]);
                var option = buttonsOptions[i];
                buttons[i].IsPressed += () =>
                {
                    var newKey = Console.ReadKey(true);
                    KeyPress.Remove(name, KeyPress.GetKey(name, Keys[option]));
                    KeyPress.Reset(name, newKey.Key, Keys[option]);
                    setValue();
                };

            }
            setValue();
            void setValue()
            {
                for(int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].Value = SignConsole.GetSignConsoles(KeyPress.GetKey(name, Keys[buttonsOptions[i]]).ToString());
                }
            }
            return buttons;
        }
        static ButtonInConsoleSetter[] SetMobe(KeyboardMenu<ButtonInConsole> menu, ConsolePrintMenu printer, string[] buttonsName)
        {
            var buttons = new ButtonInConsoleSetter[buttonsName.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = new ButtonInConsoleSetter(printer.Width - 10, 1, printer, SignConsole.GetSignConsoles(buttonsName[i]));
                printer.Register((5, 8 + i * 2), buttons[i], buttons[i].GetCoordinates());
                menu.AddLastButton(buttons[i]);
            }
            return buttons;
        }
    }
}
