using GameNetcodeStuff;
using System;
using System.Reflection;
using UnityEngine;

namespace LethalRoles.Utility
{
    public static partial class Utils
    {
        public static TerminalNode CreateTerminalNode(string displayText, bool clearPreviousText = true, bool acceptAnything = false, params CompatibleNoun[] options)
        {
            var node = ScriptableObject.CreateInstance<TerminalNode>();
            node.displayText = displayText;
            node.clearPreviousText = clearPreviousText;
            node.acceptAnything = acceptAnything;
            node.terminalOptions = options;
            return node;
        }
    }
}
