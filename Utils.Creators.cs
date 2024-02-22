using GameNetcodeStuff;
using System;
using System.Reflection;
using UnityEngine;

namespace LethalRoles
{
    public static partial class Utils
    {
        public static TerminalKeyword CreateKeyword(string word, bool isVerb, TerminalNode result = null) 
        {
            var keyword = ScriptableObject.CreateInstance<TerminalKeyword>();
            keyword.word = word;
            keyword.isVerb = isVerb;
            keyword.specialKeywordResult = result;
            return keyword;
        }

        public static CompatibleNoun CreateCompatibleNoun(TerminalKeyword noun, TerminalNode node)
        {
            return new CompatibleNoun()
            {
                noun = noun,
                result = node
            };
        }

        public static TerminalNode CreateTerminalNode(string displayText, bool clearPreviousText = false, params CompatibleNoun[] options)
        {
            var node = ScriptableObject.CreateInstance<TerminalNode>();
            node.displayText = displayText;
            node.clearPreviousText = clearPreviousText;
            node.terminalOptions = options;
            return node;
        }
    }
}
