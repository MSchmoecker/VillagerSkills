using System;
using System.Collections.Generic;

namespace VillagerSkills {
    public enum Skill {
        Woodcutting,
        Mining,
        Farming,
        Fishing,
        Exploring,
        Building,
        Combat,
    }

    public static class SkillExtension {
        private static readonly Dictionary<string, Skill> AttributeToSkillCache = new Dictionary<string, Skill>();

        static SkillExtension() {
            foreach (Skill skill in Enum.GetValues(typeof(Skill))) {
                AttributeToSkillCache.Add(SkillToAttributeName(skill), skill);
            }
        }

        public static bool SkillFromAttribute(this string attributeName, out Skill skill) {
            return AttributeToSkillCache.TryGetValue(attributeName, out skill);
        }

        public static string SkillToAttribute(this Skill skill) {
            return SkillToAttributeName(skill);
        }

        private static string SkillToAttributeName(Skill skill) {
            return $"vl_xp_{skill.ToString().ToLower()}";
        }
    }
}
