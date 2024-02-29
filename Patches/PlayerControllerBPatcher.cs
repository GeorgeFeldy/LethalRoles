using GameNetcodeStuff;
using HarmonyLib;
using LethalRoles.Core;
using LethalRoles.Utility;
using UnityEngine;

namespace LethalRoles.Patches.PlayerController
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    public static class PlayerControllerBPatcher
    {
        [HarmonyPatch((nameof(PlayerControllerB.DamagePlayer)))]
        [HarmonyPrefix]
        private static bool DamagePlayerHook(PlayerControllerB __instance, ref int damageNumber, bool hasDamageSFX, bool callRPC, CauseOfDeath causeOfDeath, int deathAnimation, bool fallDamage, Vector3 force)
        {
            PlayerControllerB player = __instance;
            Role role = player.GetRole();

            int newDamage = role.ModifyIncomingDamage(player, damageNumber, hasDamageSFX, callRPC, causeOfDeath, deathAnimation, fallDamage, force);

            if (newDamage != damageNumber)
            {
                Plugin.Logger.LogInfo($"{player.name} ({role}) took {newDamage} damage, instead of {damageNumber} damage.");
                damageNumber = newDamage;

                if(newDamage <= 0)
                {
                    Plugin.Logger.LogInfo($"{player.name} ({role}) skipped DamagePlayer.");
                    return false;
                }
            }

            return true;
        }
    }
}
