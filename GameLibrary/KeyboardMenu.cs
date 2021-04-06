using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SquareRectangle;
using System.Threading.Tasks;

namespace GameLibrary
{
    namespace MenuLibrary
    {
        public class KeyboardMenu<T> where T : IButton
        {
            public string Name { get; }
            public LinkedList<T> Buttons { get; }
            public LinkedListNode<T> CurrentButton { get; set; }
            bool Curcule { get; set; }
            public KeyboardMenu(string name, bool curcule = false)
            {
                Buttons = new LinkedList<T>();
                Name = name;
                Curcule = curcule;
            }
            public void Press() => CurrentButton?.Value.Press();
            public void Next()
            {
                CurrentButton.Value.NotCurrent();
                if (CurrentButton.Next != null)
                {                   
                    CurrentButton = CurrentButton.Next;   
                }
                else if (Curcule)
                {
                    CurrentButton = CurrentButton.List.First;
                }
                CurrentButton.Value.IsCurrent();
            }
            public void Previous()
            {
                CurrentButton.Value.NotCurrent();
                if (CurrentButton.Previous != null && Curcule)
                {
                    CurrentButton = CurrentButton.Previous;
                }
                else if (Curcule)
                {
                    CurrentButton = CurrentButton.List.Last;
                }
                CurrentButton.Value.IsCurrent();
            }
            public void AddLastButton(T button)
            {
                Buttons.AddLast(button);
            }

        }
    }
}
