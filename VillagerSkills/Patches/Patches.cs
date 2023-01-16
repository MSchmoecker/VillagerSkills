using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace VillagerSkills {
    [HarmonyPatch]
    public static class Patches {
        [HarmonyPatch(typeof(GameDataLoader), MethodType.Constructor), HarmonyPostfix]
        public static void GameDataLoaderPatch(GameDataLoader __instance) {
            __instance.GetStringForBag(SetCardBag.BasicIdea);
            __instance.result[SetCardBag.BasicIdea] += ", blueprint_villagerskills_training_dummy";
        }

        [HarmonyPatch(typeof(Mob), nameof(Mob.CanHaveCard)), HarmonyPostfix]
        public static void TrainingDummyStackPatch(Mob __instance, ref bool __result, CardData otherCard) {
            if (__result && !(__instance is TrainingDummy) && otherCard is TrainingDummy) {
                __result = false;
            }
        }
    }
}
