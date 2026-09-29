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
        [SerializeField] private InventoryContainer inventory;
        [SerializeField] private QuickBarSlot[] slots = new QuickBarSlot[4];

        public event Action Changed;
        public QuickBarSlot[] Slots => slots;

        public void Assign(int index, ItemDefinition item)
        {
            if (index < 0 || index >= slots.Length) return;
            slots[index].item = item;
            Changed?.Invoke();
        }

        public bool ConsumeOne(int index)
        {
            if (index < 0 || index >= slots.Length || !inventory) return false;
            var item = slots[index].item;
            if (!item || inventory.Count(item) <= 0) return false;

            bool removed = inventory.Remove(item, 1);
            if (removed && inventory.Count(item) == 0)
                slots[index].item = null;

            if (removed) Changed?.Invoke();
            return removed;
        }
    }
}
