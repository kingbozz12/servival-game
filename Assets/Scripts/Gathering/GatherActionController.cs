using UnityEngine;
using SurvivalGame.Inventory;
using SurvivalGame.Core;

namespace SurvivalGame.Gathering
{
    public class GatherActionController : MonoBehaviour
    {
        [SerializeField] private EquipmentSystem equipment;
        [SerializeField] private InventoryContainer inventory;
        [SerializeField] private ResourceInteractionSensor sensor;

        public ResourceNode CurrentNode => sensor ? sensor.Closest : null;

        public ItemDefinition CurrentTool
        {
            get
            {
                var item = equipment ? equipment.GetEquipped(EquipmentSlot.Weapon) : null;
                return item && item.useMode == ItemUseMode.GatheringTool ? item : null;
            }
        }

        public bool ShouldShowGatherButton
        {
            get
            {
                var node = CurrentNode;
                var tool = CurrentTool;
                return node && tool && node.Accepts(tool);
            }
        }

        public Sprite CurrentToolIcon => ShouldShowGatherButton ? CurrentTool.icon : null;

        public bool UseTool()
        {
            if (!ShouldShowGatherButton || !inventory)
                return false;

            return CurrentNode.Gather(CurrentTool, inventory) > 0;
        }
    }
}
