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

            if(p.field_Private_APIUser_0.id == myId && WebsocketHandler.beingTaggedAlong)
            {
                WebsocketHandler.SendTagAlongUpdate();
            }

            if (Main.esp)
            {
                if (p.transform.Find("SelectRegion"))
                {
                    if (p.field_Private_APIUser_0.isFriend)
                    {
                        Main.friendsFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("ForwardDirection/Avatar").GetComponentInChildren<SkinnedMeshRenderer>(), true);
                        Main.friendsFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("ForwardDirection/Avatar").GetComponentInChildren<Renderer>(), true);
                        Main.friendsFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("ForwardDirection/Avatar").GetComponentInChildren<MeshRenderer>(), true);
                    }
                    else if (p.field_Private_APIUser_0.hasVeteranTrustLevel)
                    {
                        Main.trustedFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("ForwardDirection/Avatar").GetComponentInChildren<SkinnedMeshRenderer>(), true);
                        Main.trustedFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("ForwardDirection/Avatar").GetComponentInChildren<Renderer>(), true);
                        Main.trustedFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("ForwardDirection/Avatar").GetComponentInChildren<MeshRenderer>(), true);
                    }
                    else if (p.field_Private_APIUser_0.hasTrustedTrustLevel)
                    {
                        Main.knownFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("ForwardDirection/Avatar").GetComponentInChildren<SkinnedMeshRenderer>(), true);
                        Main.knownFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("ForwardDirection/Avatar").GetComponentInChildren<Renderer>(), true);
                        Main.knownFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("ForwardDirection/Avatar").GetComponentInChildren<MeshRenderer>(), true);
                    }
                    else if (p.field_Private_APIUser_0.hasKnownTrustLevel)
                    {
                        Main.userFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("ForwardDirection/Avatar").GetComponentInChildren<SkinnedMeshRenderer>(), true);
                        Main.userFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("ForwardDirection/Avatar").GetComponentInChildren<Renderer>(), true);
                        Main.userFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("ForwardDirection/Avatar").GetComponentInChildren<MeshRenderer>(), true);
                    }
                    else if (p.field_Private_APIUser_0.hasBasicTrustLevel)
                    {
                        Main.newUserFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("ForwardDirection/Avatar").GetComponentInChildren<SkinnedMeshRenderer>(), true);
                        Main.newUserFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("ForwardDirection/Avatar").GetComponentInChildren<Renderer>(), true);
                        Main.newUserFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("ForwardDirection/Avatar").GetComponentInChildren<MeshRenderer>(), true);
                    }
                    else
                    {
                        Main.visitorFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("ForwardDirection/Avatar").GetComponentInChildren<SkinnedMeshRenderer>(), true);
                        Main.visitorFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("ForwardDirection/Avatar").GetComponentInChildren<Renderer>(), true);
                        Main.visitorFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("ForwardDirection/Avatar").GetComponentInChildren<MeshRenderer>(), true);
                    }
                }
            }

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
            if (PU.GetVRCPlayer() == null)
                yield break;

            var ob = GameObject.Instantiate(Main.notifBundle.LoadAsset<GameObject>($"JoinNotif.prefab"), PU.GetVRCPlayer().gameObject.transform);
            yield return new WaitForSeconds(3);
            GameObject.Destroy(ob);
        }

        public static IEnumerator Leave()
        {
            if (PU.GetVRCPlayer() == null)
                yield break;

            var ob = GameObject.Instantiate(Main.notifBundle.LoadAsset<GameObject>($"LeaveNotif.prefab"), PU.GetVRCPlayer().gameObject.transform);
            yield return new WaitForSeconds(3);
            GameObject.Destroy(ob);
        }

        public static IEnumerator Notif()
        {
            if (PU.GetVRCPlayer() == null)
                yield break;

            var ob = GameObject.Instantiate(Main.notifBundle.LoadAsset<GameObject>($"Notif.prefab"), PU.GetVRCPlayer().gameObject.transform);
            yield return new WaitForSeconds(3);
            GameObject.Destroy(ob);
        }
    }
}
