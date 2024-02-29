using BepInEx.Logging;
using GameNetcodeStuff;
using Unity.Netcode;
using static LethalRoles.Utility.Utils;

namespace LethalRoles.Managers
{
    public class PlayerPowerManager : NetworkBehaviour
    {
        internal static PlayerPowerManager Instance { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        public void ApplyRolePowers()
        {
            Role role = RoleManager.Instance.GetRoleOfLocalPlayer();

            switch (role)
            {
                case Role.Scout:
                    break;

                case Role.Hauler:
                    break;

                case Role.Cleaner:
                    break;

                case Role.Techie:
                    break;

                case Role.None: 
                    break;
            }
        }

        #region Netcode hooks

        public static int GetIncomingDamageAdjustment(PlayerControllerB player, int incomingDamage, bool fallDamage)
        {
            Role role = RoleManager.Instance.GetRole(player);
            int newDamage = incomingDamage;

            if (fallDamage)
            {
                switch (role)
                {
                    case Role.Scout:
                        newDamage = (int)(incomingDamage * 0.95f);
                        break;
                }
            }
            else
            {
                switch (role)
                {
                    case Role.Scout:
                        newDamage = (int)(incomingDamage * 1.2f);
                        break;

                    case Role.Hauler:
                        newDamage = (int)(incomingDamage * 0.8f);
                        break;

                    case Role.Cleaner:
                        newDamage = (int)(incomingDamage * 0.9f);
                        break;
                }
            }

            if (newDamage != incomingDamage)
                Plugin.Logger.LogInfo($"{player.name} ({role}) took {newDamage} damage, instead of {incomingDamage} damage.");

            return newDamage;       
        }

        #endregion

        #region Local powers

        public static float GetScanSphereRadius(float value)
        {
            Role role = RoleManager.Instance.GetRoleOfLocalPlayer();

            if (role is Role.Scout)
                value *= 1.2f;

            return value;
        }

        public static float GetScanSphereTravelDistance(float value)
        {
            Role role = RoleManager.Instance.GetRoleOfLocalPlayer();

            if(role is Role.Scout)
                value *= 2f;

            return value;
        }

        public static float GetLandmarkScanDistanceMultiplier()
        {
            Role role = RoleManager.Instance.GetRoleOfLocalPlayer();
            float value = 1f;

            if (role is Role.Scout)
                value = 1.5f;

            return value;
        }

        public static float GetObjectScanDistanceMultiplier()
        {
            Role role = RoleManager.Instance.GetRoleOfLocalPlayer();
            float value = 1f;

            if (role is Role.Scout)
                value = 2f;

            return value;
        }

        #endregion
    }
}
