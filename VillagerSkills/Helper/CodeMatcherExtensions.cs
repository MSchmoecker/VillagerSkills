using System;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace VillagerSkills {
    public static class CodeMatcherExtensions {
        public static CodeMatcher GetPosition(this CodeMatcher codeMatcher, out int position) {
            position = codeMatcher.Pos;
            return codeMatcher;
        }

        public static CodeMatcher AddLabel(this CodeMatcher codeMatcher, out Label label) {
            label = new Label();
            codeMatcher.AddLabels(new[] { label });
            return codeMatcher;
        }

        public static CodeMatcher GetOperand(this CodeMatcher codeMatcher, out object operand) {
            operand = codeMatcher.Operand;
            return codeMatcher;
        }

        internal static CodeMatcher Print(this CodeMatcher codeMatcher, int before, int after) {
            for (int i = -before; i <= after; ++i) {
                int index = codeMatcher.Pos + i;

                if (index <= 0) {
                    continue;
                }

                if (index >= codeMatcher.Length) {
                    break;
                }

                try {
                    CodeInstruction line = codeMatcher.InstructionAt(i);
                    Log.LogCodeInstruction($"[{i}] " + line.ToString());
                } catch (Exception e) {
                    Log.LogCodeInstruction(e.Message);
                }
            }

            return codeMatcher;
        }

        internal static CodeMatcher PrintInstruction(this CodeMatcher codeMatcher) {
            try {
                CodeInstruction instruction = codeMatcher.Instruction;
                Log.LogCodeInstruction($"{instruction.opcode} {instruction.operand} [{instruction.operand.GetType()}]");
            } catch (Exception e) {
                Log.LogCodeInstruction(e.Message);
            }

            return codeMatcher;
        }

        public static bool IsVirtCall(this CodeInstruction i, string declaringType, string name) {
            return i.opcode == OpCodes.Callvirt && i.operand is MethodInfo methodInfo && methodInfo.DeclaringType?.Name == declaringType && methodInfo.Name == name;
        }
    }
}
