using System;
using System.Collections.Generic;
using UnityEngine;
using SurvivalGame.Character;
using SurvivalGame.Core;

namespace SurvivalGame.Inventory
{
    [Serializable]
    public class EquippedItem
    {
        public EquipmentSlot slot;
        public ItemDefinition item;
    }

    public class EquipmentSystem : MonoBehaviour
    {
        [SerializeField] private InventoryContainer inventory;
        [SerializeField] private EquipmentController visualController;
        [SerializeField] private List<EquippedItem> equipped = new();

        public event Action Changed;
        public IReadOnlyList<EquippedItem> Equipped => equipped;

        public bool Equip(ItemDefinition item)
        {
            if (!item || !item.equippable || !inventory) return false;
            if (inventory.Count(item) < 1) return false;

            var current = equipped.Find(e => e.slot == item.equipmentSlot);
            if (current != null && current.item)
            {
                if (inventory.Add(current.item, 1) != 1)
                    return false;

                visualController?.Unequip(current.slot);
                equipped.Remove(current);
            }

            if (!inventory.Remove(item, 1))
                return false;

            equipped.Add(new EquippedItem { slot = item.equipmentSlot, item = item });
            visualController?.Equip(item.id, item.equipmentSlot);
            SaveProfileSlot(item.equipmentSlot, item.id);
            Changed?.Invoke();
            return true;
        }

        public bool Unequip(EquipmentSlot slot)
        {
            if (!inventory) return false;

            var current = equipped.Find(e => e.slot == slot);
            if (current == null || !current.item) return false;
            if (inventory.Add(current.item, 1) != 1) return false;

            equipped.Remove(current);
            visualController?.Unequip(slot);
            SaveProfileSlot(slot, null);
            Changed?.Invoke();
            return true;
        }

        public ItemDefinition GetEquipped(EquipmentSlot slot)
        {
            return equipped.Find(e => e.slot == slot)?.item;
        }

        private static void SaveProfileSlot(EquipmentSlot slot, string itemId)
        {
            var profile = CharacterProfileStore.Load();

            switch (slot)
            {
                case EquipmentSlot.Head: profile.headItem = itemId; break;
                case EquipmentSlot.Torso: profile.torsoItem = itemId; break;
                case EquipmentSlot.Jacket: profile.jacketItem = itemId; break;
                case EquipmentSlot.Legs: profile.legsItem = itemId; break;
                case EquipmentSlot.Boots: profile.bootsItem = itemId; break;
                case EquipmentSlot.Gloves: profile.glovesItem = itemId; break;
                case EquipmentSlot.Backpack: profile.backpackItem = itemId; break;
                case EquipmentSlot.Weapon: profile.weaponItem = itemId; break;
            }

            CharacterProfileStore.Save(profile);
        }
    }
}
