using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace LethalRoles
{
    public class RolesManager : NetworkBehaviour
    {
        private Dictionary<ulong, Role> playerRoles = new();
        private static StartOfRound PlayersManager => StartOfRound.Instance;

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

        private PlayerControllerB FindPlayerById(ulong clientId)
        {
            foreach (var player in PlayersManager.allPlayerScripts)
            {
                if (player.actualClientId == clientId)
                     return player;
            }
            return null;
        }
    }
}
