using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using SurvivalGame.Inventory;

namespace SurvivalGame.UI
{
    public class InventoryScreenPresenter : MonoBehaviour
    {
        [SerializeField] private InventoryContainer inventory;
        [SerializeField] private Text weightText;
        [SerializeField] private Text slotsText;

        private InventorySlotView[] slotViews;

        public void Configure(InventoryContainer source, Text weight, Text slots)
        {
            inventory = source;
            weightText = weight;
            slotsText = slots;
            CacheSlots();
            Refresh();
        }

        private void OnEnable()
        {
            CacheSlots();
            if (inventory) inventory.Changed += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            if (inventory) inventory.Changed -= Refresh;
        }

        public void Refresh()
        {
            if (!inventory) return;
            CacheSlots();

            if (weightText)
                weightText.text = $"{inventory.CurrentWeight:0.0} / {inventory.MaxWeight:0.0} кг";

            if (slotsText)
                slotsText.text = $"{inventory.UsedSlots} / {inventory.SlotCapacity}";

            var stacks = inventory.Stacks
                .Where(s => s != null && !s.IsEmpty)
                .ToArray();

            for (int i = 0; i < slotViews.Length; i++)
                slotViews[i].Show(i < stacks.Length ? stacks[i] : null);
        }

        public void Sort()
        {
            if (inventory) inventory.SortByCategoryThenName();
        }

        private void CacheSlots()
        {
            if (slotViews == null || slotViews.Length == 0)
                slotViews = GetComponentsInChildren<InventorySlotView>(true);
        }
    }
}
