using System;
using HarmonyLib;

namespace VillagerLevel {
    [HarmonyPatch]
    public static class ExperiencePatches {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CardData), nameof(CardData.FinishBlueprint))]
        public static void FinishBlueprint(CardData __instance) {
            foreach (GameCard gameCard in __instance.MyGameCard.GetAllCardsInStack()) {
                if (gameCard.CardData is Villager villager) {
                    VillagerLevel villagerLevel = VillagerLevel.GetVillagerLevel(villager);

                    if (__instance.MyGameCard.TimerActionId == "finish_blueprint") {
                        villagerLevel.AddExperience(Skill.Building, 5);
                    } else {
                        villagerLevel.AddExperience(Skill.Crafting, 5);
                    }
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Harvestable), nameof(Harvestable.CompleteHarvest))]
        [HarmonyPatch(typeof(CombatableHarvestable), nameof(CombatableHarvestable.CompleteHarvest))]
        public static void CompleteHarvest(CardData __instance) {
            foreach (GameCard gameCard in __instance.MyGameCard.GetAllCardsInStack()) {
                if (gameCard.CardData is Villager villager) {
                    VillagerLevel villagerLevel = VillagerLevel.GetVillagerLevel(villager);
                    Tuple<Skill, float> skill = CardSkill.GetSkillXp(__instance);
                    villagerLevel.AddExperience(skill.Item1, skill.Item2);
                }
            }
        }
    }
}
