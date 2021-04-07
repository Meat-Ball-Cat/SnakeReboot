using System;
using System.Runtime.InteropServices;

namespace ToolsLibrary
{
    public class FullScreen
    {
        internal static class DllImports
        {
            [DllImport("kernel32.dll")]
            public static extern IntPtr GetStdHandle(int handle);
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetConsoleDisplayMode(
                IntPtr ConsoleOutput
                , uint Flags
                , out Coord NewScreenBufferDimensions
                );
        }
        public static void FullScreenOn()
        {
            IntPtr ThisConsole = DllImports.GetStdHandle(-11);
            Coord xy;
            DllImports.SetConsoleDisplayMode(ThisConsole, 1, out xy);

            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight + 1);
            Console.CursorVisible = false;
        }
    }
}
