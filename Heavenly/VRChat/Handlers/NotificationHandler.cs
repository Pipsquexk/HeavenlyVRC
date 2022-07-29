using System;
using System.Collections;

using VRC;
using Transmtn;
using MelonLoader;
using UnityEngine;

using Heavenly.VRChat.Utilities;
using Heavenly.Client.Utilities;


namespace Heavenly.VRChat.Handlers
{
    public static class NotificationHandler
    {
        public static string myId = null;
        public static void JoinNotify(Player p)
        {
            try
            {
                myId = PU.GetPlayer().field_Private_APIUser_0.id;

                if (p.field_Private_APIUser_0.id == myId)
                {
                    if (PU.currentLobbyId != WU.BuildInstanceID())
                    {
                        PU.lastLobbyId = PU.currentLobbyId;
                        PU.currentLobbyId = WU.BuildInstanceID();
                    }

                    if (WebsocketHandler.beingTaggedAlong)
                    {
                        WebsocketHandler.SendTagAlongUpdate();
                    }
                }

                if (Main.esp && Main.playerESP)
                {
                    if (p.transform.Find("SelectRegion"))
                    {
                        if (p.field_Private_APIUser_0.isFriend)
                        {
                            Main.friendsFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                        }
                        else if (p.field_Private_APIUser_0.hasVeteranTrustLevel)
                        {
                            Main.trustedFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                        }
                        else if (p.field_Private_APIUser_0.hasTrustedTrustLevel)
                        {
                            Main.knownFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                        }
                        else if (p.field_Private_APIUser_0.hasKnownTrustLevel)
                        {
                            Main.userFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                        }
                        else if (p.field_Private_APIUser_0.hasBasicTrustLevel)
                        {
                            Main.newUserFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                        }
                        else
                        {
                            Main.visitorFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                        }
                    }
                }

                if (p.field_Private_APIUser_0.id == myId)
                    return;

                Main.debugList.AddEntry($"{p.field_Private_APIUser_0.displayName} <color=green>joined</color> your jobby");

                if (Main.nConfig.UseNotifs == false)
                    return;

                MelonCoroutines.Start(Join());
            }
            catch (NullReferenceException)
            {
                CU.Log(ConsoleColor.Yellow, "Something with join notifications is wrong. More Specifically, something is empty. Please submit a support ticket at https://discord.gg/APz5CANAvt in the #support-tickets channel with a screenshot of this.");
            }
            catch (UnhollowerBaseLib.Il2CppException ex2)
            {
                CU.Log(ConsoleColor.Yellow, "Something with join notifications is wrong. Please submit a support ticket at https://discord.gg/APz5CANAvt in the #support-tickets channel with a screenshot of the following.");
                CU.Log(ConsoleColor.Red, ex2.ToString());
            }
        }

        public static void LeaveNotify(Player p)
        {
            if (p.field_Private_APIUser_0.id == myId)
                return;

            Main.debugList.AddEntry($"{p.field_Private_APIUser_0.displayName} <color=red>left</color> your jobby");

            if (Main.nConfig.UseNotifs == false)
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
