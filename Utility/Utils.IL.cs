﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace LethalRoles
{
    // Code courtesy of Late Game Upgrades (MoreShipUpgrades), MIT License. Thanks! 
    public static partial class Utils
    {
        public static int FindCodeInstruction(int index, ref List<CodeInstruction> codes, object findValue, MethodInfo addCode, bool skip = false, bool requireInstance = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, string errorMessage = "Not found")
        {
            bool found = false;
            for (; index < codes.Count; index++)
            {
                if (!CheckCodeInstruction(codes[index], findValue)) continue;
                found = true;
                if (skip) break;
                if (andInstruction) codes.Insert(index + 1, new CodeInstruction(OpCodes.And));
                if (!andInstruction && orInstruction) codes.Insert(index + 1, new CodeInstruction(OpCodes.Or));
                if (notInstruction) codes.Insert(index + 1, new CodeInstruction(OpCodes.Not));
                codes.Insert(index + 1, new CodeInstruction(OpCodes.Call, addCode));
                if (requireInstance) codes.Insert(index + 1, new CodeInstruction(OpCodes.Ldarg_0));
                break;
            }
            if (!found) Plugin.Logger.LogError(errorMessage);
            return index + 1;
        }

        public static int FindLocalField(int index, ref List<CodeInstruction> codes, Type localType, int localIndex, object addCode, bool skip = false, bool store = false, bool requireInstance = false, string errorMessage = "Not found")
        {
            bool found = false;
            for (; index < codes.Count; index++)
            {
                if (!CheckCodeInstruction(codes[index], localType, localIndex, store)) continue;
                found = true;
                if (skip) break;
                codes.Insert(index + 1, new CodeInstruction(OpCodes.Call, addCode));
                if (requireInstance) codes.Insert(index + 1, new CodeInstruction(OpCodes.Ldarg_0));
                break;
            }
            if (!found) Plugin.Logger.LogError(errorMessage);
            return index + 1;
        }

        public static int FindString(int index, ref List<CodeInstruction> codes, string findValue, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found") 
            => FindCodeInstruction(index, ref codes, findValue, addCode, skip, requireInstance, notInstruction, andInstruction, orInstruction, errorMessage);

        public static int FindField(int index, ref List<CodeInstruction> codes, FieldInfo findField, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found") 
            => FindCodeInstruction(index, ref codes, findField, addCode, skip, requireInstance, notInstruction, andInstruction, orInstruction, errorMessage);

        public static int FindMethod(int index, ref List<CodeInstruction> codes, MethodInfo findMethod, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found") 
            => FindCodeInstruction(index, ref codes, findMethod, addCode, skip, requireInstance, notInstruction, andInstruction, orInstruction, errorMessage);

        public static int FindFloat(int index, ref List<CodeInstruction> codes, float findValue, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found") 
            => FindCodeInstruction(index, ref codes, findValue, addCode, skip, requireInstance, notInstruction, andInstruction, orInstruction, errorMessage);

        public static int FindInteger(int index, ref List<CodeInstruction> codes, sbyte findValue, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
            => FindCodeInstruction(index, ref codes, findValue, addCode, skip, requireInstance, notInstruction, andInstruction, orInstruction, errorMessage);

        public static int FindSub(int index, ref List<CodeInstruction> codes, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found")
            => FindCodeInstruction(index, ref codes, findValue: OpCodes.Sub, addCode, skip, notInstruction, andInstruction, orInstruction, requireInstance, errorMessage);

        public static int FindAdd(int index, ref List<CodeInstruction> codes, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found") 
            => FindCodeInstruction(index, ref codes, findValue: OpCodes.Add, addCode, skip, notInstruction, andInstruction, orInstruction, requireInstance, errorMessage);

        public static int FindMul(int index, ref List<CodeInstruction> codes, MethodInfo addCode = null, bool skip = false, bool notInstruction = false, bool andInstruction = false, bool orInstruction = false, bool requireInstance = false, string errorMessage = "Not found") 
            => FindCodeInstruction(index, ref codes, findValue: OpCodes.Mul, addCode, skip, notInstruction, andInstruction, orInstruction, requireInstance, errorMessage);

        private static bool CheckCodeInstruction(CodeInstruction code, Type localType, int localIndex, bool store = false)
        {
            if (!store)
            {
                return localIndex switch
                {
                    0 => code.opcode == OpCodes.Ldloc_0,
                    1 => code.opcode == OpCodes.Ldloc_1,
                    2 => code.opcode == OpCodes.Ldloc_2,
                    3 => code.opcode == OpCodes.Ldloc_3,
                    _ => code.opcode == OpCodes.Ldloc && code.operand.ToString() == $"{nameof(localType)} ({localIndex})",
                };
            }
            else
            {
                return localIndex switch
                {
                    0 => code.opcode == OpCodes.Stloc_0,
                    1 => code.opcode == OpCodes.Stloc_1,
                    2 => code.opcode == OpCodes.Stloc_2,
                    3 => code.opcode == OpCodes.Stloc_3,
                    _ => code.opcode == OpCodes.Stloc && code.operand.ToString() == $"{nameof(localType)} ({localIndex})",
                };
            }
        }

        private static bool CheckCodeInstruction(CodeInstruction code, object findValue)
        {
            if (findValue is sbyte)
            {
                bool result = CheckIntegerCodeInstruction(code, findValue);
                return result;
            }
            if (findValue is float)
            {
                bool result = code.opcode == OpCodes.Ldc_R4 && code.operand.Equals(findValue);
                return result;
            }
            if (findValue is string) return code.opcode == OpCodes.Ldstr && code.operand.Equals(findValue);
            if (findValue is MethodInfo) return (code.opcode == OpCodes.Call && code.operand == findValue);
            if (findValue is FieldInfo) return (code.opcode == OpCodes.Ldfld || code.opcode == OpCodes.Stfld) && code.operand == findValue;
            if (findValue is OpCode opCode) return (code.opcode == opCode);
            return false;
        }

        private static bool CheckIntegerCodeInstruction(CodeInstruction code, object findValue)
        {
            switch ((sbyte)findValue)
            {
                case 0: return code.opcode == OpCodes.Ldc_I4_0;
                case 1: return code.opcode == OpCodes.Ldc_I4_1;
                case 2: return code.opcode == OpCodes.Ldc_I4_2;
                case 3: return code.opcode == OpCodes.Ldc_I4_3;
                case 4: return code.opcode == OpCodes.Ldc_I4_4;
                case 5: return code.opcode == OpCodes.Ldc_I4_5;
                case 6: return code.opcode == OpCodes.Ldc_I4_6;
                case 7: return code.opcode == OpCodes.Ldc_I4_7;
                case 8: return code.opcode == OpCodes.Ldc_I4_8;
                default:
                    {
                        return code.opcode == OpCodes.Ldc_I4_S && code.operand.Equals(findValue);
                    }
            }
        }
    }
}
