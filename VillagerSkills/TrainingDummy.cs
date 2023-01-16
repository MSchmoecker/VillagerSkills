using System.Collections.Generic;

namespace VillagerSkills {
    public class TrainingDummy : Mob {
        public override void Awake() {
            base.Awake();

            CanAttack = true;
            CanHaveInventory = false;
            PossibleEquipables = new List<Equipable>();
            AttackAnimations = new List<AttackAnimation>();

            Drops = new CardBag() {
                Chances = new List<CardChance>(),
                SetPackCards = new List<string>(),
            };

            BaseCombatStats = new CombatStats() {
                MaxHealth = 20,
                AttackSpeed = AttackSpeed.VerySlow,
                HitChance = HitChance.VerySmall,
                AttackDamage = AttackDamage.VeryWeak,
                Defence = Defence.VeryWeak,
                AttackSpeedIncrement = 0,
                HitChanceIncrement = 0,
                AttackDamageIncrement = 0,
                DefenceIncrement = 0,
                SpecialHits = new List<SpecialHit>(),
            };

            HealthPoints = BaseCombatStats.MaxHealth;
        }

        public override bool CanBeDragged => true;

        public override bool CanMove => false;

        public override bool CanHaveCard(CardData otherCard) => otherCard is TrainingDummy || otherCard is Villager;
    }
}
