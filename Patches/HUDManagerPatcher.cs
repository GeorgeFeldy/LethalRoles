using GameNetcodeStuff;
using HarmonyLib;
using LethalRoles.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LethalRoles.Patches.PlayerController
{
    [HarmonyPatch(typeof(HUDManager))]
    public static class HUDManagerPatcher
    {
        [HarmonyPatch("AssignNewNodes")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> AssignNewNodesTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo radiusMethod = typeof(PlayerPowerManager).GetMethod(nameof(PlayerPowerManager.GetScanSphereRadius));
            MethodInfo distanceMethod = typeof(PlayerPowerManager).GetMethod(nameof(PlayerPowerManager.GetScanSphereTravelDistance));
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            index = Utils.FindFloat(index, ref codes, findValue: 20f, skip: true, errorMessage: "Couldn't find scan start multiplier: 20f");
            index = Utils.FindFloat(index, ref codes, findValue: 20f, addCode: radiusMethod, errorMessage: "Couldn't find scan sphere radius: 20f");
            index = Utils.FindFloat(index, ref codes, findValue: 80f, addCode: distanceMethod, errorMessage: "Couldn't find scan sphere travel distance: 80f");

            return codes.AsEnumerable();
        }

        [HarmonyPatch("MeetsScanNodeRequirements")]
        [HarmonyPostfix]
        private static void MeetsScanNodeRequirementsHook(ScanNodeProperties node, ref bool __result, PlayerControllerB playerScript)
        {
            bool lineOfSight = Physics.Linecast(playerScript.gameplayCamera.transform.position, node.transform.position, 0x100, QueryTriggerInteraction.Ignore);
            if (lineOfSight)
            {
                float distance = Vector3.Distance(playerScript.transform.position, node.transform.position);

                float maxRange;
                if (node.headerText is "Main entrance" or "Ship")
                    maxRange = node.maxRange * PlayerPowerManager.GetLandmarkScanDistanceMultiplier();
                else 
                    maxRange = node.maxRange * PlayerPowerManager.GetObjectScanDistanceMultiplier();

                __result = distance < maxRange && distance > node.minRange;
            }
        }
    }
}
