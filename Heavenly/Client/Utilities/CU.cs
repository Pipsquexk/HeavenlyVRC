using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using VRC.Core;
using UnityEngine;
using MelonLoader;
using Newtonsoft.Json;

using Heavenly.Client.API;
using Heavenly.VRChat.Utilities;


namespace Heavenly.Client.Utilities
{
    public static class CU
    {

        private static Il2CppSystem.Collections.Generic.List<ApiAvatar> resAvis = new Il2CppSystem.Collections.Generic.List<ApiAvatar>();
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
                File.WriteAllText("Heavenly\\Configs\\Keybindings.cfg", JsonConvert.SerializeObject(new KeyConfig() { FlyKey = "F", EarrapeKey = "E", RejoinKey = "R" }));
                File.WriteAllText("Heavenly\\Configs\\Notifications.cfg", JsonConvert.SerializeObject(new NotifConfig() { Voice = "Male", UseNotifs = true }));
                using (WebClient client = new WebClient())
                {
                    client.DownloadFileAsync(new Uri("https://www.heavenlyclient.com/Notifs.hev"), "Heavenly\\Assets\\Notifs.hev");
                }
            }

            if (!File.Exists("Heavenly\\Configs\\Keybindings.cfg"))
            {
                File.WriteAllText("Heavenly\\Keybindings.cfg", JsonConvert.SerializeObject(new KeyConfig() { FlyKey = "F", EarrapeKey = "E", RejoinKey = "R" }));
                File.WriteAllText("Heavenly\\Notifications.cfg", JsonConvert.SerializeObject(new NotifConfig() { Voice = "Male", UseNotifs = true }));
            }

            Main.kConfig = JsonConvert.DeserializeObject<KeyConfig>(File.ReadAllText("Heavenly\\Configs\\Keybindings.cfg"));
            Main.nConfig = JsonConvert.DeserializeObject<NotifConfig>(File.ReadAllText("Heavenly\\Configs\\Notifications.cfg"));


            Console.SetCursorPosition(0, top);

            Console.WriteLine("Doing actual client stuff and things...");
            Console.Write("[===================     ]");

            Main.notifBundle = AssetBundle.LoadFromFile("Heavenly\\Assets\\Notifs.hev");
            Main.otherBundle = AssetBundle.LoadFromFile("Heavenly\\Assets\\Other.hev");


            Console.SetCursorPosition(0, top);

            Console.WriteLine("Done!                                      ");
            Console.Write("[========================]\n");

            Log("Started Heavenly!");

            Console.Clear();
        }

        public static IEnumerator AddAvatarToRes(List<HevApiAvatar> avatars, string name)
        {
            foreach (HevApiAvatar avi in avatars)
            {
                resAvis.Add(avi.ToApiAvatar());
                yield return null;
            }

            Main.searchList.vrcAvatarList.Method_Protected_Void_List_1_T_Int32_Boolean_VRCUiContentButton_0<ApiAvatar>(resAvis);

            Main.searchList.text.text = $"{name} - {resAvis.Count} Results for {name}";
        }

        public static async void SearchAvatars(string name)
        {
            resAvis.Clear();

            Main.searchList.text.text = $"{name} - Loading results for {name}...";

            Main.searchList.vrcAvatarList.enabled = true;


            List<HevApiAvatar> hevAvatars;

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrEmpty(name) && name.Length > 2)
            {
                using (WebClient client = new WebClient())
                {
                    var jsonString = await client.DownloadStringTaskAsync($"https://www.heavenlyclient.com/api/avatars?name={name}");
                    hevAvatars = JsonConvert.DeserializeObject<List<HevApiAvatar>>(jsonString);
                }
            }
            else
            {
                hevAvatars = new List<HevApiAvatar>();
            }

            MelonCoroutines.Start(AddAvatarToRes(hevAvatars, name));
        }


        public static void RestartGame()
        {
            Process.Start("VRChat.exe", Environment.CommandLine.ToString());
            Process.GetCurrentProcess().Kill();
        }

        public static void RestartRejoinGame()
        {
            Process.Start("VRChat.exe", Environment.CommandLine.ToString() + $" vrchat://launch/?ref=vrchat.com&id={WU.BuildInstanceID()}");
            Process.GetCurrentProcess().Kill();
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
