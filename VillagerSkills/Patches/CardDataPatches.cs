using System.Collections.Generic;
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
    }
}
