using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalGame.Inventory
{
    [Serializable]
    public class StarterItem
    {
        public ItemDefinition item;
        [Min(1)] public int amount = 1;
        public bool equipOnStart;
    }

    public class StarterLoadout : MonoBehaviour
    {
        private const string GrantedKey = "starter_loadout_granted_v1";

        [SerializeField] private InventoryContainer inventory;
        [SerializeField] private EquipmentSystem equipment;
        [SerializeField] private List<StarterItem> items = new();

        private void Start()
        {
            if (PlayerPrefs.GetInt(GrantedKey, 0) == 1) return;
            Grant();
        }

        public void Grant()
        {
            if (!inventory) return;

            foreach (var entry in items)
            {
                if (!entry.item) continue;

                int added = inventory.Add(entry.item, Mathf.Max(1, entry.amount));
                if (added > 0 && entry.equipOnStart && entry.item.equippable)
                    equipment?.Equip(entry.item);
            }

            PlayerPrefs.SetInt(GrantedKey, 1);
            PlayerPrefs.Save();
        }
    }
}
