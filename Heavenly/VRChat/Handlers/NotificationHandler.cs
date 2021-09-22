using System.Collections;
using Transmtn;
using UnityEngine;
using VRC;
using MelonLoader;
using Heavenly.VRChat.Utilities;

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

            MelonCoroutines.Start(Join());
        }

        public static void LeaveNotify(Player p)
        {
            if (p.field_Private_APIUser_0.id == myId || Main.nConfig.UseNotifs == false)
                return;

            MelonCoroutines.Start(Leave());
        }

        public static void NotificationNotify(Il2CppSystem.Object o, NotificationEvent nE)
        {
            if (Main.nConfig.UseNotifs == false)
                return;

            MelonCoroutines.Start(Notif());
        }




        public static IEnumerator Join()
        {
            var ob = GameObject.Instantiate(Main.notifBundle.LoadAsset<GameObject>($"JoinNotif.prefab"), PU.GetVRCPlayer().gameObject.transform);
            yield return new WaitForSeconds(3);
            GameObject.Destroy(ob);
        }

        public static IEnumerator Leave()
        {
            var ob = GameObject.Instantiate(Main.notifBundle.LoadAsset<GameObject>($"LeaveNotif.prefab"), PU.GetVRCPlayer().gameObject.transform);
            yield return new WaitForSeconds(3);
            GameObject.Destroy(ob);
        }

        public static IEnumerator Notif()
        {
            var ob = GameObject.Instantiate(Main.notifBundle.LoadAsset<GameObject>($"Notif.prefab"), PU.GetVRCPlayer().gameObject.transform);
            yield return new WaitForSeconds(3);
            GameObject.Destroy(ob);
        }
    }
}
