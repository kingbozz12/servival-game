using System;
using UnityEngine;
using SurvivalGame.Core;
using SurvivalGame.Inventory;

namespace SurvivalGame.Gathering
{
    public class ResourceNode : MonoBehaviour
    {
        [Header("Type")]
        [SerializeField] private ResourceNodeType nodeType;

        [Header("Resource")]
        [SerializeField] private ItemDefinition droppedItem;
        [SerializeField, Min(1)] private int totalYield = 5;
        [SerializeField, Min(1)] private int durability = 5;

        public event Action<ResourceNode> Depleted;
        public event Action<ResourceNode> Hit;

        public ResourceNodeType NodeType => nodeType;
        public bool IsDepleted => durability <= 0 || totalYield <= 0;

        public bool Accepts(ItemDefinition tool)
        {
            if (!tool || tool.useMode != ItemUseMode.GatheringTool) return false;

            return nodeType switch
            {
                ResourceNodeType.Tree => tool.toolType == ToolType.Axe,
                ResourceNodeType.Stone => tool.toolType == ToolType.Pickaxe,
                _ => false
            };
        }

        public int Gather(ItemDefinition tool, InventoryContainer inventory)
        {
            if (IsDepleted || !inventory || !Accepts(tool)) return 0;

            int power = Mathf.Max(1, tool.gatherPower);
            durability -= power;

            int wanted = Mathf.Min(power, totalYield);
            int added = droppedItem ? inventory.Add(droppedItem, wanted) : 0;
            totalYield -= added;

            Hit?.Invoke(this);

            if (IsDepleted)
                Depleted?.Invoke(this);

            return added;
        }
    }
}
