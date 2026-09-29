namespace SurvivalGame.Core
{
    public enum CharacterGender { Male, Female }
    public enum EquipmentSlot { Head, Torso, Jacket, Legs, Boots, Gloves, Backpack, Weapon }
    public enum LocationType { WorldMap, HomeBase, Resource, Event, Dungeon }

    public enum ItemUseMode
    {
        None,
        MeleeWeapon,
        GatheringTool,
        Consumable
    }

    public enum ToolType
    {
        None,
        Axe,
        Pickaxe
    }

    public enum ResourceNodeType
    {
        Tree,
        Stone
    }
}
