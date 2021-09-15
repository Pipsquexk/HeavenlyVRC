using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using MelonLoader;

namespace Heavenly
{
    public class Main : MelonMod
    {
        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            Console.Clear();
            MelonUtils.SetConsoleTitle("Heavenly");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("                   ,                                 _        ");
            Console.WriteLine("                  /|   |                            | |       ");
            Console.WriteLine("                   |___|  _   __,        _   _  _   | |       ");
            Console.WriteLine("                   |   |\\|/  /  |  |  |_|/  / |/ |  |/  |   | ");
            Console.WriteLine("                   |   |/|__/\\_/|_/ \\/  |__/  |  |_/|__/ \\_/|/");
            Console.WriteLine("                                                           /| ");
            Console.WriteLine("                                                           \\| ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("                                    I\'m back");
            Console.WriteLine(" ");
            Console.WriteLine("                        ================================");
            Console.WriteLine("                        ||                            ||");
            Console.WriteLine("                        ||                            ||");
            Console.WriteLine("                        ||                            ||");
            Console.WriteLine("                        ||                            ||");
            Console.WriteLine("                        ||                            ||");
            Console.WriteLine("                        ||                            ||");
            Console.WriteLine("                        ||                            ||");
            Console.WriteLine("                        ||                            ||");
            Console.WriteLine("                        ||                            ||");
            Console.WriteLine("                        ||                            ||");
            Console.WriteLine("                        ||                            ||");
            Console.WriteLine("                        ||                            ||");
            Console.WriteLine("                        ||                            ||");
            Console.WriteLine("                        ||                            ||");
            Console.WriteLine("                        ================================");
            Patches.ApplyPatches();
            MelonLogger.Msg("Fuck you");
        }
    }
}
