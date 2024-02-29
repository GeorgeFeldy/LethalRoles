using LethalRoles.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LethalRoles.Roles
{
    public class Hauler : Role
    {
        public override string LongDescription =>
        """
        They would be the carry support of the crew, having a 5th inventory slot, suffering far less from the speed penalty given by carrying many items, and being able to carry up to 2 heavy items at once instead of one. They also have a lot more health as well, but they are a little slower to run than normal. Should they hold 2 heavy items, they have a random chance to drop the last one they picked while sprinting.    
        """;

        public override string ShortDescription =>
        """
        Hauler 
        They would be the carry support of the crew, having a 5th inventory slot, suffering far less from the speed penalty given by carrying many items, and being able to carry up to 2 heavy items at once instead of one. They also have a lot more health as well, but they are a little slower to run than normal. Should they hold 2 heavy items, they have a random chance to drop the last one they picked while sprinting.
        - TZP-Inhalant has lessened visual effects (no smoke effect).
        - Jetpack has less thrust, making it near unusable by the hauler.
        """;
    }
}
