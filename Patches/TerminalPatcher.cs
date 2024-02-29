using HarmonyLib;
using LethalRoles.Managers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static LethalRoles.Utility.Utils;

/*
    To stop executing prefixes and skip the original, let the prefix return a bool that returns false. 
    To let the original run after all prefixes, return a bool that returns true.
 */

namespace LethalRoles.Patches
{
    public class TerminalPatcher
    {
        private static TerminalNode rolePickerTerminalNode;
        private static TerminalNode scoutTerminalNode;
        private static TerminalNode haulerTerminalNode;
        private static TerminalNode cleanerTerminalNode;
        private static TerminalNode techieTerminalNode;

        private static TerminalNode roleConfirmNode;
        private static TerminalNode roleDenyNode;

        private static Role pendingRole = Role.None;

        [HarmonyPatch(typeof(Terminal), "Start")]
        [HarmonyPostfix]
        private static void StartHook(ref Terminal __instance)
        {
            rolePickerTerminalNode = CreateTerminalNode(OverviewText);

            scoutTerminalNode = CreateTerminalNode(ScoutText);
            haulerTerminalNode = CreateTerminalNode(HaulerText);
            cleanerTerminalNode = CreateTerminalNode(CleanerText);
            techieTerminalNode = CreateTerminalNode(TechieText);
            roleConfirmNode = CreateTerminalNode("PLACEHOLDER (how did you get here?)\n");
            roleDenyNode = CreateTerminalNode("You have refused to take the new role!\n", clearPreviousText: false);
        }

