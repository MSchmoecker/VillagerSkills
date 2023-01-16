using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace VillagerSkills {
    [HarmonyPatch]
    public static class CardDataPatches {
        [HarmonyPatch(typeof(CardData), nameof(CardData.SetExtraCardData), typeof(object), typeof(List<ExtraCardData>)), HarmonyPostfix]
        public static void SetExtraCardData(object o, List<ExtraCardData> extraData) {
            if (o is Villager villager) {
                villager.GetVillagerData().SetExtraCardData(extraData);
            }
        }

        [HarmonyPatch(typeof(CardData), nameof(CardData.GetExtraCardData), typeof(object)), HarmonyPostfix]
        public static void GetExtraCardData(object o, ref List<ExtraCardData> __result) {
            if (o is Villager villager) {
                __result.AddRange(villager.GetVillagerData().GetExtraCardData());
            }
        }

        [HarmonyPatch(typeof(House), nameof(House.GrowUpKid)), HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> GrowUpCopyCustomData(IEnumerable<CodeInstruction> instructions) {
            return new CodeMatcher(instructions)
                   .MatchForward(true, new CodeMatch(i => i.IsVirtCall(nameof(WorldManager), nameof(WorldManager.CreateCard))))
                   .Advance(1)
                   .InsertAndAdvance(
                       new CodeInstruction(OpCodes.Dup), // after CreateCard, villager is on top of the stack
                       new CodeInstruction(OpCodes.Ldloc_0), // load the kid
                       Transpilers.EmitDelegate<Action<Villager, Kid>>(CopyCustomData)) // execute our custom method with the villager and kid
                   .InstructionEnumeration();
        }

        private static void CopyCustomData(Villager villager, Kid kid) {
            kid.GetVillagerData().CopyTo(villager);
        }
    }
}
