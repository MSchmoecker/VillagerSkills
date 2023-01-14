using System;
using HarmonyLib;

namespace VillagerSkills {
    [HarmonyPatch]
    public static class ExperiencePatches {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CardData), nameof(CardData.FinishBlueprint))]
        public static void FinishBlueprint(CardData __instance) {
            foreach (GameCard gameCard in __instance.MyGameCard.GetAllCardsInStack()) {
                if (gameCard.CardData is Villager villager) {
                    if (__instance.MyGameCard.TimerActionId == "finish_blueprint") {
                        villager.GetVillagerData().AddExperience(Skill.Building, 5);
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
                    Tuple<Skill, float> skill = CardSkill.GetSkillXp(__instance);
                    villager.GetVillagerData().AddExperience(skill.Item1, skill.Item2);
                }
            }
        }
    }
}
