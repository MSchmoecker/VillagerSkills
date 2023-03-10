using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VillagerSkills {
    public class VillagerSkillData : ISkillData {
        private readonly Dictionary<Skill, float> experience = new Dictionary<Skill, float>();
        public int Age { get; set; } = -1;

        private static readonly Dictionary<string, VillagerSkillData> VillagerDataCache = new Dictionary<string, VillagerSkillData>();

        private VillagerSkillData(int defaultAge) {
            if (Age < 0) {
                Age = defaultAge;
            }

            foreach (Skill skill in SkillExtension.Skills) {
                experience.Add(skill, 0);
            }
        }

        public static VillagerSkillData GetVillagerData(string uniqueId, int defaultAge) {
            if (VillagerDataCache.TryGetValue(uniqueId, out VillagerSkillData villagerData)) {
                return villagerData;
            }

            villagerData = new VillagerSkillData(defaultAge);
            VillagerDataCache[uniqueId] = villagerData;
            return villagerData;
        }

        public void AddExperience(Skill skill, float xp) {
            experience[skill] += xp;
        }

        public void SetExperience(Skill skill, float xp) {
            experience[skill] = xp;
        }

        public IEnumerable<ExtraCardData> GetExtraCardData() {
            foreach (ExtraCardData extraCardData in experience.Select(pair => new ExtraCardData(pair.Key.SkillToAttribute(), pair.Value))) {
                yield return extraCardData;
            }

            yield return new ExtraCardData($"vl_age", Age);
        }

        public void SetExtraCardData(List<ExtraCardData> extraData) {
            foreach (ExtraCardData data in extraData) {
                if (data.AttributeId.SkillFromAttribute(out Skill skill)) {
                    experience[skill] = data.FloatValue;
                }

                if (data.AttributeId == "vl_age") {
                    Age = data.IntValue;
                }
            }
        }

        public float GetActionTimeModifier(Skill skill) {
            return 0.5f + Mathf.Exp(-0.23f * GetLevel(skill));
        }

        public int GetLevel(Skill skill) {
            return (int)Mathf.Floor(Mathf.Pow(experience[skill] / 5f, 0.7f) / 2f);
        }

        private string GetLevelString(Skill skill, float xp) {
            return $"{skill.ToString()} {GetLevel(skill)}";
        }

        public string GetDescription() {
            return $"{Environment.NewLine}" +
                   $"<i>Age {Age} Moons</i>" +
                   $"{Environment.NewLine}" +
                   $"{Environment.NewLine}" +
                   string.Join(Environment.NewLine, experience.Select(pair => GetLevelString(pair.Key, pair.Value)));
        }

        public void CopyTo(Villager villager) {
            ISkillData other = villager.GetVillagerData();

            foreach (Skill skill in SkillExtension.Skills) {
                other.SetExperience(skill, experience[skill]);
            }
        }
    }
}
