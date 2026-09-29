using System;
using System.Collections.Generic;
using UnityEngine;
using SurvivalGame.Inventory;

namespace SurvivalGame.Loot
{
    [Serializable]
    public class LootEntry
    {
        public ItemDefinition item;
        [Range(0f, 1f)] public float chance = 1f;
        [Min(1)] public int minAmount = 1;
        [Min(1)] public int maxAmount = 1;
    }

    [CreateAssetMenu(menuName = "Survival Game/Loot/Loot Table")]
    public class LootTable : ScriptableObject
    {
        public List<LootEntry> entries = new();

        public List<ItemStack> Roll()
        {
            var result = new List<ItemStack>();

            foreach (var entry in entries)
            {
                if (!entry.item || UnityEngine.Random.value > entry.chance) continue;
                int max = Mathf.Max(entry.minAmount, entry.maxAmount);
                int amount = UnityEngine.Random.Range(entry.minAmount, max + 1);
                result.Add(new ItemStack(entry.item, amount));
            }

            return result;
        }
    }
}
