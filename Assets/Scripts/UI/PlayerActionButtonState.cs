using UnityEngine;
using SurvivalGame.Combat;
using SurvivalGame.Gathering;

namespace SurvivalGame.UI
{
    public enum PlayerActionButtonMode
    {
        Hidden,
        MeleeAttack,
        Gather
    }

    public class PlayerActionButtonState : MonoBehaviour
    {
        [SerializeField] private MeleeAttackController melee;
        [SerializeField] private GatherActionController gathering;

        public PlayerActionButtonMode Mode
        {
            get
            {
                if (gathering && gathering.ShouldShowGatherButton)
                    return PlayerActionButtonMode.Gather;

                if (melee && melee.CurrentWeapon && melee.CurrentWeapon.useMode == Core.ItemUseMode.MeleeWeapon)
                    return PlayerActionButtonMode.MeleeAttack;

                return PlayerActionButtonMode.Hidden;
            }
        }

        public Sprite Icon
        {
            get
            {
                return Mode switch
                {
                    PlayerActionButtonMode.Gather => gathering.CurrentToolIcon,
                    PlayerActionButtonMode.MeleeAttack => melee.CurrentAttackIcon,
                    _ => null
                };
            }
        }

        public bool Press()
        {
            return Mode switch
            {
                PlayerActionButtonMode.Gather => gathering.UseTool(),
                PlayerActionButtonMode.MeleeAttack => melee.Attack(),
                _ => false
            };
        }
    }
}
