using System;

namespace VillagerLevel {
    public static class CardSkill {
        public static Skill GetSkill(CardData cardData) {
            return Get(cardData).Item1;
        }

        public static Tuple<Skill, float> GetSkillXp(CardData cardData) {
            return Get(cardData);
        }

        private static Tuple<Skill, float> Get(CardData cardData) {
            string id = cardData.Id;

            if (cardData.MyCardType == CardType.Locations) {
                return new Tuple<Skill, float>(Skill.Exploring, 5f);
            }

            if (id == "apple_tree" || id == "berrybush" || id == "banana_tree" || id == "cotton_plant" || id == "sugar_cane") {
                return new Tuple<Skill, float>(Skill.Farming, 5f);
            }

            if (id == "tree" || id == "lumbercamp" || id == "driftwood") {
                return new Tuple<Skill, float>(Skill.Woodcutting, 5f);
            }

            if (id == "rock" || id == "iron_deposit" || id == "mine" || id == "quarry" || id == "gold_deposit" || id == "sand_quarry" || id == "gold_mine") {
                return new Tuple<Skill, float>(Skill.Mining, 5f);
            }

            if (id == "fishing_spot") {
                return new Tuple<Skill, float>(Skill.Fishing, 5f);
            }

            Log.LogWarning("Unknown skill with card: " + id);
            return new Tuple<Skill, float>(Skill.Crafting, 5f);
        }
    }
}
