using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using HarmonyLib;

namespace Heavenly
{
    public static class Patches
    {

        public static HarmonyLib.Harmony Instance;


        public static HarmonyMethod GetLocalPatch(string name) { return new HarmonyMethod(typeof(Patches).GetMethod(name, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)); }

        public static void ApplyPatches()
        {
            Instance = new HarmonyLib.Harmony("HeavenlyPatches");
            //Instance.Patch(AccessTools.Property(typeof(MelonLogger), "DefaultMelonColor").GetMethod, GetLocalPatch("MelonColorPatch"));
        }
    }
}
