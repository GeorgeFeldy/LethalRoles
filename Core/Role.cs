using GameNetcodeStuff;
using UnityEngine;
using static LethalRoles.Utility.Utils;

namespace LethalRoles.Core
{
    public abstract class Role
    {
        private class DefaultDummyRole : Role
        {
            public override string ShortDescription => "This is the default role";
            public override string LongDescription => "This is the default role";
        }

        public static Role None = new DefaultDummyRole();

        public virtual string Name => GetType().Name;

        public abstract string ShortDescription { get; }
        public abstract string LongDescription { get; }
        public virtual string ConfirmationText { get; } = "\n\nWould you like to take this formation?\nCONFIRM or DENY\n";

        public virtual string TerminalKeyword => Name.ToLower();

        public TerminalNode TerminalNode;

        public Role() 
        {
            TerminalNode = CreateTerminalNode(LongDescription + ConfirmationText);
        }

        public virtual void ModifyPlayerOnRoleAssign(PlayerControllerB player)
        {
            player.usernameBillboardText.text += $" ({player.GetRole().Name})";
        }

        public virtual void ModifyPlayerOnRoleClear(PlayerControllerB player)
        {
            player.usernameBillboardText.text = player.name;
        }

        public virtual int ModifyIncomingDamage(PlayerControllerB player, int damageNumber, bool hasDamageSFX = true, bool callRPC = true, CauseOfDeath causeOfDeath = CauseOfDeath.Unknown, int deathAnimation = 0, bool fallDamage = false, Vector3 force = default) 
        {
            return damageNumber;
        }

        public virtual float ModifyLandmarkScanDistance(float distance)
        {
            return distance;
        }

        public virtual float ModifyScrapScanDistance(float distance)
        {
            return distance;
        }

        public virtual float ModifyThreatScanDistance(float distance)
        {
            return distance;
        }

        /*
        public virtual bool ScanRequiresLineOfSight()
        {
            return true;
        }

        public virtual float ModifyScanSphereRadius(float radius) 
        {
            return radius;
        }

        public virtual float ModifyScanSphereTravelDistance(float distance) 
        {
            return distance;
        }
        */
    }
}
