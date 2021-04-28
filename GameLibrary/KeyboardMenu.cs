using System;
using System.Collections.Generic;

namespace GameLibrary
{
    namespace MenuLibrary 
    { 
        public delegate void ChangeButton<T>(T last, T now) where T : IButton;
        interface IMenu<T> where T : IButton
        {
            void Press();
            void Next();
            void Previous();
            void AddLastButton(T button);
        }      
        public class KeyboardMenu<T> : IMenu<T> where T : IButton
        {
            public string Name { get; }
            LinkedList<T> Buttons { get; }
            public LinkedListNode<T> CurrentButton { get; set; }
            bool Curcule { get; set; }
            public event ChangeButton<T> ChangeButton;
            public KeyboardMenu(string name, bool curcule = false)
            {
                Buttons = new LinkedList<T>();
                Name = name;
                Curcule = curcule;
            }
            public void Press() => CurrentButton?.Value.Press();
            public void Next()
            {
                var last = CurrentButton.Value;
                if (CurrentButton.Next != null)
                {                   
                    CurrentButton = CurrentButton.Next;   
                }
                else if (Curcule)
                {
                    CurrentButton = CurrentButton.List.First;
                }
                ChangeButton.Invoke(last, CurrentButton.Value);
            }
            public void Previous()
            {
                var last = CurrentButton.Value;
                if (CurrentButton.Previous != null)
                {
                    CurrentButton = CurrentButton.Previous;
                }
                else if (Curcule)
                {
                    CurrentButton = CurrentButton.List.Last;
                }
                ChangeButton.Invoke(last, CurrentButton.Value);
            }
            public void AddLastButton(T button)
            {
                Buttons.AddLast(button);
                CurrentButton = Buttons.First;
            }

        }
    }
}
