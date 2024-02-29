using LethalRoles.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LethalRoles.Roles
{
    public class Cleaner : Role
    {
        public override string ShortDescription =>
        """
        Their job would be to ensure the crew comes back alive by kicking the ass of anything that stands in their way. Their health is a little higher, but remains lower than the hauler's. They also regain a portion of their health and endurance back whenever they hurt an enemy. They also hit harder when using weapons, but they have no scanner on themselves, and suffer a bigger speed penalty when hauling items not categorized as weapons.  
        """;

        public override string LongDescription =>
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
        """;
    }
}
