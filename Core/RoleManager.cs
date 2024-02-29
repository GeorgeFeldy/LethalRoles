using BepInEx.Logging;
using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Data;
using Unity.Netcode;
using static LethalRoles.Utility.Utils;

namespace LethalRoles.Core
{
    public class RoleManager : NetworkBehaviour
    {
        internal static RoleManager Instance { get; set; }

        public Dictionary<ulong, Role> PlayerRoles { get; private set; }

        public List<Role> RegisteredRoles { get; private set;  }

        private void Awake()
        {
            Instance = this;
            PlayerRoles = new();
            RegisteredRoles = new();
        }

        public void RegisterRole<T>() where T : Role
        {
            Type roleType = typeof(T);
            if (Activator.CreateInstance(roleType) is Role role)
                RegisteredRoles.Add(role);
        }

        public void SetRoleToLocalPlayer(string newRole) 
        {
            Type type = Type.GetType("LethalRoles.Core.Role." + newRole);
            if (type is not null)
            {
                object instance = Activator.CreateInstance(type, new object[] { LocalPlayer });
                if (instance is Role role)
                    SetRoleToLocalPlayer(role);
                else
                    Plugin.Logger.LogError($"Cannot set role {newRole}");
            }
            else
            {
                Plugin.Logger.LogError($"Cannot set role {newRole}");
            }
        }

        public void SetRoleToLocalPlayer(Role newRole)
        {
            SetRole(LocalPlayer, newRole);
            SyncRoleServerRpc(LocalPlayer.playerClientId, newRole);
        }

        public void SetRole(PlayerControllerB player, Role newRole)
        {
            Role oldRole = GetRole(player);
            oldRole.ModifyPlayerOnRoleClear(player);

            ulong clientId = player.actualClientId;
            if (PlayerRoles.ContainsKey(clientId))
                PlayerRoles[clientId] = newRole;
            else
                PlayerRoles.Add(clientId, newRole);

            newRole.ModifyPlayerOnRoleAssign(player);
        }

        public Role GetRoleOfLocalPlayer() => GetRole(LocalPlayer);

        public Role GetRole(PlayerControllerB player)
        {
            ulong clientId = player.actualClientId;
            if (PlayerRoles.ContainsKey(clientId))
                return PlayerRoles[clientId];

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
