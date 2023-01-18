using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VillagerSkills.UI {
    public class VillagerRow : MonoBehaviour {
        public List<VillagerColumn> Columns { get; } = new List<VillagerColumn>();
        private bool isHeader;
        private string villagerUniqueId;

        public void SpawnColumns(Villager villager, Transform parent) {
            SpawnColumn("Name", parent);
            SpawnColumn("Age", parent);

            foreach (Skill skill in SkillExtension.Skills) {
                SpawnColumn(skill.ToString(), parent);
            }

            isHeader = !villager;

            if (villager) {
                UpdateColumns(villager);
                villagerUniqueId = villager.UniqueId;
            }
        }

        private void SpawnColumn(string text, Transform parent) {
            GameObject column = Instantiate(Mod.ColumnPrefab, parent);
            TMP_Text textComponent = column.transform.Find("Text").GetComponent<TMP_Text>();

            textComponent.text = text;
            textComponent.font = FontManager.instance.GetFont(FontType.Regular);

            VillagerColumn villagerColumn = column.AddComponent<VillagerColumn>();
            villagerColumn.Init(textComponent, this);
            Columns.Add(villagerColumn);
        }

        public void UpdateColumns(Villager villager) {
            VillagerData villagerData = villager.GetVillagerData();
            int index = 0;

            Columns[index++].UpdateValue(villager.Name);
            Columns[index++].UpdateValue(villagerData.Age);

            foreach (Skill skill in SkillExtension.Skills) {
                Columns[index++].UpdateValue(villagerData.GetLevel(skill));
            }
        }

        public void OnClick(VillagerColumn column) {
            if (isHeader) {
                SortRows(column);
            } else {
                JumpToVillager();
            }
        }

        private void JumpToVillager() {
            GameCard card = WorldManager.instance.GetCardWithUniqueId(villagerUniqueId);

            if (card) {
                Vector3 cardPos = card.transform.position;
                Vector3 targetPos = new Vector3(cardPos.x, 7f, cardPos.z - 1f);
                GameCamera.instance.cameraTargetPosition = targetPos;
            }
        }

        private void SortRows(VillagerColumn column) {
            SkillUI.Instance.SetSortColumn(Columns.IndexOf(column));
        }
    }
}
