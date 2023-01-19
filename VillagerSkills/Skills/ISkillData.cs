using System.Collections.Generic;

namespace VillagerSkills {
    public interface ISkillData {
        int Age { get; set; }
        int GetLevel(Skill skill);
        void AddExperience(Skill skill, float xp);
        void SetExperience(Skill skill, float xp);
        float GetActionTimeModifier(Skill skill);
        IEnumerable<ExtraCardData> GetExtraCardData();
        void SetExtraCardData(List<ExtraCardData> extraData);
        string GetDescription();
        void CopyTo(Villager villager);
    }
}
