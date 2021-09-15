using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heavenly
{
    public static class CU
    {
        public static void Log(string txt)
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(DateTime.Now.ToString("HH:mm:ss.fff"));
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("]");

            Console.Write(" [");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("Heavenly");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"] {txt}\n");
        }

        public static void Log(ConsoleColor color, string txt)
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(DateTime.Now.ToString("HH:mm:ss.fff"));
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("]");

            Console.Write(" [");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("Heavenly");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("]");

            Console.ForegroundColor = color;
            Console.Write($" {txt}\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
