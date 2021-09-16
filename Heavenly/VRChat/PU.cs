using Heavenly.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VRC;
using VRC.Core;
using VRC.SDKBase;

namespace Heavenly.VRChat
{
    public static class PU
    {

        public static VRCPlayer GetVRCPlayer()
        {
            return VRCPlayer.field_Internal_Static_VRCPlayer_0;
        }

        public static Player GetPlayer()
        {
            return Player.prop_Player_0;
        }

        public static VRCPlayerApi GetVRCPlayerApi()
        {
            return GetVRCPlayer().field_Private_VRCPlayerApi_0;
        }

        public static void DownloadAvatar(ApiAvatar avatar)
        {
            if (!Directory.Exists("Heavenly\\Avatars"))
            {
                Directory.CreateDirectory("Heavenly\\Avatars");
            }
            WebClient client = new WebClient();
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.164 Safari/537.36 OPR/77.0.4054.298");
            client.Headers.Add("Cookie", "auth=" + ApiCredentials.authToken);
            client.DownloadFileAsync(new Uri(avatar.assetUrl), $"Heavenly\\Avatars\\{avatar.name}-{avatar.id}-{avatar.authorName}.vrca");

        }

        //public static async void ReuploadAvatar(ApiAvatar avatar)
        //{
        //    CU.Log("avtr_" + Guid.NewGuid().ToString());
        //    avatar.id = "avtr_" + Guid.NewGuid().ToString();
        //    avatar.authorId = "avtr_" + Guid.NewGuid().ToString();
        //    avatar.Save();
        //    ApiFile.Create();
        //}

    }
}
