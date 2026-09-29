using System;

namespace SurvivalGame.Inventory
{
    [Serializable]
    public class ItemStack
    {
        public ItemDefinition item;
        public int amount;

        public ItemStack(ItemDefinition item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }

        public bool IsEmpty => item == null || amount <= 0;
        public float TotalWeight => item == null ? 0f : item.weight * amount;
    }
}
