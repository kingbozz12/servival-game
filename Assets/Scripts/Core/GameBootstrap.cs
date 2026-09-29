using UnityEngine;
using SurvivalGame.Character;

namespace SurvivalGame.Core
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private ModularCharacterVisual characterVisual;
        [SerializeField] private EquipmentController equipment;

        private void Start()
        {
            var profile = CharacterProfileStore.Load();
            if (characterVisual) characterVisual.Apply(profile);

            if (!equipment) return;
            equipment.HideAll();

            EquipIfSet(profile.headItem, EquipmentSlot.Head);
            EquipIfSet(profile.torsoItem, EquipmentSlot.Torso);
            EquipIfSet(profile.jacketItem, EquipmentSlot.Jacket);
            EquipIfSet(profile.legsItem, EquipmentSlot.Legs);
            EquipIfSet(profile.bootsItem, EquipmentSlot.Boots);
            EquipIfSet(profile.glovesItem, EquipmentSlot.Gloves);
            EquipIfSet(profile.backpackItem, EquipmentSlot.Backpack);
            EquipIfSet(profile.weaponItem, EquipmentSlot.Weapon);
        }

        private void EquipIfSet(string id, EquipmentSlot slot)
        {
            if (!string.IsNullOrWhiteSpace(id))
                equipment.Equip(id, slot);
        }
    }
}
