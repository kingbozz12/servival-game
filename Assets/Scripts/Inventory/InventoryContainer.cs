using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SurvivalGame.Inventory
{
    public class InventoryContainer : MonoBehaviour
    {
        [Header("Capacity")]
        [SerializeField, Min(1)] private int slotCapacity = 20;
        [SerializeField, Min(0f)] private float maxWeight = 60f;

        [Header("Runtime")]
        [SerializeField] private List<ItemStack> stacks = new();

        public event Action Changed;

        public IReadOnlyList<ItemStack> Stacks => stacks;
        public int SlotCapacity => slotCapacity;
        public float MaxWeight => maxWeight;
        public float CurrentWeight => stacks.Sum(x => x?.TotalWeight ?? 0f);
        public int UsedSlots => stacks.Count(x => x != null && !x.IsEmpty);

        public bool CanAdd(ItemDefinition item, int amount)
        {
            if (!item || amount <= 0) return false;
            if (CurrentWeight + item.weight * amount > maxWeight) return false;

            int remaining = amount;

            if (item.maxStack > 1)
            {
                foreach (var stack in stacks)
                {
                    if (stack?.item != item) continue;
                    remaining -= Mathf.Max(0, item.maxStack - stack.amount);
                    if (remaining <= 0) return true;
                }
            }

            int freeSlots = slotCapacity - UsedSlots;
            int neededSlots = Mathf.CeilToInt(remaining / (float)Mathf.Max(1, item.maxStack));
            return neededSlots <= freeSlots;
        }

        public int Add(ItemDefinition item, int amount)
        {
            if (!item || amount <= 0) return 0;

            int added = 0;
            int remaining = amount;

            if (item.maxStack > 1)
            {
                foreach (var stack in stacks)
                {
                    if (remaining <= 0) break;
                    if (stack?.item != item || stack.amount >= item.maxStack) continue;

                    int byStack = item.maxStack - stack.amount;
                    int byWeight = MaxAmountByWeight(item);
                    int delta = Mathf.Min(remaining, byStack, byWeight);
                    if (delta <= 0) break;

                    stack.amount += delta;
                    remaining -= delta;
                    added += delta;
                }
            }

            while (remaining > 0 && UsedSlots < slotCapacity)
            {
                int byWeight = MaxAmountByWeight(item);
                if (byWeight <= 0) break;

                int delta = Mathf.Min(remaining, Mathf.Max(1, item.maxStack), byWeight);
                stacks.Add(new ItemStack(item, delta));
                remaining -= delta;
                added += delta;
            }

            Cleanup();
            if (added > 0) Changed?.Invoke();
            return added;
        }

        public bool Remove(ItemDefinition item, int amount)
        {
            if (!item || amount <= 0 || Count(item) < amount) return false;

            int remaining = amount;
            for (int i = stacks.Count - 1; i >= 0 && remaining > 0; i--)
            {
                var stack = stacks[i];
                if (stack?.item != item) continue;

                int delta = Mathf.Min(stack.amount, remaining);
                stack.amount -= delta;
                remaining -= delta;
            }

            Cleanup();
            Changed?.Invoke();
            return true;
        }

        public int Count(ItemDefinition item)
        {
            if (!item) return 0;
            return stacks.Where(s => s?.item == item).Sum(s => s.amount);
        }

        public void SortByCategoryThenName()
        {
            stacks = stacks
                .Where(s => s != null && !s.IsEmpty)
                .OrderBy(s => s.item.category)
                .ThenBy(s => s.item.displayName)
                .ToList();
            Changed?.Invoke();
        }

        private int MaxAmountByWeight(ItemDefinition item)
        {
            if (item.weight <= 0f) return int.MaxValue;
            float room = Mathf.Max(0f, maxWeight - CurrentWeight);
            return Mathf.FloorToInt(room / item.weight);
        }

        private void Cleanup()
        {
            stacks.RemoveAll(s => s == null || s.IsEmpty);
        }
    }
}
