using ExitGames.Client.Photon;
using HarmonyLib;
using Heavenly.Client.Utilities;
using Heavenly.VRChat.Handlers;
using Heavenly.VRChat.Utilities;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using VRC.Networking;

namespace Heavenly.Client
{
    public static class Patches
    {

        public static HarmonyLib.Harmony Instance;


        public static HarmonyMethod GetLocalPatch(string name) { return new HarmonyMethod(typeof(Patches).GetMethod(name, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)); }

        public static void ApplyPatches()
        {
            try
            {
                CU.Log(ConsoleColor.Cyan, "PATCHING");
                Instance = new HarmonyLib.Harmony("HeavenlyPatches");
                Instance.Patch(typeof(UdonSync).GetMethod("UdonSyncRunProgramAsRPC"), GetLocalPatch("NewUdonSyncRunProgramAsRPC"));
                Instance.Patch(typeof(LoadBalancingClient).GetMethod("Method_Public_Virtual_New_Boolean_Byte_Object_RaiseEventOptions_SendOptions_0"), GetLocalPatch("NewRaiseEvent"));
                //Instance.Patch(typeof(PhotonNetwork).GetMethod("Method_Private_Static_Void_EventData_PDM_0"), GetLocalPatch("OnEvent"));
                Instance.Patch(typeof(UiAvatarList).GetMethod("OnEnable"), GetLocalPatch("NewOnEnable"));
                CU.Log(ConsoleColor.Green, "PATCHED");

            }
            catch (Exception ex)
            {
                CU.Log(ConsoleColor.DarkRed, "FAILED TO PATCH");
                CU.Log(ConsoleColor.DarkRed, ex.ToString());
            }

            //Instance.Patch(typeof(QuickMenu).GetMethod("LateUpdate"), GetLocalPatch("NewQuickMenuLateUpdate"));

            //var exclusions = new string[] { "set", "get", "component", "message", "thread" };

            //foreach (MethodInfo mI in typeof(QuickMenu).GetMethods())
            //{
            //    if(!mI.Name.ToLower().Contains(exclusions[0]) && !mI.Name.ToLower().Contains(exclusions[1]) && !mI.Name.ToLower().Contains(exclusions[2]) && !mI.Name.ToLower().Contains(exclusions[3]) && !mI.Name.ToLower().Contains(exclusions[4]))
            //    {
            //        Instance.Patch(mI, GetLocalPatch("AllPatch"));

            //    }
            //}

        }

        //private static bool OnEvent(ref EventData __0)
        //{
        //    if (__0.Code == 6 || __0.Code == 7 || __0.Code == 9)
        //    {

        //        var syncData = Serialization.FromIL2CPPToManaged<byte[]>(__0.CustomData);

        //        if (sizeLimit)
        //        {
                    
        //            if (syncData.Length > 200)
        //            {
        //                MelonLogger.Msg($"Blocked Event {__0.Code} because Size to big (Size: " + syncData.Length + ") from: " + eventSender);
        //                return false;

        //            }
        //        }

        //        if (rateLimit)
        //        {
        //            if (!blacklistedUsers.Contains(eventSender))
        //            {
        //                blacklistedUsers.Add(eventSender);
        //                Delay(1f, delegate
        //                {
        //                    blacklistedUsers.Remove(eventSender);
        //                });
        //            }
        //            MelonLogger.Msg($"Rate limited Event 210 from: " + eventSender);
        //            return false;
        //        }
        //    }

        //    if (__0.Code == 8 || __0.Code == 210)
        //    {

        //        var syncData = Serialization.FromIL2CPPToManaged<int[]>(__0.CustomData);

        //        if (sizeLimit)
        //        {

        //            if (syncData.Length > 200)
        //            {
        //                MelonLogger.Msg($"Blocked Event {__0.Code} because Size to big (Size: " + syncData.Length + ") from: " + eventSender);
        //                return false;

        //            }
        //        }

        //        if (rateLimit)
        //        {
        //            if (!blacklistedUsers.Contains(eventSender))
        //            {
        //                blacklistedUsers.Add(eventSender);
        //                Delay(1f, delegate
        //                {
        //                    blacklistedUsers.Remove(eventSender);
        //                });
        //            }
        //            MelonLogger.Msg($"Rate limited Event 210 from: " + eventSender);
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        private static bool NewUdonSyncRunProgramAsRPC(string __0, VRC.Player __1)
        {
            CU.Log($"{__0} was called by {__1.field_Private_APIUser_0.displayName}");
            return true;
        }

        private static bool NewQuickMenuLateUpdate()
        {
            if (UIU.GetQuickMenu() == null)
                return true;

            //if (UIU.GetQuickMenu().field_Private_Player_0 == null)
            //    return true;


            //if (UIU.GetQuickMenu().field_Private_Player_0.prop_ApiAvatar_0.releaseStatus.ToLower() == "private")
            //{
            //    ButtonHandler.GetCloneAvatarButton().GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "Private";
            //    ButtonHandler.GetCloneAvatarButton().GetComponentInChildren<Button>().GetComponentInChildren<Text>().color = Color.red;
            //    ButtonHandler.SetButtonColor(ButtonHandler.GetCloneAvatarButton(), Color.red);
            //    ButtonHandler.GetCloneAvatarButton().GetComponentInChildren<Button>().interactable = false;
            //    return true;
            //}


            ButtonHandler.GetCloneAvatarButton().GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "Clone";
            ButtonHandler.GetCloneAvatarButton().GetComponentInChildren<Button>().GetComponentInChildren<Text>().color = Color.green;
            ButtonHandler.SetButtonColor(ButtonHandler.GetCloneAvatarButton(), Color.green);
            ButtonHandler.GetCloneAvatarButton().GetComponentInChildren<Button>().interactable = true;


            return true;
        }

        private static bool AllPatch(MethodInfo __originalMethod)
        {
            CU.Log(__originalMethod.Name);
            return true;
        }

        private static bool NewOnEnable()
        {
            Main.favList.RefreshList();
            return true;
        }

        private static bool NewRaiseEvent(byte __0, Il2CppSystem.Object __1, RaiseEventOptions __2, SendOptions __3)
        {

            switch (__0)
            {
                case 7:
                    if (Main.serialize)
                        return false;
                    break;
            }

            return true;
        }
    }
}
