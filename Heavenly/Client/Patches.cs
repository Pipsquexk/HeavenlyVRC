
using System.Reflection;

using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using VRC.Networking;
using Photon.Realtime;
using ExitGames.Client.Photon;

using Heavenly.VRChat;
using Heavenly.VRChat.Utilities;
using Heavenly.Client.Utilities;
using Heavenly.VRChat.Handlers;


namespace Heavenly.Client
{
    public static class Patches
    {

        public static HarmonyLib.Harmony Instance;

        public static HarmonyMethod GetLocalPatch(string name) => new HarmonyMethod(typeof(Patches).GetMethod(name, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic));

        public static void ApplyPatches()
        {
            Instance = new HarmonyLib.Harmony("HeavenlyPatches");
            Instance.Patch(typeof(UdonSync).GetMethod("UdonSyncRunProgramAsRPC"), GetLocalPatch("NewUdonSyncRunProgramAsRPC"));
            Instance.Patch(typeof(LoadBalancingClient).GetMethod("Method_Public_Virtual_New_Boolean_Byte_Object_RaiseEventOptions_SendOptions_0"), GetLocalPatch("NewRaiseEvent"));
            Instance.Patch(typeof(UiAvatarList).GetMethod("OnEnable"), GetLocalPatch("NewOnEnable"));
            Instance.Patch(typeof(QuickMenu).GetMethod("LateUpdate"), GetLocalPatch("NewQuickMenuLateUpdate"));
        }

        private static bool NewUdonSyncRunProgramAsRPC(string __0, VRC.Player __1)
        {
            CU.Log($"{__0} was called by {__1.field_Private_APIUser_0.displayName}");
            return true;
        }

        private static bool NewQuickMenuLateUpdate()
        {
            if (UIU.GetQuickMenu() == null)
                return true;

            if (UIU.GetQuickMenu().field_Private_Player_0 == null)
                return true;


            if (UIU.GetQuickMenu().field_Private_Player_0.prop_ApiAvatar_0.releaseStatus.ToLower() == "private")
            {
                ButtonHandler.GetCloneAvatarButton().GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "Private";
                ButtonHandler.GetCloneAvatarButton().GetComponentInChildren<Button>().GetComponentInChildren<Text>().color = Color.red;
                ButtonHandler.SetButtonColor(ButtonHandler.GetCloneAvatarButton(), Color.red);
                ButtonHandler.GetCloneAvatarButton().GetComponentInChildren<Button>().interactable = false;
                return true;
            }


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
