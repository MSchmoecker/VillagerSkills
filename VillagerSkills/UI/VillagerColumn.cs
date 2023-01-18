using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VillagerSkills.UI {
    public class VillagerColumn : MonoBehaviour, IPointerClickHandler {
        public TMP_Text text;
        private VillagerRow row;
        private object cachedValue;

        public void Init(TMP_Text textField, VillagerRow parentRow) {
            text = textField;
            row = parentRow;
        }

        public void UpdateValue(object newValue) {
            if (cachedValue != null && cachedValue.Equals(newValue)) {
                return;
            }

            text.text = newValue.ToString();
            cachedValue = newValue;
        }

        public void OnPointerClick(PointerEventData eventData) {
            row.OnClick(this);
        }
    }
}
