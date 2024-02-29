using LethalRoles.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LethalRoles.Roles
{
    public class Techie : Role
    {
        public override string LongDescription =>
        """
        The egghead of the crew, they specialize in disarming the traps of the facilities to their advantage, and even be able to use them to their advantage. Turrets and landmines can be disarmed permanently, but they can also be rewired to only target enemies, making them powerful defense measures. Disarming however, needs to be done by hand, and should be done with absolute care. They also are highly afraid of monsters, freezing for 1 full second if an enemy get close by them.        
        """;

        public override string ShortDescription =>
        """
        Techie
        The egghead of the crew, they specialize in disarming the traps of the facilities to their advantage, and even be able to use them to their advantage. Turrets and landmines can be disarmed permanently, but they can also be rewired to only target enemies, making them powerful defense measures. Disarming however, needs to be done by hand, and should be done with absolute care. They also are highly afraid of monsters, freezing for 1 full second if an enemy get close by them.
        - Battery powered items last 25% longer.
        - Zap gun can deactivate traps as if done by a terminal.
        - Flashlights can highlight broken valves.
        - TZP-Inhalant has worse visual effects.
        """;
    }
}
