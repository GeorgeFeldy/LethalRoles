using GameNetcodeStuff;
using HarmonyLib;
using LethalRoles.Managers;

namespace LethalRoles.Patches.PlayerController
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    public static class PlayerControllerBPatcher
    {
        [HarmonyPatch((nameof(PlayerControllerB.DamagePlayer)))]
        [HarmonyPrefix]
        private static void DamagePlayerHook(PlayerControllerB __instance, ref int damageNumber, bool fallDamage)
        {
            PlayerControllerB player = __instance;
            damageNumber = PlayerPowerManager.GetIncomingDamageAdjustment(player, damageNumber, fallDamage);
        }
    }
}