        /// <summary> <c> 
        /// private TerminalNode ParsePlayerSentence()
        /// </c></summary>
        [HarmonyPatch(typeof(Terminal), "ParsePlayerSentence")]
        [HarmonyPrefix]
        private static bool ParsePlayerSentenceHook(ref Terminal __instance, ref TerminalNode __result)
        {
            Terminal terminal = __instance;

            string s = terminal.screenText.text[^terminal.textAdded..];
            s = (string)InvokeMethod(terminal, "RemovePunctuation", s);
            string[] array = s.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (array.Length >= 1)
            {
                string word = array[0];

                switch (word)
                {
                    case "job" or "jobs" or "role" or "roles":

                        Role role = RoleManager.Instance.GetRoleOfLocalPlayer();
                        if (role != Role.None)
                            rolePickerTerminalNode.displayText = $"You are currently a {role}.\n" + OverviewText;
                        else
                            rolePickerTerminalNode.displayText = OverviewText;

                        __result = rolePickerTerminalNode;
                        return false;

                    case "scout":
                        pendingRole = Role.Scout;
                        __result = scoutTerminalNode;
                        return false;

                    case "hauler":
                        pendingRole = Role.Hauler;
                        __result = haulerTerminalNode;
                        return false;

                    case "cleaner":
                        pendingRole = Role.Cleaner;
                        __result = cleanerTerminalNode;
                        return false;

                    case "techie":
                        pendingRole = Role.Techie;
                        __result = techieTerminalNode;
                        return false;

                    default:
                        break;
                }

                if (pendingRole != Role.None)
                {
                    if (word == "confirm")
                    {
                        RoleManager.Instance.SetRoleToLocalPlayer(pendingRole);
                        roleConfirmNode.displayText = $"You are now a {RoleManager.Instance.GetRoleOfLocalPlayer()}!\n";

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

        public const string OverviewText =
            """
            --- ROLES ---
            Scout 
            They would focus on exploring the areas of the facility with relatively good mobility overall and are able to use mobility/exploration tools more efficiently. On top of that, their scanner has more range and any enemy that gets scanned, would automatically become highlighted to the other members of the crew. They would however, suffer from having less health than normal, having one less inventory slot, and being less efficient with weapons.

            Hauler 
            They would be the carry support of the crew, having a 5th inventory slot, suffering far less from the speed penalty given by carrying many items, and being able to carry up to 2 heavy items at once instead of one. They also have a lot more health as well, but they are a little slower to run than normal. Should they hold 2 heavy items, they have a random chance to drop the last one they picked while sprinting.

            Cleaner 
            Their job would be to ensure the crew comes back alive by kicking the ass of anything that stands in their way. Their health is a little higher, but remains lower than the hauler's. They also regain a portion of their health and endurance back whenever they hurt an enemy. They also hit harder when using weapons, but they have no scanner on themselves, and suffer a bigger speed penalty when hauling items not categorized as weapons.

            Techie
            The egghead of the crew, they specialize in disarming the traps of the facilities to their advantage, and even be able to use them to their advantage. Turrets and landmines can be disarmed permanently, but they can also be rewired to only target enemies, making them powerful defense measures. Disarming however, needs to be done by hand, and should be done with absolute care. They also are highly afraid of monsters, freezing for 1 full second if an enemy get close by them.

            Type the role name for more information.
            To give up your role, type NOROLE.

            """;

        public const string ScoutText =
            """
            Scout 
            They would focus on exploring the areas of the facility with relatively good mobility overall and are able to use mobility/exploration tools more efficiently. On top of that, their scanner has more range and any enemy that gets scanned, would automatically become highlighted to the other members of the crew. They would however, suffer from having less health than normal, having one less inventory slot, and being less efficient with weapons.

            - The two flashlights would make scrap items shine when exposed to their light for everyone to see.
            - The lockpicker would take half as long to unlock doors and weight half as much (8 lb instead of 16lb).
            - Spray paint also works indefinitely when shaken once, it will no longer need to be shaken again if held by the scout.
            - Jetpack has also a 20% increase to its thrust power.
            - TZP-Inhalant gives even more speed.
            - Shovel deals 30% less damage.

            Would you like to take this formation? CONFIRM or DENY.

            """;


        public const string HaulerText =
            """
            Hauler 
            They would be the carry support of the crew, having a 5th inventory slot, suffering far less from the speed penalty given by carrying many items, and being able to carry up to 2 heavy items at once instead of one. They also have a lot more health as well, but they are a little slower to run than normal. Should they hold 2 heavy items, they have a random chance to drop the last one they picked while sprinting.
            - TZP-Inhalant has lessened visual effects (no smoke effect).
            - Jetpack has less thrust, making it near unusable by the hauler.

            Would you like to take this formation? CONFIRM or DENY.

            """;

        public const string CleanerText =
            """
            Cleaner 
            Their job would be to ensure the crew comes back alive by kicking the ass of anything that stands in their way. Their health is a little higher, but remains lower than the hauler's. They also regain a portion of their health and endurance back whenever they hurt an enemy. They also hit harder when using weapons, but they have no scanner on themselves, and suffer a bigger speed penalty when hauling items not categorized as weapons.
            - Shovel deals 35% more damage.
            - Stun grenades cannot disorient the cleaner.
            - Zap gun is given a slider which helps you find the sweet spot to keep on zapping as long as the gun can.
            - Homemade flashbangs are immediately thrown in front of the user upon use.
            - Shotgun shells are directly stored into a separate "5th" inventory slot, which only serves as an ammo reserve.
            - Stop and Yeld signs weight as much as a shovel, but don't benefit from the damage bonus.
            - All other items and scrap, weight DOUBLE.

            Would you like to take this formation? CONFIRM or DENY.

            """;

        public const string TechieText =
            """       
            Techie
            The egghead of the crew, they specialize in disarming the traps of the facilities to their advantage, and even be able to use them to their advantage. Turrets and landmines can be disarmed permanently, but they can also be rewired to only target enemies, making them powerful defense measures. Disarming however, needs to be done by hand, and should be done with absolute care. They also are highly afraid of monsters, freezing for 1 full second if an enemy get close by them.
            - Battery powered items last 25% longer.
            - Zap gun can deactivate traps as if done by a terminal.
            - Flashlights can highlight broken valves.
            - TZP-Inhalant has worse visual effects.

            Would you like to take this formation? CONFIRM or DENY.

            """;
    }
}
