using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace VillagerSkills.UI {
    public class SkillUI : MonoBehaviour {
        public Image background;
        public Transform headerParent;
        public Transform scrollParent;

        public static SkillUI Instance { get; private set; }

        private readonly Dictionary<string, VillagerRow> rows = new Dictionary<string, VillagerRow>();

        private void Awake() {
            Instance = this;

            Sprite sketchyBox = Resources.FindObjectsOfTypeAll<Sprite>().FirstOrDefault(i => i.name == "sketchy_box_2");

            Mod.ColumnPrefab.transform.Find("Background").GetComponent<Image>().sprite = sketchyBox;
            background.sprite = sketchyBox;

            RebuildHeader();
            RebuildRows();
        }

        private void Update() {
            if (Time.frameCount % 5 == 0) {
                RefreshRows();
            }
        }

        private void RebuildHeader() {
            foreach (Transform child in headerParent) {
                Destroy(child.gameObject);
            }

            SpawnRow(null, headerParent);
        }

        public void RebuildRows() {
            foreach (Transform child in scrollParent) {
                Destroy(child.gameObject);
            }

            List<Villager> villagers = WorldManager.instance.GetCards<Villager>();

            foreach (Villager villager in villagers) {
                rows.Add(villager.UniqueId, SpawnRow(villager, scrollParent));
            }
        }

        private void RefreshRows() {
            List<Villager> villagers = WorldManager.instance.GetCards<Villager>();
            Dictionary<string, VillagerRow> remainingRows = new Dictionary<string, VillagerRow>(rows);

            foreach (Villager villager in villagers) {
                if (rows.ContainsKey(villager.UniqueId)) {
                    rows[villager.UniqueId].UpdateColumns(villager);
                    remainingRows.Remove(villager.UniqueId);
                } else {
                    rows.Add(villager.UniqueId, SpawnRow(villager, scrollParent));
                }
            }

            foreach (string row in new List<string>(remainingRows.Keys)) {
                Destroy(remainingRows[row].gameObject);
                rows.Remove(row);
            }
        }

        private VillagerRow SpawnRow(Villager villager, Transform parent) {
            RectTransform row = (RectTransform)Instantiate(Mod.RowPrefab, parent).transform;
            VillagerRow villagerRow = row.gameObject.AddComponent<VillagerRow>();
            villagerRow.SpawnColumns(villager, row);
            return villagerRow;
        }
    }
}
