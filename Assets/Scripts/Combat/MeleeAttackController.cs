using UnityEngine;
using SurvivalGame.Inventory;
using SurvivalGame.Core;

namespace SurvivalGame.Combat
{
    public class MeleeAttackController : MonoBehaviour
    {
        [SerializeField] private EquipmentSystem equipment;
        [SerializeField] private Transform attackOrigin;
        [SerializeField] private LayerMask damageLayers = ~0;
        [SerializeField, Min(0.05f)] private float defaultRadius = 1.4f;

        private float nextAttackTime;

        public ItemDefinition CurrentWeapon => equipment ? equipment.GetEquipped(EquipmentSlot.Weapon) : null;

        public bool CanAttack
        {
            get
            {
                var weapon = CurrentWeapon;
                return weapon && weapon.useMode == ItemUseMode.MeleeWeapon && Time.time >= nextAttackTime;
            }
        }

        public Sprite CurrentAttackIcon => CurrentWeapon ? CurrentWeapon.icon : null;

        public bool Attack()
        {
            var weapon = CurrentWeapon;
            if (!weapon || weapon.useMode != ItemUseMode.MeleeWeapon || Time.time < nextAttackTime)
                return false;

            float attacksPerSecond = Mathf.Max(0.1f, weapon.attackSpeed);
            nextAttackTime = Time.time + 1f / attacksPerSecond;

            Vector3 center = attackOrigin ? attackOrigin.position : transform.position;
            float radius = weapon.range > 0f ? weapon.range : defaultRadius;

            var hits = Physics.OverlapSphere(center, radius, damageLayers, QueryTriggerInteraction.Ignore);
            foreach (var hit in hits)
            {
                if (!hit || hit.transform.root == transform.root) continue;

                var damageable = hit.GetComponentInParent<IDamageable>();
                if (damageable == null) continue;

                damageable.TakeDamage(Mathf.Max(1, weapon.damage));
                return true;
            }

            return true;
        }
    }
}
