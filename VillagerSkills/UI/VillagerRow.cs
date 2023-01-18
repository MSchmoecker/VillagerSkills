using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VillagerSkills.UI {
    public class VillagerRow : MonoBehaviour {
        public List<Column> Columns { get; } = new List<Column>();

        public class Column {
            private readonly TMP_Text text;
            private object cachedValue;

            public Column(TMP_Text text) {
                this.text = text;
            }

            public void Update(object newValue) {
                if (cachedValue != null && cachedValue.Equals(newValue)) {
                    return;
                }

                text.text = newValue.ToString();
                cachedValue = newValue;
            }
        }

        public void SpawnColumns(Villager villager, Transform parent) {
            SpawnColumn("Name", parent);
            SpawnColumn("Age", parent);

            foreach (Skill skill in SkillExtension.Skills) {
                SpawnColumn(skill.ToString(), parent);
            }

            if (villager) {
                UpdateColumns(villager);
            }
        }

        private void SpawnColumn(string text, Transform parent) {
            GameObject column = Instantiate(Mod.ColumnPrefab, parent);
            TMP_Text textComponent = column.transform.Find("Text").GetComponent<TMP_Text>();

            textComponent.text = text;
            textComponent.font = FontManager.instance.GetFont(FontType.Regular);

            Columns.Add(new Column(textComponent));
        }

        public void UpdateColumns(Villager villager) {
            VillagerData villagerData = villager.GetVillagerData();
            int index = 0;

            Columns[index++].Update(villager.Name);
            Columns[index++].Update(villagerData.Age);

            foreach (Skill skill in SkillExtension.Skills) {
                Columns[index++].Update(villagerData.GetLevel(skill));
            }
        }
    }
}
