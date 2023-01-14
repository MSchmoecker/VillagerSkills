using System.Collections;
using HarmonyLib;

namespace VillagerLevel {
    [HarmonyPatch]
    public static class AgePatches {
        [HarmonyPatch(typeof(EndOfMonthCutscenes), nameof(EndOfMonthCutscenes.FeedVillagers)), HarmonyPostfix]
        public static IEnumerator FeedVillagersPatch(IEnumerator result) {
            yield return Cutscenes.WaitForContinueClicked("The villagers are getting older");

            foreach (Villager villager in WorldManager.instance.GetCards<Villager>()) {
                villager.GetVillagerData().Age += 1;

                if (villager.GetVillagerData().Age > 46) {
                    GameCamera.instance.TargetPositionOverride = villager.transform.position;
                    EndOfMonthCutscenes.CutsceneText = $"{villager.Name} died of old age!";
                    yield return WorldManager.instance.KillVillagerCoroutine(villager, null, null);
                }
            }

            do {
                yield return result.Current;
            } while (result.MoveNext());
        }
    }
}
