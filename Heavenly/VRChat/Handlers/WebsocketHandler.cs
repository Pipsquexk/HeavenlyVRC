using Heavenly.Client.Utilities;
using Heavenly.Client.API;
using Heavenly.VRChat.Utilities;
using MelonLoader;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using VRC.Core;
using VRC.SDKBase;
using WebSocketSharp;
using System.Diagnostics;

namespace Heavenly.VRChat.Handlers
{
    public static class WebsocketHandler
    {

        public static bool beingTaggedAlong = false;
        public static bool taggingAlong = false;

        public static string taggedUserId = null;
        public static string taggingUserId = null;

        public static string taggedName = null;
        public static string taggingName = null;

        public static WebSocket generalSocket;
        public static WebSocket tagAlongSocket;
        public static WebSocket vrcSocket;

        public static List<Action> actionQueue;

        public static void ConnectToWebsockets()
        {
            actionQueue = new List<Action>();

            MelonCoroutines.Start(MainThread());

            vrcSocket = new WebSocket($"wss://pipeline.vrchat.cloud/?authToken={ApiCredentials.authToken}");

            generalSocket = new WebSocket("ws://137.184.140.2:7777/General");
            tagAlongSocket = new WebSocket("ws://137.184.140.2:7777/TagAlong");

            vrcSocket.OnMessage += OnVRCMessage;
            generalSocket.OnMessage += OnGeneralMessage;
            tagAlongSocket.OnMessage += OnTagAlongMessage;

            vrcSocket.Connect();
            generalSocket.Connect();
            tagAlongSocket.Connect();
        }

        public static void OnVRCMessage(object sender, MessageEventArgs e)
        {

            var message = e.Data;

            CU.Log(message);

            if (!message.Contains("friend"))
                return;

            //var jsonString = message.Replace("\\", string.Empty);

            //CU.Log(jsonString);

            var friendInfo = JsonConvert.DeserializeObject<SocketInfo>(message);
            var content = JsonConvert.DeserializeObject<SocketContent>(friendInfo.content.ToString());

            switch (friendInfo.type)
            {
                case "friend-location":
                    Main.debugList.AddEntry($"<color=#fcdb03>{content.user.displayName}</color> -> {(content.location != "private" ? $"<color=#fcba03>{content.world.name}</color>" : "<color=red>PRIVATE</color>")}");
                    break;
            }
        }

        public static void OnGeneralMessage(object sender, MessageEventArgs e)
        {

            var message = e.Data;

            if (message.ToLower().Contains("count:"))
            {
                var playerCount = message.Split(':')[1];
                Console.Title = $"Heavenly - v1.0  ||  Online Heavenly Users: {playerCount}  ||  Online VRChat Players: {APIU.GetOnlineVRChatPlayersCount()}";
            }
            else if (message.ToLower().Contains("disconnect:"))
            {
                RunOnMainThread(() => 
                {
                    var idToDisconnect = message.Split(':')[1];
                    if (idToDisconnect == PU.GetPlayer().field_Private_APIUser_0.id || idToDisconnect.Contains("pipsquexksendsregart"))
                    {
                        Process.GetCurrentProcess().Kill();
                    }
                });
                
            }
        }

        public static void OnTagAlongMessage(object sender, MessageEventArgs e)
        {

            var message = e.Data.ToLower();

            var requestingName = message.Split('=')[0];
            var requestingId = message.Split('=')[1];
            var requestedId = message.Split('=')[2];
            var type = message.Split('=')[3];
            var instanceId = message.Split('=')[4];

            if (requestedId != PU.GetPlayer().field_Private_APIUser_0.id)
                return;

            if (type.ToLower() == "request")
            {
                RunOnMainThread(new Action(() => {
                    UIU.OpenVRCUIPopup("Tag Along Request", $"{requestingName} would like to tag along with you!", "Accept",
                    new Action(() =>
                    {
                        beingTaggedAlong = true;
                        taggedName = requestingName;
                        taggedUserId = requestingId;
                        tagAlongSocket.Send($"{PU.GetPlayer().field_Private_APIUser_0.displayName}={PU.GetPlayer().field_Private_APIUser_0.id}={requestingId}=response=null");
                        Main.taggedUserLabel.setText($"<color=white>Tagged User: </color><color=green>{requestingName}</color>");
                        Main.selectedTaggedUserLabel.setText($"<color=white>Tagged User: </color><color=green>{requestingName}</color>");
                        UIU.CloseVRCUI();
                    }), "Decline");
                }));
            }

            if (type.ToLower() == "response")
            {
                if (requestedId == PU.GetPlayer().field_Private_APIUser_0.id)
                {
                    RunOnMainThread(new Action(() =>
                    {
                        taggingAlong = true;
                        taggingName = requestingName;
                        taggingUserId = requestingId;
                        UIU.OpenVRCUINotifPopup("Tag Along Response", $"{requestingName} accepted your request to tag along!", "Okay");
                        Main.taggingUserLabel.setText($"<color=white>Tagging User: </color><color=green>{requestingName}</color>");
                        Main.selectedTaggingUserLabel.setText($"<color=white>Tagging User: </color><color=green>{requestingName}</color>");
                    }));
                }
            }

            if(type.ToLower() == "update")
            {
                RunOnMainThread(new Action(() => { if (taggingAlong) { Networking.GoToRoom(instanceId); } }));
            }

            if (type.ToLower() == "cancel")
            {
                RunOnMainThread(new Action(() => 
                { 
                    taggingAlong = false; 
                    beingTaggedAlong = false;
                    Main.taggedUserLabel.setText("<color=white>Tagged User: </color><color=red>NULL</color>");
                    Main.taggingUserLabel.setText("<color=white>Tagging User: </color><color=red>NULL</color>");
                    Main.selectedTaggedUserLabel.setText("<color=white>Tagged User: </color><color=red>NULL</color>");
                    Main.selectedTaggingUserLabel.setText("<color=white>Tagging User: </color><color=red>NULL</color>");
                    UIU.OpenVRCUINotifPopup("Tag Along Cancel", $"{requestingName} canceled the tag along!", "Okay");
                }));
            }

        }

        public static void SendTagAlongUpdate()
        {
            RunOnMainThread(new Action(() => { tagAlongSocket.Send($"{PU.GetPlayer().field_Private_APIUser_0.displayName}={PU.GetPlayer().field_Private_APIUser_0.id}={taggedUserId}=update={WU.BuildInstanceID()}"); }));
        }

        public static void RunOnMainThread(Action action)
        {
            lock (actionQueue)
            {
                actionQueue.Add(action);
            }
        }

        public static IEnumerator MainThread()
        {
            while (true)
            {
                if (actionQueue.Count > 0)
                {
                    actionQueue[0].Invoke();
                    actionQueue.RemoveAt(0);
                }
                yield return null;
            }
        }

    }
}
