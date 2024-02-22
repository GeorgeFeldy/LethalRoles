using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using static LethalRoles.Utils;

namespace LethalRoles
{
    public static class TerminalLoader
    {
        public static TerminalNode rolePickerTerminalNode;

        private static TerminalKeyword AddNewVerb(string word, TerminalNode result)
        {
            Debug.Log($"Created keyword: {word}, as a verb");
            var keyword = CreateKeyword(word, isVerb: true, result: result);
            TerminalObject.terminalNodes.allKeywords.AddToArray(keyword);
            return keyword;
        }

        private static TerminalKeyword AddNewNoun(string word)
        {
            Debug.Log($"Created keyword: {word}, as a noun");
            var keyword = CreateKeyword(word, isVerb: false);
            return keyword;
        }

        public static void Load()
        {
            rolePickerTerminalNode = CreateTerminalNode
            (
                text,
                clearPreviousText: true,
                CreateCompatibleNoun(AddNewNoun("Scout"), null),
                CreateCompatibleNoun(AddNewNoun("Hauler"), null),
                CreateCompatibleNoun(AddNewNoun("Cleaner"), null),
                CreateCompatibleNoun(AddNewNoun("Techie"), null)
            );

            Debug.Log($"Pre-load terminal nodes: {TerminalObject.terminalNodes.allKeywords.Length}");
            TerminalObject.terminalNodes.terminalNodes.Add(rolePickerTerminalNode);
            Debug.Log($"Post-load terminal nodes: {TerminalObject.terminalNodes.allKeywords.Length}");

            Debug.Log($"Pre-load terminal keywords: {TerminalObject.terminalNodes.allKeywords.Length}");

            AddNewVerb("role", rolePickerTerminalNode);
            AddNewVerb("roles", rolePickerTerminalNode);
            AddNewVerb("job", rolePickerTerminalNode);
            AddNewVerb("jobs", rolePickerTerminalNode);

            Debug.Log($"Post-load terminal keywords: {TerminalObject.terminalNodes.allKeywords.Length}");
        }

        public const string text =
            """
            Scout 
            They would focus on exploring the areas of the facility with relatively good mobility overall and are able to use mobility/exploration tools more efficiently. On top of that, their scanner has more range and any enemy that gets scanned, would automatically become highlighted to the other members of the crew. They would however, suffer from having less health than normal, having one less inventory slot, and being less efficient with weapons.

            - The two flashlights would make scrap items shine when exposed to their light for everyone to see.
            - The lockpicker would take half as long to unlock doors and weight half as much (8 lb instead of 16lb).
            - Spray paint also works indefinitely when shaken once, it will no longer need to be shaken again if held by the scout.
            - Jetpack has also a 20% increase to its thrust power.
            - TZP-Inhalant gives even more speed.
            - Shovel deals 30% less damage.

            Hauler 
            They would be the carry support of the crew, having a 5th inventory slot, suffering far less from the speed penalty given by carrying many items, and being able to carry up to 2 heavy items at once instead of one. They also have a lot more health as well, but they are a little slower to run than normal. Should they hold 2 heavy items, they have a random chance to drop the last one they picked while sprinting.
            - TZP-Inhalant has lessened visual effects (no smoke effect).
            - Jetpack has less thrust, making it near unusable by the hauler.

            Cleaner 
            Their job would be to ensure the crew comes back alive by kicking the ass of anything that stands in their way. Their health is a little higher, but remains lower than the hauler's. They also regain a portion of their health and endurance back whenever they hurt an enemy. They also hit harder when using weapons, but they have no scanner on themselves, and suffer a bigger speed penalty when hauling items not categorized as weapons.
            - Shovel deals 35% more damage.
            - Stun grenades cannot disorient the cleaner.
            - Zap gun is given a slider which helps you find the sweet spot to keep on zapping as long as the gun can.
            - Homemade flashbangs are immediately thrown in front of the user upon use.
            - Shotgun shells are directly stored into a separate "5th" inventory slot, which only serves as an ammo reserve.
            - Stop and Yeld signs weight as much as a shovel, but don't benefit from the damage bonus.
            - All other items and scrap, weight DOUBLE.

            Techie
            The egghead of the crew, they specialize in disarming the traps of the facilities to their advantage, and even be able to use them to their advantage. Turrets and landmines can be disarmed permanently, but they can also be rewired to only target enemies, making them powerful defense measures. Disarming however, needs to be done by hand, and should be done with absolute care. They also are highly afraid of monsters, freezing for 1 full second if an enemy get close by them.
            - Battery powered items last 25% longer.
            - Zap gun can deactivate traps as if done by a terminal.
            - Flashlights can highlight broken valves.
            - TZP-Inhalant has worse visual effects.

            As for picking your class, I think you could have a new section at the terminal labelled as "jobs", which lists you the 4 classes alongside their bio.
            Writing down the name of a class gives you the bio in a format like this:

            Lorem ipsum dolor sit amet, consectetur adipiscing elit.
            Sed non risus. Suspendisse lectus tortor, dignissim sit amet, adipiscing nec, ultricies sed, dolor. 
            Cras elementum ultrices diam. Maecenas ligula massa, varius a, semper congue, euismod non, mi. 
            Proin porttitor, orci nec nonummy molestie, enim est eleifend mi, non fermentum

            Would you like to take this formation? CONFIRM or DENY.
            """;
    }
}
