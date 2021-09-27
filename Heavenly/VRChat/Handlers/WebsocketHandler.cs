using Heavenly.Client.Utilities;
using Heavenly.VRChat.Utilities;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using VRC.SDKBase;
using WebSocketSharp;

namespace Heavenly.VRChat.Handlers
{
    public static class WebsocketHandler
    {

        public static bool beingTaggedAlong = false;
        public static bool taggingAlong = false;

        public static string taggedUserId = null;

        public static WebSocket generalSocket;
        public static WebSocket tagAlongSocket;

        public static List<Action> actionQueue;

        public static void ConnectToWebsockets()
        {
            actionQueue = new List<Action>();

            MelonCoroutines.Start(MainThread());

            generalSocket = new WebSocket("ws://137.184.140.2:7777/General");
            tagAlongSocket = new WebSocket("ws://137.184.140.2:7777/TagAlong");

            generalSocket.OnMessage += OnGeneralMessage;
            tagAlongSocket.OnMessage += OnTagAlongMessage;

            generalSocket.Connect();
            tagAlongSocket.Connect();
        }

        public static void OnGeneralMessage(object sender, MessageEventArgs e)
        {

            var message = e.Data;

            if (message.ToLower().Contains("count:"))
            {
                var playerCount = message.Split(':')[1];
                Console.Title = $"Heavenly - v1.0 | Online Players: {playerCount}";
            }
            else
            {
                CU.Log(e.Data);
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
                        taggedUserId = requestingId;
                        tagAlongSocket.Send($"{PU.GetPlayer().field_Private_APIUser_0.displayName}={PU.GetPlayer().field_Private_APIUser_0.id}={requestingId}=response=null");
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
                        UIU.OpenVRCUINotifPopup("Tag Along Response", $"{requestingName} accepted your request to tag along!", "Okay");
                    }));
                }
            }

            if(type.ToLower() == "update")
            {
                RunOnMainThread(new Action(() => { if (taggingAlong) { Networking.GoToRoom(instanceId); } }));
            }

        }

        public static void SendTagAlongUpdate()
        {
            tagAlongSocket.Send($"{PU.GetPlayer().field_Private_APIUser_0.displayName}={PU.GetPlayer().field_Private_APIUser_0.id}={taggedUserId}=update={WU.BuildInstanceID()}");
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
