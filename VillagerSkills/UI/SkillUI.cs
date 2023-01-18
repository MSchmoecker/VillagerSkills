using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VillagerSkills.UI {
    public class SkillUI : MonoBehaviour {
        public Image background;
        public Transform headerParent;
        public Transform scrollParent;

        public static SkillUI Instance { get; private set; }

        private void Awake() {
            Instance = this;

            Sprite sketchyBox = Resources.FindObjectsOfTypeAll<Sprite>().FirstOrDefault(i => i.name == "sketchy_box_2");

            Mod.ColumnPrefab.transform.Find("Background").GetComponent<Image>().sprite = sketchyBox;
            background.sprite = sketchyBox;

            RebuildHeader();
            RebuildRows();
        }

        private void RebuildHeader() {
            foreach (Transform child in headerParent) {
                Destroy(child.gameObject);
            }

            Skill[] skills = Enum.GetValues(typeof(Skill)).Cast<Skill>().ToArray();

            RectTransform headerRow = (RectTransform)Instantiate(Mod.RowPrefab, headerParent).transform;
            SpawnColumn("Name", headerRow);
            SpawnColumn("Age", headerRow);

            foreach (Skill skill in skills) {
                SpawnColumn(skill.ToString(), headerRow);
            }
        }

        public void RebuildRows() {
            foreach (Transform child in scrollParent) {
                Destroy(child.gameObject);
            }

            List<Villager> villagers = WorldManager.instance.GetCards<Villager>();

            foreach (Villager villager in villagers) {
                VillagerData villagerData = villager.GetVillagerData();

                RectTransform row = (RectTransform)Instantiate(Mod.RowPrefab, scrollParent).transform;
                SpawnColumn(villager.Name, row);
                SpawnColumn(villagerData.Age.ToString(), row);

                foreach (Skill skill in Enum.GetValues(typeof(Skill))) {
                    SpawnColumn(villagerData.GetLevel(skill).ToString(), row);
                }
            }
        }

        private static void SpawnColumn(string text, Transform parent) {
            GameObject column = Instantiate(Mod.ColumnPrefab, parent);
            TMP_Text textComponent = column.transform.Find("Text").GetComponent<TMP_Text>();

            textComponent.text = text;
            textComponent.font = FontManager.instance.GetFont(FontType.Regular);
        }
    }
}
