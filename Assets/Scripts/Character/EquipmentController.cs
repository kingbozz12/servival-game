using System;
using System.Collections.Generic;
using UnityEngine;
using SurvivalGame.Core;

namespace SurvivalGame.Character
{
    [Serializable]
    public class EquipmentVisual
    {
        public string itemId;
        public EquipmentSlot slot;
        public GameObject visual;
    }

    public class EquipmentController : MonoBehaviour
    {
        [SerializeField] private List<EquipmentVisual> visuals = new();
        private readonly Dictionary<EquipmentSlot, EquipmentVisual> equipped = new();

        public void Equip(string itemId, EquipmentSlot slot)
        {
            Unequip(slot);

            var entry = visuals.Find(v => v.slot == slot && v.itemId == itemId);
            if (entry == null || !entry.visual) return;

            entry.visual.SetActive(true);
            equipped[slot] = entry;
        }

        public void Unequip(EquipmentSlot slot)
        {
            if (!equipped.TryGetValue(slot, out var current)) return;
            if (current.visual) current.visual.SetActive(false);
            equipped.Remove(slot);
        }

        public void HideAll()
        {
            foreach (var v in visuals)
                if (v.visual) v.visual.SetActive(false);
            equipped.Clear();
        }
    }
}
