using HarmonyLib;
using System;
using Unity.Jobs.LowLevel.Unsafe;

/*
    To stop executing prefixes and skip the original, let the prefix return a bool that returns false. 
    To let the original run after all prefixes, return a bool that returns true.
 */

namespace LethalRoles
{
    public class TerminalHooks
    {
        [HarmonyPatch(typeof(Terminal), "Start")]
        [HarmonyPostfix]
        private static void StartHook(ref Terminal __instance)
        {
            //TerminalLoader.Load();
        }

        /// <summary><c> 
        /// public void OnSubmit() 
        /// </c></summary>
        [HarmonyPatch(typeof(Terminal), nameof(Terminal.OnSubmit))]
        [HarmonyPrefix]
        private static bool OnSubmitHook(ref Terminal __instance)
        {
            Terminal terminal = __instance;
            return true;
        }

        /// <summary> <c> 
        /// private TerminalNode ParsePlayerSentence()
        /// </c></summary>
        [HarmonyPatch(typeof(Terminal), "ParsePlayerSentence")]
        [HarmonyPrefix]
        private static bool ParsePlayerSentenceHook(ref Terminal __instance, ref TerminalNode __result) 
        {
            Terminal terminal = __instance;
            return true;
        }
    }
}
