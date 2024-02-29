using GameNetcodeStuff;
using System;
using System.Reflection;

namespace LethalRoles.Utility
{
    public static partial class Utils
    {
        public static PlayerControllerB LocalPlayer => GameNetworkManager.Instance.localPlayerController;
        public static Terminal TerminalObject => UnityEngine.Object.FindObjectOfType<Terminal>();
    }
}
