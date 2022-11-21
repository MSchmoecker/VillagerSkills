using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace VillagerLevel {
    [HarmonyPatch]
    public static class Patches {
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

        [HarmonyPatch(typeof(Villager), nameof(Villager.GetActionTimeModifier)), HarmonyPostfix]
        public static void GetActionTimeModifier(Villager __instance, CardData baseCard, ref float __result) {
            VillagerLevel villagerLevel = VillagerLevel.GetVillagerLevel(__instance);
            Skill skill = CardSkill.GetSkill(baseCard);
            __result *= villagerLevel.GetActionTimeModifier(skill);
        }

        [HarmonyPatch(typeof(CardData), nameof(CardData.SetExtraCardData), typeof(object), typeof(List<ExtraCardData>)), HarmonyPostfix]
        public static void SetExtraCardData(object o, List<ExtraCardData> extraData) {
            if (o is Villager villager) {
                VillagerLevel.GetVillagerLevel(villager).SetExtraCardData(extraData);
            }
        }

        [HarmonyPatch(typeof(CardData), nameof(CardData.GetExtraCardData), typeof(object)), HarmonyPostfix]
        public static void GetExtraCardData(object o, ref List<ExtraCardData> __result) {
            if (o is Villager villager) {
                __result.AddRange(VillagerLevel.GetVillagerLevel(villager).GetExtraCardData());
            }
        }

        [HarmonyPatch(typeof(CardData), nameof(CardData.Description), MethodType.Getter), HarmonyPostfix]
        public static void Description(CardData __instance, ref string __result) {
            if (__instance is Villager villager) {
                __result = __result.Trim() + VillagerLevel.GetVillagerLevel(villager).GetDescription();
            }
        }
    }
}
