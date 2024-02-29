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

        public static float GetScanSphereRadius(float value)
        {
            Role role = RoleManager.Instance.GetRoleOfLocalPlayer();

            if (role is Role.Scout)
                value *= 3f;

            Logger.LogInfo($"Scanning. Scan sphere radius: {value}");
            return value;
        }

        public static float GetScanSphereTravelDistance(float value)
        {
            Role role = RoleManager.Instance.GetRoleOfLocalPlayer();

            if(role is Role.Scout)
                value *= 3f;

            Logger.LogInfo($"Scanning. Scan sphere distance: {value}");
            return value;
        }

        public static float GetScanDistanceMultiplier()
        {
            Role role = RoleManager.Instance.GetRoleOfLocalPlayer();
            float value = 1f;

            if (role is Role.Scout)
                value = 2f;

            Logger.LogInfo($"Scanning. Scan distance multiplier: {value}");

            return value;
        }
    }
}
