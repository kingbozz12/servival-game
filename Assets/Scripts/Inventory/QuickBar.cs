using System;
using UnityEngine;

namespace SurvivalGame.Inventory
{
    [Serializable]
    public class QuickBarSlot
    {
        public ItemDefinition item;
    }

    public class QuickBar : MonoBehaviour
    {
        private const int DefaultSlotCount = 4;

        [SerializeField] private InventoryContainer inventory;
        [SerializeField] private QuickBarSlot[] slots = new QuickBarSlot[DefaultSlotCount];

        public event Action Changed;
        public QuickBarSlot[] Slots => slots;

        private void Awake()
        {
            EnsureSlots();
        }

        private void OnValidate()
        {
            EnsureSlots();
        }

        public void Assign(int index, ItemDefinition item)
        {
            EnsureSlots();
            if (index < 0 || index >= slots.Length) return;

            slots[index].item = item;
            Changed?.Invoke();
        }

        public bool ConsumeOne(int index)
        {
            EnsureSlots();
            if (index < 0 || index >= slots.Length || !inventory) return false;

            var item = slots[index].item;
            if (!item || inventory.Count(item) <= 0) return false;

            bool removed = inventory.Remove(item, 1);
            if (removed && inventory.Count(item) == 0)
                slots[index].item = null;

            if (removed) Changed?.Invoke();
            return removed;
        }

        private void EnsureSlots()
        {
            if (slots == null || slots.Length != DefaultSlotCount)
                Array.Resize(ref slots, DefaultSlotCount);

            for (int i = 0; i < slots.Length; i++)
                slots[i] ??= new QuickBarSlot();
        }
    }
}
