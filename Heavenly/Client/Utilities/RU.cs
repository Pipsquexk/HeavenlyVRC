using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using VRC.Core;

namespace Heavenly.Client.Utilities
{
    public static class RU
    {

        private static ApiAvatar avi = null;


        private static string currentAviName = "NULL";
        private static string currentAviPath = "NULL";
        private static string currentOldId = "NULL";

        private static string newAviID = "NULL";
        private static string friendlyName = "NULL";

        //avtr_1b6aa5e1-b76a-463c-9e5e-67ff2ad0eda6 
        public static async void ReuploadAvatar(ApiAvatar avatar)
        {
            avi = avatar;
            await DownloadAvatar(avatar);

        }

        public static async Task DownloadAvatar(ApiAvatar avatar)
        {

            if (!avatar.id.Contains("avtr_"))
            {
                CU.Log("Can't reupload this avatar due to a bad Avatar ID");
                return;
            }

            newAviID = GenerateAvatarID();
            VRC.Core.API.Fetch<ApiAvatar>(newAviID, DelegateSupport.ConvertDelegate<Il2CppSystem.Action<ApiContainer>>(new System.Action<ApiContainer>((e) => { CU.Log("Can't reupload this avatar due to a bad Avatar ID"); return; })));

            CU.Log(ConsoleColor.Green, "Downloading avatar....");

            if (!Directory.Exists("Heavenly\\Temp"))
            {
                Directory.CreateDirectory("Heavenly\\Temp");
            }

            WebClient client = new WebClient();
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.164 Safari/537.36 OPR/77.0.4054.298");
            client.Headers.Add("Cookie", "auth=" + ApiCredentials.authToken);

            currentAviPath = $"current";
            currentOldId = avatar.id;

            await client.DownloadFileTaskAsync(avatar.assetUrl, $"Heavenly\\Temp\\current");

            CU.Log(ConsoleColor.Green, "Downloaded avatar!");

            DecompressAvatar(currentAviPath);
        }

        public static void DecompressAvatar(string filePath)
        {

            foreach (string str in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                CU.Log(str);
            }

            CU.Log(ConsoleColor.Yellow, "Decompressing avatar...");

            Process.Start("Heavenly\\Temp\\UBPU.exe", @"""Heavenly\Temp\" + filePath + @"""");

            MelonLoader.MelonLogger.Msg(@"""Heavenly\Temp\" + filePath + @"""");

            CU.Log(ConsoleColor.Yellow, "Decompressed avatar!");

            ChangeAvatarID(currentOldId, newAviID);
        }

        public static async void ChangeAvatarID(string oldId, string newId)
        {
            CU.Log(ConsoleColor.Cyan, "Changing avatar ID...");

            while (!Directory.Exists($"Heavenly\\Temp\\{currentAviPath}_dump\\"))
            {
                await Task.Delay(100);
            }


            var dumpFile = "NULL";

            DirectoryInfo di = new DirectoryInfo($"Heavenly\\Temp\\{currentAviPath}_dump\\");


            foreach (FileInfo file in di.GetFiles())
            {
                if (!file.Name.Contains(".resS"))
                {
                    dumpFile = file.FullName;
                }
            }

            await Task.Delay(2000);


            byte[] array = File.ReadAllBytes(dumpFile);
            await Task.Delay(2000);
            byte[] bytes = Encoding.ASCII.GetBytes(oldId);
            Encoding.ASCII.GetBytes(oldId.ToLower());
            byte[] bytes2 = Encoding.ASCII.GetBytes(newId);
            byte[] array2 = new byte[array.Length + bytes2.Length - bytes.Length];
            byte[] array3 = array;
            int num2;
            while ((num2 = RenameBytes(array3, bytes)) >= 0)
            {
                Buffer.BlockCopy(array3, 0, array2, 0, num2);
                Buffer.BlockCopy(bytes2, 0, array2, num2, bytes2.Length);
                Buffer.BlockCopy(array3, num2 + bytes.Length, array2, num2 + bytes2.Length, array3.Length - num2 - bytes.Length);
                array3 = array2;
            }
            await Task.Delay(2000);
            File.WriteAllBytes(dumpFile, array2);
            await Task.Delay(2000);
            CU.Log(ConsoleColor.Cyan, "Changed avatar ID!");

            friendlyName = "Avatar - " + currentAviName + " - Asset bundle - " + avi.unityVersion.ToLower() + "_" + ApiWorld.VERSION.ApiVersion.ToString().ToLower() + "_" + avi.platform.ToLower() + "_Release";

            RecompressAvatar(currentAviPath, dumpFile);
        }

        public static async void RecompressAvatar(string filePath, string dumpFile)
        {

            await Task.Delay(2000);

            CU.Log(ConsoleColor.Blue, "Recompressing avatar...");


            var p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "D:\\Steam\\steamapps\\common\\VRChat\\Heavenly\\Temp\\UBPU.exe";
            p.StartInfo.Arguments = "current.xml && lz4hc";
            p.StartInfo.WorkingDirectory = "D:\\Steam\\steamapps\\common\\VRChat\\Heavenly\\Temp\\";
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = false;
            p.Start();
            p.WaitForExit();

            while (!File.Exists($"Heavenly\\Temp\\current.LZMA"))
            {
                await Task.Delay(100);
            }

            File.Move("Heavenly\\Temp\\current.LZMA", "Heavenly\\Temp\\current.vrca");

            CU.Log(ConsoleColor.Blue, "Recompressed avatar!");

            Finally();
        }

        public static async void Finally()
        {
            CU.Log(ConsoleColor.Green, "Reuploading avatar...");

            while (!File.Exists($"Heavenly\\Temp\\current.vrca"))
            {
                await Task.Delay(100);
            }

            ApiFileHelper.UploadFileAsync("Heavenly\\Temp\\current.vrca", null, friendlyName, delegate (ApiFile file, string message) { CU.Log(ConsoleColor.Green, $"SUCCESSFULLY REUPLOADED AVATAR {file.name}!"); }, delegate (ApiFile file, string message) { CU.Log($"FAILED TO REUPLOADED AVATAR {file.name}"); }, delegate (ApiFile apiFile, string status, string subStatus, float pct) { CU.Log($"REUPLOAD AVATAR PROGRESS: {(pct * 100).ToString()}%"); }, delegate (ApiFile file) { CU.Log($"REUPLOAD AVATAR {file.name} CANCELED!"); return false; });
        }

        public static string GenerateAvatarID()
        {
            return "avtr_" + Guid.NewGuid().ToString();
        }

        private static int RenameBytes(byte[] byte_0, byte[] byte_1)
        {
            for (int i = 0; i < byte_0.Length - byte_1.Length; i++)
            {
                bool flag = true;
                for (int j = 0; j < byte_1.Length; j++)
                {
                    if (byte_0[i + j] != byte_1[j])
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    return i;
                }
            }
            return -1;
        }

    }
}
