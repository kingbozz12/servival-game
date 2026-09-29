using System;
using System.Collections.Generic;
using UnityEngine;
using SurvivalGame.Inventory;

namespace SurvivalGame.Loot
{
    public class LootContainer : MonoBehaviour
    {
        [SerializeField] private LootTable lootTable;
        [SerializeField] private List<ItemStack> contents = new();
        [SerializeField] private bool rollOnStart = true;

        public event Action Changed;
        public IReadOnlyList<ItemStack> Contents => contents;

        private void Start()
        {
            if (rollOnStart && contents.Count == 0 && lootTable)
            {
                contents = lootTable.Roll();
                Changed?.Invoke();
            }
        }

        public int Take(ItemDefinition item, int amount, InventoryContainer target)
        {
            if (!item || amount <= 0 || !target) return 0;

            var stack = contents.Find(s => s?.item == item);
            if (stack == null) return 0;

            int requested = Mathf.Min(amount, stack.amount);
            int added = target.Add(item, requested);
            stack.amount -= added;

            contents.RemoveAll(s => s == null || s.IsEmpty);
            if (added > 0) Changed?.Invoke();
            return added;
        }

        public void TakeAll(InventoryContainer target)
        {
            if (!target) return;

            for (int i = contents.Count - 1; i >= 0; i--)
            {
                var stack = contents[i];
                if (stack == null || stack.IsEmpty) continue;

                int added = target.Add(stack.item, stack.amount);
                stack.amount -= added;
            }

            contents.RemoveAll(s => s == null || s.IsEmpty);
            Changed?.Invoke();
        }
    }
}
