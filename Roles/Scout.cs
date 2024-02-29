using GameNetcodeStuff;
using LethalRoles.Core;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LethalRoles.Roles
{
    public class Scout : Role
    {
        public override int ModifyIncomingDamage(PlayerControllerB player, int damageNumber, bool hasDamageSFX, bool callRPC, CauseOfDeath causeOfDeath, int deathAnimation, bool fallDamage, Vector3 force)
        {
            int newDamage;
            if (fallDamage || causeOfDeath is CauseOfDeath.Gravity)
                 newDamage = (int)(damageNumber * 0.7f);
            else
                 newDamage = (int)(damageNumber * 1.25f);

            return newDamage;
        }

        public override float ModifyLandmarkScanDistance(float distance) => distance * 1.5f;
        public override float ModifyScrapScanDistance(float distance) => distance * 2f;
        public override float ModifyThreatScanDistance(float distance) => distance * 1.5f;

        public override string LongDescription =>
        """
        They would focus on exploring the areas of the facility with relatively good mobility overall and are able to use mobility/exploration tools more efficiently. On top of that, their scanner has more range and any enemy that gets scanned, would automatically become highlighted to the other members of the crew. They would however, suffer from having less health than normal, having one less inventory slot, and being less efficient with weapons.     
        """;

        public override string ShortDescription =>
        """
        Scout 
        They would focus on exploring the areas of the facility with relatively good mobility overall and are able to use mobility/exploration tools more efficiently. On top of that, their scanner has more range and any enemy that gets scanned, would automatically become highlighted to the other members of the crew. They would however, suffer from having less health than normal, having one less inventory slot, and being less efficient with weapons.

        - The two flashlights would make scrap items shine when exposed to their light for everyone to see.
        - The lockpicker would take half as long to unlock doors and weight half as much (8 lb instead of 16lb).
        - Spray paint also works indefinitely when shaken once, it will no longer need to be shaken again if held by the scout.
        - Jetpack has also a 20% increase to its thrust power.
        - TZP-Inhalant gives even more speed.
        - Shovel deals 30% less damage.
        """;
    }
}
