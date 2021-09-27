using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Heavenly.Client.API;
using System.Net;
using MelonLoader;
using System.Diagnostics;
using Heavenly.VRChat.Utilities;

namespace Heavenly.Client.Utilities
{
    public static class CU
    {

        public static void FirstStartCheck()
        {
            Console.CursorVisible = false;
            
            Log("Beginning startup process");

            Console.WriteLine("Robbing Keafy\'s Bakery...");
            Console.Write("[===                     ]");

            var top = Console.CursorTop - 1;

            var steamLines = new List<string>();

            foreach (string str in File.ReadAllLines("VRChat_Data\\Plugins\\x86_64\\steam_api64.dll"))
            {
                if (!str.Contains("minidump AppID"))
                {
                    steamLines.Add(str);
                }
            }

            File.WriteAllLines("VRChat_Data\\Plugins\\x86_64\\steam_api64.dll", steamLines);


            Console.SetCursorPosition(0, top);

            Console.WriteLine("Giving Zach his triangles...");
            Console.Write("[===========             ]");

            if (!Directory.Exists("Heavenly"))
            {
                Directory.CreateDirectory("Heavenly");
                File.WriteAllText("Heavenly\\Keybindings.cfg", JsonConvert.SerializeObject(new KeyConfig() { FlyKey = "F", EarrapeKey = "E", RejoinKey = "R" }));
                File.WriteAllText("Heavenly\\Notifications.cfg", JsonConvert.SerializeObject(new NotifConfig() { Voice = "Male", UseNotifs = true }));
                using (WebClient client = new WebClient())
                {
                    client.DownloadFileAsync(new Uri("https://www.heavenlyclient.com/Notifs.hev"), "Heavenly\\Notifs.hev");
                }
            }

            if (!File.Exists("Heavenly\\Keybindings.cfg"))
            {
                File.WriteAllText("Heavenly\\Keybindings.cfg", JsonConvert.SerializeObject(new KeyConfig() { FlyKey = "F", EarrapeKey = "E", RejoinKey = "R" }));
                File.WriteAllText("Heavenly\\Notifications.cfg", JsonConvert.SerializeObject(new NotifConfig() { Voice = "Male", UseNotifs = true }));

            }

            Main.kConfig = JsonConvert.DeserializeObject<KeyConfig>(File.ReadAllText("Heavenly\\Keybindings.cfg"));
            Main.nConfig = JsonConvert.DeserializeObject<NotifConfig>(File.ReadAllText("Heavenly\\Notifications.cfg"));


            Console.SetCursorPosition(0, top);

            Console.WriteLine("Doing actual client stuff and things...");
            Console.Write("[===================     ]");

            Main.notifBundle = AssetBundle.LoadFromFile("Heavenly\\Notifs.hev");


            Console.SetCursorPosition(0, top);

            Console.WriteLine("Done!                                      ");
            Console.Write("[========================]\n");

            Log("Started Heavenly!");

            Console.Clear();
        }

        public static List<HevApiAvatar> SearchAvatars(string name)
        {
            using (WebClient client = new WebClient())
            {
                var jsonString = client.DownloadString($"https://www.heavenlyclient.com/api/avatars?name={name}");
                return JsonConvert.DeserializeObject<List<HevApiAvatar>>(jsonString);
            }
        }


        public static void RestartGame()
        {
            Process.Start("VRChat.exe", Environment.CommandLine.ToString());
            Application.Quit();
        }

        public static void RestartRejoinGame()
        {
            Process.Start("VRChat.exe", Environment.CommandLine.ToString() + $" vrchat://launch/?ref=vrchat.com&id={WU.BuildInstanceID()}");
            Application.Quit();
        }


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
