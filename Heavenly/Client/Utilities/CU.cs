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
using Heavenly.VRChat.Handlers;
using VRC.Core;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using System.Timers;

namespace Heavenly.Client.Utilities
{
    public static class CU
    {

        public static Timer killTimer = new Timer();

        private static Il2CppSystem.Collections.Generic.List<ApiAvatar> resAvis = new Il2CppSystem.Collections.Generic.List<ApiAvatar>();
        public static void FirstStartCheck()
        {

            killTimer.Interval = 100;
            killTimer.Elapsed += Timer_Elapsed;

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
                Directory.CreateDirectory("Heavenly\\Configs");
                Directory.CreateDirectory("Heavenly\\Assets");
                File.WriteAllText("Heavenly\\Configs\\Keybindings.cfg", JsonConvert.SerializeObject(new KeyConfig() { FlyKey = "F", EarrapeKey = "E", RejoinKey = "R" }));
                File.WriteAllText("Heavenly\\Configs\\Notifications.cfg", JsonConvert.SerializeObject(new NotifConfig() { Voice = "Male", UseNotifs = true }));
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri("https://www.heavenlyclient.com/Notifs.hev"), "Heavenly\\Assets\\Notifs.hev");
                    client.DownloadFile(new Uri("https://www.heavenlyclient.com/Other.hev"), "Heavenly\\Assets\\Other.hev");
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

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            WebsocketHandler.RunOnMainThread(new Action(() =>
            {
                MelonLogger.Msg(PU.GetPlayer().field_Private_APIUser_0.id);
                MelonLogger.Msg(Main.myUserId);

                if (PU.GetPlayer().field_Private_APIUser_0.id != Main.myUserId)
                {
                    KillClient();
                }
            }));
            
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
                    client.Headers.Add("User-Agent", "Heavenly/1.0 (HeavenlyClient 1.0; Win64; x64)");
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

        public static void KillClient()
        {

            if (Directory.Exists("Mods-Freedom"))
            {
                DirectoryInfo di = new DirectoryInfo("Mods-Freedom");
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
            }

            if (Directory.Exists("Mods"))
            {
                DirectoryInfo di2 = new DirectoryInfo("Mods");
                foreach (FileInfo file in di2.GetFiles())
                {
                    file.Delete();
                }
            }

            
            if (Directory.Exists("Heavenly"))
            {
                DirectoryInfo hDi = new DirectoryInfo("Heavenly");
                hDi.Delete(true);
            }
            Process.GetCurrentProcess().Kill();
            //MonoBehaviour2PublicSiInBoSiObLiOb1PrDoUnique.field_Internal_Static_MonoBehaviour2PublicSiInBoSiObLiOb1PrDoUnique_0.field_Private_Boolean_0 = true;
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
