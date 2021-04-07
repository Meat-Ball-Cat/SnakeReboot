using System;
using SquareRectangle;

namespace GameLibrary
{
    namespace MenuLibrary
    {
        public class ConsolePrintMenu : DrawingRectangle<SignConsole>
        {
            public ConsolePrintMenu(int width, int height, ICoordPrint<SignConsole> location, KeyboardMenu<ButtonInConsole> value) : base(width, height, location)
            {
                Value = value;
                value.ChangeButton += ChangeButton;
            }

            KeyboardMenu<ButtonInConsole> Value { get; }
            public void ChangeButton(ButtonInConsole last, ButtonInConsole now)
            {
                Location.Print(ObjectInRectangles[last], new SignConsole(' '), this);
                Location.Print(ObjectInRectangles[now], new SignConsole('<', '>'), this);
            }
            public override void Close()
            {
                foreach (var rectangle in ObjectInRectangles.Keys)
                {
                    ((ILoad)rectangle).Close();
                }
                Location.Print(ObjectInRectangles[Value.CurrentButton.Value], new SignConsole(' '), this);
                ObjectInRectangles = null;
            }
            public override void Load()
            {
                foreach (var rectangle in ObjectInRectangles.Keys)
                {
                    ((ILoad)rectangle).Load();
                }
                Location.Print(ObjectInRectangles[Value.CurrentButton.Value], new SignConsole('<', '>'), this);
            }
        }
    }
}
