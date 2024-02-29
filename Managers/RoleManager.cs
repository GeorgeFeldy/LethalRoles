using GameNetcodeStuff;
using System.Collections.Generic;
using Unity.Netcode;
using static LethalRoles.Utility.Utils;

namespace LethalRoles.Managers
{
    public enum Role
    {
        None = -1,
        Scout,
        Hauler,
        Cleaner,
        Techie
    }

    public class RoleManager : NetworkBehaviour
    {
        internal static RoleManager Instance { get; set; }

        private Dictionary<ulong, Role> playerRoles;

        private void Awake()
        {
            Instance = this;
            playerRoles = new();
        }

        public void SetRoleToLocalPlayer(string newRole) => GetRoleFromString(newRole);
        public void SetRoleToLocalPlayer(Role newRole)
        {
            SetRole(LocalPlayer, newRole);
            SyncRoleServerRpc(LocalPlayer.playerClientId, newRole);
        }


        public void SetRole(PlayerControllerB player, Role newRole)
        {
            ulong clientId = player.actualClientId;

            if (playerRoles.ContainsKey(clientId))
                playerRoles[clientId] = newRole;
            else
                playerRoles.Add(clientId, newRole);

            // TODO: logic
        }

        public Role GetRoleOfLocalPlayer() => GetRole(LocalPlayer);

        public Role GetRole(PlayerControllerB player)
        {
            ulong clientId = player.actualClientId;

            if (playerRoles.ContainsKey(clientId))
                return playerRoles[clientId];

            return Role.None;
        }


        [ServerRpc]
        private void SyncRoleServerRpc(ulong clientId, Role newRole)
        {
            SyncRoleClientRpc(clientId, newRole);
        }

        [ClientRpc]
        private void SyncRoleClientRpc(ulong clientId, Role newRole)
        {
            PlayerControllerB playerToChange = FindPlayerById(clientId);
            if (playerToChange != null)
                SetRole(playerToChange, newRole);
        }
    }
}
