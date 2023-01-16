namespace VillagerSkills {
    public static class VillagerDataExtension {
        public static VillagerData GetVillagerData(this Villager villager) {
            return VillagerData.GetVillagerData(villager.UniqueId, 16);
        }

        public static VillagerData GetVillagerData(this Kid villager) {
            return VillagerData.GetVillagerData(villager.UniqueId, 0);
        }
    }
}
