using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TolsLibrary
{
    public class Event
    {
        bool Multy { get; set; }
        public EventHandler Handler { get; private set; }
        public Event(EventHandler handler, bool multy = false)
        {
            Handler = handler;
            Multy = multy;
        }
        public void Set(EventHandler handler, bool multy = false)
        {
            Handler = handler;
            Multy = multy;
        }
        public void BeginInvoke(object sender)
        {
            if (Multy)
            {
                Handler?.BeginInvoke(sender, new EventArgs(), null, null);
            }
            else
            {
                Handler?.Invoke(sender, new EventArgs());
            }
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Event))
            {
                return Handler == ((Event)obj).Handler;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Handler.GetHashCode();
        }
    }
    public static class KeyPress
    {
        static Thread keyPressThread;
        static string _currentPresetName;
        static string CurrentPresetName
        {
            get
            {
                return _currentPresetName;
            }
            set
            {
                if (Presets.ContainsKey(value))
                {
                    _currentPresetName = value;
                }
            }
        }
        public static Dictionary<ConsoleKey, Event> CurrentControl { get { return CurrentPresetName == null ? null : Presets[CurrentPresetName]; } }
        static Dictionary<string, Dictionary<ConsoleKey, Event>> Presets = new Dictionary<string, Dictionary<ConsoleKey, Event>>();
        public static bool SetControl(string presetName)
        {
            if (Presets.ContainsKey(presetName))
            {
                CurrentPresetName = presetName;
                return true;
            }
            return false;
        }
        public static void ResetControl()
        {
            _currentPresetName = null;
        }
        public static bool AddControl(string contrilName)
        {
            if (!Presets.ContainsKey(contrilName))
            {
                Presets.Add(contrilName, new Dictionary<ConsoleKey, Event>());
                return true;
            }
            return false;
        }
        public static void Start()
        {
            keyPressThread = new Thread(WaitingForPress);
            keyPressThread.Start();
        }
        public static void Close()
        {
            if (keyPressThread.IsAlive)
            {
                keyPressThread.Abort();
            }
        }
        static void WaitingForPress()
        {
            while (true)
            {
                var PressedKey = Console.ReadKey(true);
                Event keyEventHandler = null;
                CurrentControl?.TryGetValue(PressedKey.Key, out keyEventHandler);
                keyEventHandler?.BeginInvoke(PressedKey);
            }
        }
        public static ConsoleKey GetKey(string name, Event value)
        {
            if (Presets.ContainsKey(name))
            {
                var now = Presets[name];
                var result = now.Where(p => p.Value == value).Select(p => p.Key).ToArray();
                return result.Length == 1 ? result[0] : default;
            }
            return default;
        }
        public static void Set(string name, ConsoleKey keyValue, Event action)
        {
            if (Presets.ContainsKey(name))
            {
                var current = Presets[name];
                if (!current.ContainsKey(keyValue))
                {
                    current.Add(keyValue, action);
                }
            }
        }
        public static void Set(ConsoleKey keyValue, Event action)
        {
            if (!CurrentControl.ContainsKey(keyValue))
            {
                CurrentControl.Add(keyValue, action); 
            }
        }

        public static void Reset(string name, ConsoleKey keyValue, Event action)
        {
            if (Presets.ContainsKey(name))
            {
                var current = Presets[name];
                if (current.ContainsKey(keyValue))
                {
                    current.Remove(keyValue);
                }
                current.Add(keyValue, action);
            }
        }
        public static void Reset(ConsoleKey keyValue, Event action)
        {
            if (CurrentControl.ContainsKey(keyValue))
            {
                CurrentControl.Remove(keyValue);
            }
            CurrentControl.Add(keyValue, action);
        }
        public static void Remove(string name, ConsoleKey keyValue)
        {
            if (Presets.ContainsKey(name))
            {
                var current = Presets[name];
                if (current.ContainsKey(keyValue))
                {
                    current.Remove(keyValue);
                }
            }
        }
        public static void Remove(ConsoleKey keyvalue)
        {
            if (CurrentControl.ContainsKey(keyvalue))
            {
                CurrentControl.Remove(keyvalue);
            }
        }
    }
}
