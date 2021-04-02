using System;
using System.Collections.Generic;
using System.Threading;

namespace TolsLibrary
{
    public static class KeyPress
    {
        static Thread keyPressThread;
        static Dictionary<ConsoleKey, EventHandler> keyPressEvent = new Dictionary<ConsoleKey, EventHandler>();
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
                keyPressEvent.TryGetValue(PressedKey.Key, out var keyEventHandler);
                keyEventHandler?.Invoke(PressedKey, new EventArgs());
            }
        }
        public static bool Set(ConsoleKey keyValue, EventHandler action)
        {
            try
            {
                if (!keyPressEvent.ContainsKey(keyValue))
                {
                    keyPressEvent.Add(keyValue, action);
                    return true;
                }
                else
                {
                    return Reset(keyValue, action);
                }
            }
            catch
            {
                return false;
            }
        }
        public static bool Reset(ConsoleKey keyValue, EventHandler action)
        {
            try
            {
                keyPressEvent.Remove(keyValue);
                keyPressEvent.Add(keyValue, action);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
