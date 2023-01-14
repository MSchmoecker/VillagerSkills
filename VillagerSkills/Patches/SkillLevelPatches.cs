using HarmonyLib;

namespace VillagerSkills {
    [HarmonyPatch]
    public static class SkillLevelPatches {
        [HarmonyPatch(typeof(Villager), nameof(Villager.GetActionTimeModifier)), HarmonyPostfix]
        public static void GetActionTimeModifier(Villager __instance, CardData baseCard, ref float __result) {
            Skill skill = CardSkill.GetSkill(baseCard);
            __result *= __instance.GetVillagerData().GetActionTimeModifier(skill);
        }

        [HarmonyPatch(typeof(CardData), nameof(CardData.Description), MethodType.Getter), HarmonyPostfix]
        public static void Description(CardData __instance, ref string __result) {
            if (__instance is Villager villager) {
                __result = __result.Trim() + villager.GetVillagerData().GetDescription();
            }
        }
    }
}
