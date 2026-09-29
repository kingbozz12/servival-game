using UnityEngine;
using SurvivalGame.Core;

namespace SurvivalGame.Inventory
{
    [CreateAssetMenu(menuName = "Survival Game/Items/Item Definition")]
    public class ItemDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string id;
        public string displayName;
        [TextArea] public string description;
        public ItemCategory category = ItemCategory.Misc;
        public Sprite icon;

        [Header("Inventory")]
        [Min(1)] public int maxStack = 1;
        [Min(0f)] public float weight = 0.1f;

        [Header("Equipment")]
        public bool equippable;
        public EquipmentSlot equipmentSlot;
        public ItemUseMode useMode;
        public ToolType toolType;

        [Header("Combat / protection")]
        [Min(0)] public int damage;
        [Min(0)] public int defense;
        [Min(0)] public int durabilityMax;
        [Min(0f)] public float attackSpeed = 1f;
        [Min(0f)] public float range = 1f;

        [Header("Gathering")]
        [Min(0)] public int gatherPower = 1;

        [Header("Survival")]
        public int healthRestore;
        public int hungerRestore;
        public int thirstRestore;
    }
}
