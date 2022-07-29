
using System;
using System.IO;
using System.Net;
using System.Collections.Generic;

using VRC;
using VRC.UI;
using VRC.Core;
using VRC.SDKBase;
using UnityEngine;
using Newtonsoft.Json;

using Heavenly.Client.API;
using Heavenly.VRChat.Handlers;
using Heavenly.Client.Utilities;


namespace Heavenly.VRChat.Utilities
{
    public static class PU
    {
        public static string lastLobbyId = "NULL", 
        currentLobbyId = "NULL";
        
        public static VRCPlayer GetVRCPlayer() => VRCPlayer.field_Internal_Static_VRCPlayer_0;
        
        public static VRCPlayerApi GetVRCPlayerApi() => GetVRCPlayer().field_Private_VRCPlayerApi_0;
        
        public static Player GetPlayer() => Player.prop_Player_0;
        public static Player GetSelectedPlayer() => UIU.GetQuickMenu().field_Private_Player_0;

        
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
            return new ApiAvatar() { name = avi.name, id = avi.id, authorName = avi.authorName, authorId = avi.authorId, thumbnailImageUrl = avi.thumbnailImageUrl, assetUrl = avi.assetUrl, supportedPlatforms = (ApiModel.SupportedPlatforms)(avi.platforms == null ? 1 : avi.platforms) };
        }

        public static void RequestToTagAlong(string tagId)
        {
            WebsocketHandler.tagAlongSocket.Send($"{PU.GetPlayer().field_Private_APIUser_0.displayName}={PU.GetPlayer().field_Private_APIUser_0.id}={tagId}=request=null");
        }

        public static void CancelTagAlong()
        {
            if(WebsocketHandler.taggedUserId != null)
            {
                WebsocketHandler.tagAlongSocket.Send($"{PU.GetPlayer().field_Private_APIUser_0.displayName}={PU.GetPlayer().field_Private_APIUser_0.id}={WebsocketHandler.taggedUserId}=cancel=null");
            }
            if (WebsocketHandler.taggingUserId != null)
            {
                WebsocketHandler.tagAlongSocket.Send($"{PU.GetPlayer().field_Private_APIUser_0.displayName}={PU.GetPlayer().field_Private_APIUser_0.id}={WebsocketHandler.taggingUserId}=cancel=null");
            }
            Main.taggedUserLabel.setText("<color=white>Tagged User: </color><color=red>NULL</color>");
            Main.taggingUserLabel.setText("<color=white>Tagging User: </color><color=red>NULL</color>");
            Main.selectedTaggedUserLabel.setText("<color=white>Tagged User: </color><color=red>NULL</color>");
            Main.selectedTaggingUserLabel.setText("<color=white>Tagging User: </color><color=red>NULL</color>");
            WebsocketHandler.taggingAlong = false;
            WebsocketHandler.beingTaggedAlong = false;
        }

        public static void ToggleESP(bool state)
        {
            HighlightsFX.prop_HighlightsFX_0.field_Protected_Material_0.SetColor("_HighlightColor", Color.red);

            if (Main.playerESP)
            {
                foreach (Player p in PU.GetAllPlayers())
                {
                    if (p.transform.Find("SelectRegion"))
                    {
                        HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), state);
                    }
                }
            }

            if (Main.itemESP)
            {
                foreach (VRC_Pickup pickup in PU.GetAllPickups())
                {
                    HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(pickup.GetComponentInChildren<MeshRenderer>(), state);
                }
            }

            if (Main.triggerESP)
            {
                foreach (VRC_Trigger trigger in PU.GetAllTriggers())
                {
                    HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(trigger.GetComponentInChildren<MeshRenderer>(), state);
                }
            }


        }

        public static List<VRC_Trigger> GetAllTriggers()
        {
            var triggers = new List<VRC_Trigger>();

            foreach (VRC_Trigger trigger in GameObject.FindObjectsOfType<VRC_Trigger>())
            {
                triggers.Add(trigger);
            }

            return triggers;
        }

        public static List<VRC_Pickup> GetAllPickups()
        {
            var pickups = new List<VRC_Pickup>();

            foreach (VRC_Pickup pickup in GameObject.FindObjectsOfType<VRC_Pickup>())
            {
                pickups.Add(pickup);
            }

            return pickups;
        }

        public static IEnumerable<Player> GetAllPlayers()
        {
            return PlayerManager.Method_Public_Static_ArrayOf_Player_0();
        }
        
        public static List<GameObject> GetAllGameObjects()
        {
            var gameObjects = new List<GameObject>();

            foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
            {
                gameObjects.Add(go);
            }

            return gameObjects;
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
