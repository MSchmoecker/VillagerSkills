using System;
using System.IO;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using VillagerSkills.UI;
using Object = UnityEngine.Object;

namespace VillagerSkills {
    [HarmonyPatch]
    public static class UIPatches {
        [HarmonyPatch(typeof(GameScreen), nameof(GameScreen.Awake)), HarmonyPostfix]
        public static void GameScreenAwakePatch(GameScreen __instance) {
            Object.Instantiate(Mod.SkillUIPrefab, __instance.transform);
        }
    }
}
