using ExitGames.Client.Photon;
using HarmonyLib;
using Photon.Realtime;
using System.Reflection;

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

            //var exclusions = new string[] { "set", "get", "component", "message", "thread" };

            //foreach (MethodInfo mI in typeof(QuickMenu).GetMethods())
            //{
            //    if(!mI.Name.ToLower().Contains(exclusions[0]) && !mI.Name.ToLower().Contains(exclusions[1]) && !mI.Name.ToLower().Contains(exclusions[2]) && !mI.Name.ToLower().Contains(exclusions[3]) && !mI.Name.ToLower().Contains(exclusions[4]))
            //    {
            //        Instance.Patch(mI, GetLocalPatch("AllPatch"));

            //    }
            //}

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
