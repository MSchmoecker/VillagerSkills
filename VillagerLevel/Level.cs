using System;
using System.Collections.Generic;
using UnityEngine;

namespace VillagerLevel {
    public class Level {
        public float WoodcuttingXp { get; set; }
        public float MiningXp { get; set; }
        public float FarmingXp { get; set; }
        public float CraftingXp { get; set; }
        public float FishingXp { get; set; }
        public float ExploringXp { get; set; }
        public float BuildingXp { get; set; }

        private static readonly Dictionary<Villager, Level> VillagerLevels = new Dictionary<Villager, Level>();

        public static Level GetVillagerLevel(Villager villager) {
            if (VillagerLevels.TryGetValue(villager, out Level level)) {
                return level;
            }

            level = new Level();
            VillagerLevels[villager] = level;
            return level;
        }

        public void AddExperience(Skill skill, float xp) {
            switch (skill) {
                case Skill.Woodcutting:
                    WoodcuttingXp += xp;
                    break;
                case Skill.Mining:
                    MiningXp += xp;
                    break;
                case Skill.Farming:
                    FarmingXp += xp;
                    break;
                case Skill.Crafting:
                    CraftingXp += xp;
                    break;
                case Skill.Fishing:
                    FishingXp += xp;
                    break;
                case Skill.Exploring:
                    ExploringXp += xp;
                    break;
                case Skill.Building:
                    BuildingXp += xp;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(skill), skill, null);
            }
        }

        public int GetLevel(Skill skill) {
            switch (skill) {
                case Skill.Woodcutting:
                    return GetLevel(WoodcuttingXp);
                case Skill.Mining:
                    return GetLevel(MiningXp);
                case Skill.Farming:
                    return GetLevel(FarmingXp);
                case Skill.Crafting:
                    return GetLevel(CraftingXp);
                case Skill.Fishing:
                    return GetLevel(FishingXp);
                case Skill.Exploring:
                    return GetLevel(ExploringXp);
                case Skill.Building:
                    return GetLevel(BuildingXp);
                default:
                    throw new ArgumentOutOfRangeException(nameof(skill), skill, null);
            }
        }

        public IEnumerable<ExtraCardData> GetExtraCardData() {
            yield return new ExtraCardData("vl_xp_crafting", CraftingXp);
            yield return new ExtraCardData("vl_xp_exploring", ExploringXp);
            yield return new ExtraCardData("vl_xp_farming", FarmingXp);
            yield return new ExtraCardData("vl_xp_woodcutting", WoodcuttingXp);
            yield return new ExtraCardData("vl_xp_mining", MiningXp);
            yield return new ExtraCardData("vl_xp_fishing", FishingXp);
            yield return new ExtraCardData("vl_xp_building", BuildingXp);
        }

        public void SetExtraCardData(List<ExtraCardData> extraData) {
            foreach (ExtraCardData data in extraData) {
                switch (data.AttributeId) {
                    case "vl_xp_crafting":
                        CraftingXp = data.FloatValue;
                        break;
                    case "vl_xp_exploring":
                        ExploringXp = data.FloatValue;
                        break;
                    case "vl_xp_farming":
                        FarmingXp = data.FloatValue;
                        break;
                    case "vl_xp_woodcutting":
                        WoodcuttingXp = data.FloatValue;
                        break;
                    case "vl_xp_mining":
                        MiningXp = data.FloatValue;
                        break;
                    case "vl_xp_fishing":
                        FishingXp = data.FloatValue;
                        break;
                    case "vl_xp_building":
                        BuildingXp = data.FloatValue;
                        break;
                }
            }
        }

        public static float GetActionTime(int level) {
            return 0.5f + Mathf.Exp(-0.23f * level);
        }

        private static int GetLevel(float xp) {
            return (int)Mathf.Floor(Mathf.Pow(xp / 5f, 0.7f) / 2f);
        }

        private static string GetLevelString(string profession, float xp) {
            return $"{profession} Level {GetLevel(xp)}, {xp}";
        }

        public string GetDescription() {
            return $"{Environment.NewLine}{Environment.NewLine}" +
                   $"{GetLevelString("Woodcutting", WoodcuttingXp)}{Environment.NewLine}" +
                   $"{GetLevelString("Mining", MiningXp)}{Environment.NewLine}" +
                   $"{GetLevelString("Farming", FarmingXp)}{Environment.NewLine}" +
                   $"{GetLevelString("Crafting", CraftingXp)}{Environment.NewLine}" +
                   $"{GetLevelString("Exploring", ExploringXp)}{Environment.NewLine}" +
                   $"{GetLevelString("Fishing", FishingXp)}{Environment.NewLine}" +
                   $"{GetLevelString("Building", BuildingXp)}";
        }
    }
}
