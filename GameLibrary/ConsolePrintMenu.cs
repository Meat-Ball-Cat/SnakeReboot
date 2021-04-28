using System;
using SquareRectangle;

namespace GameLibrary
{
    namespace MenuLibrary
    {
        public class ConsolePrintMenu : DrawingRectangle<SignConsole>
        {
            public ConsolePrintMenu(int width, int height, IDrawingByCoordinates<SignConsole> location, KeyboardMenu<ButtonInConsole> menu) : base(width, height, location)
            {
                Menu = menu;
                menu.ChangeButton += ChangeButton;
            }
            public void SetWriter(ConsoleWriter writer)
            {
                Writer?.Close();
                Writer = writer;
                Writer.Load();
            }

            KeyboardMenu<ButtonInConsole> Menu { get; }
            public void ChangeButton(ButtonInConsole last, ButtonInConsole now)
            {
                RemoveButton(last);
                SetButton(now);
            }
            void SetButton(ButtonInConsole button)
            {
                button.SetColors(ConsoleColor.White, ConsoleColor.Black);
            }
            void RemoveButton(ButtonInConsole button)
            {
                button.SetColors(ConsoleColor.Black, ConsoleColor.White);
            }
            ConsoleWriter Writer { get; set; }
            public override void Close()
            {
                if (ObjectInRectangles != null)
                {
                    foreach (var rectangle in ObjectInRectangles.Keys)
                    {
                        ((ILoad)rectangle).Close();
                    }
                }
            }
            public override void Load()
            {
                foreach (var rectangle in ObjectInRectangles.Keys)
                {
                    ((ILoad)rectangle).Load();
                }
                Writer?.WriteLine(Menu.Name);
                if (Menu.CurrentButton != null)
                {
                    SetButton(Menu.CurrentButton.Value);
                }
            }
        }
    }
}
