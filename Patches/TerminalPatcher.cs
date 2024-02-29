using HarmonyLib;
using LethalRoles.Core;
using System;
using static LethalRoles.Utility.Utils;

namespace LethalRoles.Patches
{
    [HarmonyPatch(typeof(Terminal))]
    public class TerminalPatcher
    {
        private static TerminalNode rolePickerTerminalNode;

        private static TerminalNode roleConfirmNode;
        private static TerminalNode roleDenyNode;

        private static Role pendingRole = Role.None;

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        [HarmonyDebug]
        private static void StartHook(ref Terminal __instance)
        {
            rolePickerTerminalNode = CreateTerminalNode("PLACEHOLDER");
            roleConfirmNode = CreateTerminalNode("PLACEHOLDER");
            roleDenyNode = CreateTerminalNode("You have refused to take the new role!\n", clearPreviousText: false);
        }

        /// <summary> <c> 
        /// private TerminalNode ParsePlayerSentence()
        /// </c></summary>
        [HarmonyPatch("ParsePlayerSentence")]
        [HarmonyPrefix]
        [HarmonyDebug]
        private static bool ParsePlayerSentenceHook(ref Terminal __instance, ref TerminalNode __result)
        {
            Terminal terminal = __instance;

            string s = terminal.screenText.text[^terminal.textAdded..];
            s = (string)InvokeMethod(terminal, "RemovePunctuation", s);
            string[] array = s.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (array.Length >= 1)
            {
                string word = array[0];

                if(word is "job" or "jobs" or "role" or "roles")
                {
                    Role role = LocalPlayer.GetRole();
                    rolePickerTerminalNode.displayText = GetRoleOverviewText(role);
    
                    __result = rolePickerTerminalNode;
                    return false;
                }

                foreach(Role role in RoleManager.Instance.RegisteredRoles)
                {
                    if(word == role.TerminalKeyword)
                    {
                        pendingRole = role;
                        __result = role.TerminalNode;
                        return false;
                    }
                }

                if (pendingRole != Role.None)
                {
                    if (word == "confirm")
                    {
                        RoleManager.Instance.SetRoleToLocalPlayer(pendingRole);
                        roleConfirmNode.displayText = $"You are now a {LocalPlayer.GetRole().Name}!\n";

                        pendingRole = Role.None;
                        __result = roleConfirmNode;
                        return false;
                    }
                    else if (word == "deny")
                    {
                        pendingRole = Role.None;
                        __result = roleDenyNode;
                        return false;
                    }
                }
            }

            return true;
        }

        private static string GetRoleOverviewText(Role currentRole)
        {
            string text = "";
            if (currentRole != Role.None)
                text += $"You are currently a {currentRole.Name}.\n\n";

            foreach (Role role in RoleManager.Instance.RegisteredRoles)
            {
                text += role.ShortDescription + "\n\n";
            }

            text +=
            """

            Type the role name for more information.

            Type NOROLE OR CLEARROLE to give up your role.

            """;

            return text;
        }
    }
}
