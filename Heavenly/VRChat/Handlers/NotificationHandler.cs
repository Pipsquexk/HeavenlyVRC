using Transmtn;
using UnityEngine;
using VRC;

namespace Heavenly.VRChat.Handlers
{
    public static class NotificationHandler
    {
        public static string myId = null;
        public static void JoinNotify(Player p)
        {
            myId = PU.GetPlayer().field_Private_APIUser_0.id;
            if (p.field_Private_APIUser_0.id == myId || Main.nConfig.UseNotifs == false)
                return;

            GameObject.Instantiate(Main.notifBundle.LoadAsset<GameObject>($"JoinNotif.prefab"), PU.GetVRCPlayer().gameObject.transform);
        }

        public static void LeaveNotify(Player p)
        {
            if (p.field_Private_APIUser_0.id == myId || Main.nConfig.UseNotifs == false)
                return;

            GameObject.Instantiate(Main.notifBundle.LoadAsset<GameObject>($"LeaveNotif.prefab"), PU.GetVRCPlayer().gameObject.transform);
        }

        public static void NotificationNotify(Il2CppSystem.Object o, NotificationEvent nE)
        {
            if (Main.nConfig.UseNotifs == false)
                return;

            GameObject.Instantiate(Main.notifBundle.LoadAsset<GameObject>($"Notif.prefab"), PU.GetVRCPlayer().gameObject.transform);
        }
    }
}
