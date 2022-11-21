using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VillagerLevel {
    public class VillagerLevel {
        private readonly Dictionary<Skill, float> experience = new Dictionary<Skill, float>();

        private static readonly Dictionary<string, Skill> AttributeToSkill = new Dictionary<string, Skill>();
        private static readonly Dictionary<Villager, VillagerLevel> VillagerLevels = new Dictionary<Villager, VillagerLevel>();

        static VillagerLevel() {
            foreach (Skill skill in Enum.GetValues(typeof(Skill))) {
                AttributeToSkill.Add(SkillToAttributeName(skill), skill);
            }
        }

        private VillagerLevel() {
            foreach (Skill skill in Enum.GetValues(typeof(Skill))) {
                experience.Add(skill, 0);
            }
        }

        public static VillagerLevel GetVillagerLevel(Villager villager) {
            if (VillagerLevels.TryGetValue(villager, out VillagerLevel level)) {
                return level;
            }

            level = new VillagerLevel();
            VillagerLevels[villager] = level;
            return level;
        }

        public void AddExperience(Skill skill, float xp) {
            experience[skill] += xp;
        }

        public IEnumerable<ExtraCardData> GetExtraCardData() {
            return experience.Select(pair => new ExtraCardData(SkillToAttributeName(pair.Key), GetLevel(pair.Key)));
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
