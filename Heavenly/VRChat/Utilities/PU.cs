using Heavenly.Client;
using Heavenly.Client.API;
using Heavenly.Client.Utilities;
using Heavenly.VRChat.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.SDKBase;
using VRC.UI;

namespace Heavenly.VRChat.Utilities
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

        public static Player GetSelectedPlayer()
        {
            return UIU.GetQuickMenu().field_Private_Player_0;
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

        public static void ForceClone(ApiAvatar avatar)
        {
            if (avatar.releaseStatus.ToLower() == "private")
            {
                CU.Log(ConsoleColor.Red, $"Cannot clone {avatar.name}, it is private");
                return;
            }

            var page = GameObject.Find("Screens").transform.Find("Avatar").GetComponent<PageAvatar>();

            page.field_Public_SimpleAvatarPedestal_0 = new VRC.SimpleAvatarPedestal()
            {
                field_Internal_ApiAvatar_0 = avatar
            };

            page.ChangeToSelectedAvatar();
        }

        public static void ForceCloneByID(string id)
        {

            var page = GameObject.Find("Screens").transform.Find("Avatar").GetComponent<PageAvatar>();

            page.field_Public_SimpleAvatarPedestal_0 = new VRC.SimpleAvatarPedestal()
            {
                field_Internal_ApiAvatar_0 = new ApiAvatar() { id = id }
            };

            page.ChangeToSelectedAvatar();
        }

        public static ApiAvatar ToApiAvatar(this HevApiAvatar avi)
        {
            return new ApiAvatar() { name = avi.name, id = avi.id, authorName = avi.authorName, authorId = avi.authorId, thumbnailImageUrl = avi.thumbnailImageUrl, assetUrl = avi.assetUrl };
        }

        public static void RequestToTagAlong(Player player)
        {
            WebsocketHandler.tagAlongSocket.Send($"{PU.GetPlayer().field_Private_APIUser_0.displayName}={PU.GetPlayer().field_Private_APIUser_0.id}={player.field_Private_APIUser_0.id}=request=null");
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
