using System.Collections.Generic;

namespace VillagerSkills {
    public class MonkeySkillData : ISkillData {
        public int Age { get; set; } = 0;

        public int GetLevel(Skill skill) {
            return 0;
        }

        public void AddExperience(Skill skill, float xp) {
        }

        public void SetExperience(Skill skill, float xp) {
        }

        public float GetActionTimeModifier(Skill skill) {
            return 1.5f;
        }

        public IEnumerable<ExtraCardData> GetExtraCardData() {
            yield break;
        }

        public void SetExtraCardData(List<ExtraCardData> extraData) {
        }

        public string GetDescription() {
            return string.Empty;
        }

        public void CopyTo(Villager villager) {
        }
    }
}
