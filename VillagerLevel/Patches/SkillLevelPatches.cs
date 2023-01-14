using HarmonyLib;

namespace VillagerLevel {
    [HarmonyPatch]
    public static class SkillLevelPatches {
        [HarmonyPatch(typeof(Villager), nameof(Villager.GetActionTimeModifier)), HarmonyPostfix]
        public static void GetActionTimeModifier(Villager __instance, CardData baseCard, ref float __result) {
            VillagerLevel villagerLevel = VillagerLevel.GetVillagerLevel(__instance);
            Skill skill = CardSkill.GetSkill(baseCard);
            __result *= villagerLevel.GetActionTimeModifier(skill);
        }

        [HarmonyPatch(typeof(CardData), nameof(CardData.Description), MethodType.Getter), HarmonyPostfix]
        public static void Description(CardData __instance, ref string __result) {
            if (__instance is Villager villager) {
                __result = __result.Trim() + VillagerLevel.GetVillagerLevel(villager).GetDescription();
            }
        }
    }
}
