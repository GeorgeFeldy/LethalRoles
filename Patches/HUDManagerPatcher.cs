using GameNetcodeStuff;
using HarmonyLib;
using LethalRoles.Core;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static LethalRoles.Utility.Utils;

namespace LethalRoles.Patches.PlayerController
{
    [HarmonyPatch(typeof(HUDManager))]
    public static class HUDManagerPatcher
    {
        const int ScrapScanNodeType = 2;

        [HarmonyPatch("MeetsScanNodeRequirements")]
        [HarmonyPostfix]
        private static void MeetsScanNodeRequirementsHook(ScanNodeProperties node, ref bool __result, PlayerControllerB playerScript)
        {
            Role role = LocalPlayer.GetRole();

            bool lineOfSight = Physics.Linecast(playerScript.gameplayCamera.transform.position, node.transform.position, 0x100, QueryTriggerInteraction.Ignore);
            if (lineOfSight)
            {
                float distance = Vector3.Distance(playerScript.transform.position, node.transform.position);

                float maxRange;
                if (node.headerText is "Main entrance" or "Ship")
                    maxRange = role.ModifyLandmarkScanDistance(node.maxRange);
                else if (node.nodeType == ScrapScanNodeType) 
                    maxRange = role.ModifyScrapScanDistance(node.maxRange);
                else 
                    maxRange = role.ModifyThreatScanDistance(node.maxRange);

                __result = distance < maxRange && distance > node.minRange;
            }
        }

        /*
        public static float RadiusMethod(float initialValue) => LocalPlayer.GetRole().ModifyScanSphereRadius(initialValue);
        public static float DistanceMethod(float initialValue) => LocalPlayer.GetRole().ModifyScanSphereTravelDistance(initialValue);

        [HarmonyPatch("AssignNewNodes")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> AssignNewNodesTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo radiusMethod = typeof(HUDManagerPatcher).GetMethod(nameof(RadiusMethod));
            MethodInfo distanceMethod = typeof(HUDManagerPatcher).GetMethod(nameof(DistanceMethod));
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            index = Utils.FindFloat(index, ref codes, findValue: 20f, skip: true, errorMessage: "Couldn't find scan start multiplier: 20f");
            index = Utils.FindFloat(index, ref codes, findValue: 20f, addCode: radiusMethod, errorMessage: "Couldn't find scan sphere radius: 20f");
            index = Utils.FindFloat(index, ref codes, findValue: 80f, addCode: distanceMethod, errorMessage: "Couldn't find scan sphere travel distance: 80f");

            return codes.AsEnumerable();
        }
        */
    }
}
