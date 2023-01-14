using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VillagerLevel {
    public class VillagerData {
        private readonly Dictionary<Skill, float> experience = new Dictionary<Skill, float>();

        private static readonly Dictionary<string, Skill> AttributeToSkill = new Dictionary<string, Skill>();
        private static readonly Dictionary<string, VillagerData> VillagerDataCache = new Dictionary<string, VillagerData>();

        static VillagerData() {
            foreach (Skill skill in Enum.GetValues(typeof(Skill))) {
                AttributeToSkill.Add(SkillToAttributeName(skill), skill);
            }
        }

        private VillagerData() {
            foreach (Skill skill in Enum.GetValues(typeof(Skill))) {
                experience.Add(skill, 0);
            }
        }

        public static VillagerData GetVillagerData(string uniqueId) {
            if (VillagerDataCache.TryGetValue(uniqueId, out VillagerData villagerData)) {
                return villagerData;
            }

            villagerData = new VillagerData();
            VillagerDataCache[uniqueId] = villagerData;
            return villagerData;
        }

        public void AddExperience(Skill skill, float xp) {
            experience[skill] += xp;
        }

        public IEnumerable<ExtraCardData> GetExtraCardData() {
            return experience.Select(pair => new ExtraCardData(SkillToAttributeName(pair.Key), pair.Value));
        }

        public void SetExtraCardData(List<ExtraCardData> extraData) {
            foreach (ExtraCardData data in extraData) {
                if (AttributeToSkill.TryGetValue(data.AttributeId, out Skill skill)) {
                    experience[skill] = data.FloatValue;
                }
            }
        }

        public float GetActionTimeModifier(Skill skill) {
            return 0.5f + Mathf.Exp(-0.23f * GetLevel(skill));
        }

        public int GetLevel(Skill skill) {
            return (int)Mathf.Floor(Mathf.Pow(experience[skill] / 5f, 0.7f) / 2f);
        }

        private static string SkillToAttributeName(Skill skill) {
            return $"vl_xp_{skill.ToString().ToLower()}";
        }

        private string GetLevelString(Skill skill, float xp) {
            return $"{skill.ToString()} Level {GetLevel(skill)}, {xp}";
        }

        public string GetDescription() {
            return $"{Environment.NewLine}{Environment.NewLine}" +
                   string.Join(Environment.NewLine, experience.Select(pair => GetLevelString(pair.Key, pair.Value)));
        }
    }
}
