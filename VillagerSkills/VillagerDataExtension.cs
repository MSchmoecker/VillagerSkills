namespace VillagerSkills {
    public static class VillagerDataExtension {
        private static readonly MonkeySkillData MonkeySkillData = new MonkeySkillData();

        public static ISkillData GetVillagerData(this Villager villager) {
            if (villager.Id == "trained_monkey") {
                return MonkeySkillData;
            }

            return VillagerSkillData.GetVillagerData(villager.UniqueId, 16);
        }

        public static ISkillData GetVillagerData(this Kid villager) {
            if (villager.Id == "trained_monkey") {
                return MonkeySkillData;
            }

            return VillagerSkillData.GetVillagerData(villager.UniqueId, 0);
        }
    }
}
