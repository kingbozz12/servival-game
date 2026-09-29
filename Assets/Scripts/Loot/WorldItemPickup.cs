using UnityEngine;
using SurvivalGame.Inventory;

namespace SurvivalGame.Loot
{
    public class WorldItemPickup : MonoBehaviour
    {
        [SerializeField] private ItemDefinition item;
        [SerializeField, Min(1)] private int amount = 1;

        public bool TryPickup(InventoryContainer inventory)
        {
            if (!inventory || !item) return false;

            int added = inventory.Add(item, amount);
            amount -= added;

            if (amount <= 0)
                Destroy(gameObject);

            return added > 0;
        }
    }
}
