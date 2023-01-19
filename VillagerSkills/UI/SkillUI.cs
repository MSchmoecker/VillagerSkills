using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace VillagerSkills.UI {
    public class SkillUI : MonoBehaviour {
        public Image background;
        public Transform headerParent;
        public Transform scrollParent;
        public CustomButton foldButton;

        private Transform foldButtonIcon;
        private bool foldButtonWasClicked;
        private bool isExpanded = true;
        private RectTransform selfRect;

        public static SkillUI Instance { get; private set; }

        private readonly Dictionary<string, VillagerRow> rows = new Dictionary<string, VillagerRow>();
        private int sortColumn = 1;

        private void Awake() {
            Instance = this;
            selfRect = GetComponent<RectTransform>();

            Sprite sketchyBox = Resources.FindObjectsOfTypeAll<Sprite>().FirstOrDefault(i => i.name == "sketchy_box_2");
            Sprite minimizeButton = Resources.FindObjectsOfTypeAll<Sprite>().FirstOrDefault(i => i.name == "minimizebutton");

            Mod.ColumnPrefab.transform.Find("Background").GetComponent<Image>().sprite = sketchyBox;
            background.sprite = sketchyBox;
            foldButton.Image.sprite = sketchyBox;
            foldButtonIcon = foldButton.transform.Find("Icon");
            foldButtonIcon.GetComponent<Image>().sprite = minimizeButton;

            RebuildHeader();
            RebuildRows();
        }


        private void Update() {
            if (foldButtonWasClicked && !foldButton.IsClicked) {
                isExpanded = !isExpanded;
                AudioManager.me.PlaySound2D(AudioManager.me.Click, 1f, 0.1f);
            }

            foldButtonWasClicked = foldButton.IsClicked;

            if (isExpanded && Time.frameCount % 5 == 0) {
                RefreshRows();
            }

            if (isExpanded) {
                selfRect.anchoredPosition = new Vector2(selfRect.anchoredPosition.x, -selfRect.sizeDelta.y / 2f - 5f);
                foldButtonIcon.rotation = Quaternion.Euler(0f, 0f, 90f);
            } else {
                selfRect.anchoredPosition = new Vector2(selfRect.anchoredPosition.x, selfRect.sizeDelta.y / 2f - 12f);
                foldButtonIcon.rotation = Quaternion.Euler(0f, 0f, -90f);
            }

            if (scrollParent.gameObject.activeInHierarchy != isExpanded) {
                scrollParent.gameObject.SetActive(isExpanded);
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

            List<Villager> villagers = GetVillagers();

            foreach (Villager villager in villagers) {
                rows.Add(villager.UniqueId, SpawnRow(villager, scrollParent));
            }
        }

        private void RefreshRows() {
            List<Villager> villagers = GetVillagers();
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

            SortRows();
        }

        private VillagerRow SpawnRow(Villager villager, Transform parent) {
            RectTransform row = (RectTransform)Instantiate(Mod.RowPrefab, parent).transform;
            VillagerRow villagerRow = row.gameObject.AddComponent<VillagerRow>();
            villagerRow.SpawnColumns(villager, row);
            return villagerRow;
        }

        public void SetSortColumn(int columnIndex) {
            sortColumn = columnIndex;
        }

        private void SortRows() {
            const int ageColumn = 1;

            List<VillagerRow> sortedRows = rows.Values
                                               .OrderByDescending(i => i.Columns[sortColumn].GetValue())
                                               .ThenByDescending(i => i.Columns[ageColumn].GetValue())
                                               .ToList();

            for (int i = 0; i < sortedRows.Count; i++) {
                sortedRows[i].transform.SetSiblingIndex(i);
            }
        }

        private List<Villager> GetVillagers() {
            return WorldManager.instance.GetCards<Villager>().Where(i => i.Id != "trained_monkey").ToList();
        }
    }
}
