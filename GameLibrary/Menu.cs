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
        public abstract class Menu<T> where T : IButton 
        {
            public string Name { get; }
            protected LinkedList<T> Buttons { get; }
            protected LinkedListNode<T> CurrentButton { get; set; }
            bool Curcule { get; set; }
            public Menu(string name, bool curcule = false)
            {
                Buttons = new LinkedList<T>();
                Name = name;
                Curcule = curcule;
            }
            public void Press() => CurrentButton?.Value.Press();
            public void Next()
            {
                if(CurrentButton.Next != null && Curcule)
                {
                    CurrentButton = CurrentButton.Next;
                }
                else if (Curcule)
                {
                    CurrentButton = CurrentButton.List.First;
                }
            }
            public void Previous()
            {
                if (CurrentButton.Previous != null && Curcule)
                {
                    CurrentButton = CurrentButton.Previous;
                }
                else if (Curcule)
                {
                    CurrentButton = CurrentButton.List.Last;
                }
            }

        }
    }
}
