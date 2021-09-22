using ExitGames.Client.Photon;
using HarmonyLib;
using Heavenly.VRChat;
using Heavenly.VRChat.Utilities;
using Heavenly.VRChat.Handlers;
using Photon.Realtime;
using System.Reflection;
using UnityEngine.UI;
using UnityEngine;

namespace Heavenly.Client
{
    public static class Patches
    {

        public static HarmonyLib.Harmony Instance;


        public static HarmonyMethod GetLocalPatch(string name) { return new HarmonyMethod(typeof(Patches).GetMethod(name, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)); }

        public static void ApplyPatches()
        {
            Instance = new HarmonyLib.Harmony("HeavenlyPatches");
            Instance.Patch(typeof(LoadBalancingClient).GetMethod("Method_Public_Virtual_New_Boolean_Byte_Object_RaiseEventOptions_SendOptions_0"), GetLocalPatch("NewRaiseEvent"));
            Instance.Patch(typeof(UiAvatarList).GetMethod("OnEnable"), GetLocalPatch("NewOnEnable"));
            Instance.Patch(typeof(QuickMenu).GetMethod("LateUpdate"), GetLocalPatch("NewQuickMenuLateUpdate"));

            //var exclusions = new string[] { "set", "get", "component", "message", "thread" };

            //foreach (MethodInfo mI in typeof(QuickMenu).GetMethods())
            //{
            //    if(!mI.Name.ToLower().Contains(exclusions[0]) && !mI.Name.ToLower().Contains(exclusions[1]) && !mI.Name.ToLower().Contains(exclusions[2]) && !mI.Name.ToLower().Contains(exclusions[3]) && !mI.Name.ToLower().Contains(exclusions[4]))
            //    {
            //        Instance.Patch(mI, GetLocalPatch("AllPatch"));

            //    }
            //}

        }

        private static bool NewQuickMenuLateUpdate()
        {
            if (PU.GetQuickMenu() == null)
                return true;

            if (PU.GetQuickMenu().field_Private_Player_0 == null)
                return true;


            if (PU.GetQuickMenu().field_Private_Player_0.prop_ApiAvatar_0.releaseStatus.ToLower() == "private")
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
