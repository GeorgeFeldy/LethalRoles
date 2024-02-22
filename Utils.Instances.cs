using GameNetcodeStuff;
using System;
using System.Reflection;

namespace LethalRoles
{
    public static partial class Utils
    {
        public static StartOfRound PlayersManager => StartOfRound.Instance;
        public static Terminal TerminalObject => UnityEngine.Object.FindObjectOfType<Terminal>();
    }
}
