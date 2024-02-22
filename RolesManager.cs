using GameNetcodeStuff;
using System.Collections.Generic;
using Unity.Netcode;
using static LethalRoles.Utils;

namespace LethalRoles
{
    public class RolesManager : NetworkBehaviour
    {
        public static RolesManager Instance => FindObjectOfType<RolesManager>();

        private Dictionary<ulong, Role> playerRoles = new();

        public void AssignNewRoleToLocalPlayer(string newRole) => GetRoleFromString(newRole);
        public void AssignNewRoleToLocalPlayer(Role newRole)
        {
            var player = PlayersManager.localPlayerController;
            AssignRole(player, newRole);
            SyncRoleServerRpc(player.playerClientId, newRole);
        }

        private void AssignRole(PlayerControllerB player, Role newRole)
        {
            ulong clientId = player.actualClientId;

            if (playerRoles.ContainsKey(clientId))
                 playerRoles[clientId] = newRole;
             else
                 playerRoles.Add(clientId, newRole);
 
            // TODO: logic
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
            if(playerToChange != null) 
                AssignRole(playerToChange, newRole);
        }
    }
}
