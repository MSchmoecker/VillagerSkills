using HarmonyLib;
using UnityEngine;

namespace VillagerSkills {
    [HarmonyPatch]
    public static class CombatPatches {
        private static readonly SpecialHitType GrazeHitType = (SpecialHitType)"villager_skills_graze".GetHashCode();

        [HarmonyPatch(typeof(Combatable), nameof(Combatable.PerformAttack)), HarmonyPrefix]
        public static bool PerformAttackPrefix(Combatable __instance, Combatable target, Vector3 attackPos) {
            if (!__instance.AttackIsHit || !(__instance is Villager villager)) {
                return true;
            }

            float grazeChance = GetGrazeChance(villager, target);
            bool doGraze = Random.value < grazeChance;
            int grazeDamage = Mathf.RoundToInt(Random.value);

            Log.LogInfo($"graze chance: {grazeChance}, do graze: {doGraze}, graze damage: {grazeDamage}");

            if (doGraze) {
                target.Damage(grazeDamage);
                __instance.ShowHitText(__instance, target, attackPos, __instance.AttackIsHit, grazeDamage, GrazeHitType);
                return false;
            }

            return true;
        }

        [HarmonyPatch(typeof(Combatable), nameof(Combatable.ShowHitText)), HarmonyPrefix]
        public static bool DisplayGraze(Combatable origin, Combatable effectTarget, Vector3 targetPosition, bool isHit, int damage, SpecialHitType? type) {
            if (type == GrazeHitType) {
                if (!isHit) {
                    effectTarget.CreateHitText("miss", PrefabManager.instance.MissHitText).transform.position = targetPosition;
                    AudioManager.me.PlaySound2D(AudioManager.me.Miss, Random.Range(0.8f, 1.2f), 0.5f);
                } else if (damage > 0) {
                    effectTarget.CreateHitText("graze").SetVeryEffective(false);
                    AudioManager.me.PlaySound2D(origin.GetAttackTypeHitSound(), Random.Range(0.8f, 1.2f), 0.2f);
                } else {
                    effectTarget.CreateHitText("block", PrefabManager.instance.BlockHitText);
                    AudioManager.me.PlaySound2D(AudioManager.me.Block, Random.Range(0.8f, 1.2f), 0.2f);
                }

                return false;
            }

            return true;
        }

        [HarmonyPatch(typeof(Combatable), nameof(Combatable.ShowHitText)), HarmonyPostfix]
        public static void IncreaseCombatSkill(Combatable origin, bool isHit, int damage) {
            if (isHit && damage > 0 && origin is Villager villager) {
                villager.GetVillagerData().AddExperience(Skill.Combat, damage);
            }
        }

        private static float GetGrazeChance(Combatable origin, Combatable target) {
            if (origin is Villager villager) {
                int combatLevel = villager.GetVillagerData().GetLevel(Skill.Combat);
                int targetDefence = CombatStats.GetDefenceFromEnum(target.ProcessedCombatStats.Defence);
                return GetGrazeChance(targetDefence, combatLevel);
            }

            return 0;
        }

        private static float GetGrazeChance(int targetDefence, int combatLevel) {
            return Mathf.Clamp01(0.6f + targetDefence / 4f - combatLevel / 6f);
        }
    }
}
