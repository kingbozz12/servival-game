using System;
using UnityEngine;

namespace SurvivalGame.Player
{
    public class PlayerVitals : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField, Min(1)] private int maxHealth = 100;
        [SerializeField] private int health = 100;

        [Header("Armor")]
        [SerializeField, Min(0)] private int maxArmor = 100;
        [SerializeField] private int armor = 0;

        [Header("Survival")]
        [SerializeField, Range(0, 100)] private int food = 100;
        [SerializeField, Range(0, 100)] private int water = 100;

        public event Action Changed;

        public int MaxHealth => maxHealth;
        public int Health => health;
        public int MaxArmor => maxArmor;
        public int Armor => armor;
        public int Food => food;
        public int Water => water;

        private void OnValidate()
        {
            health = Mathf.Clamp(health, 0, maxHealth);
            armor = Mathf.Clamp(armor, 0, maxArmor);
            food = Mathf.Clamp(food, 0, 100);
            water = Mathf.Clamp(water, 0, 100);
        }

        public void SetHealth(int value)
        {
            int next = Mathf.Clamp(value, 0, maxHealth);
            if (next == health) return;
            health = next;
            Changed?.Invoke();
        }

        public void SetArmor(int value)
        {
            int next = Mathf.Clamp(value, 0, maxArmor);
            if (next == armor) return;
            armor = next;
            Changed?.Invoke();
        }

        public void SetFood(int value)
        {
            int next = Mathf.Clamp(value, 0, 100);
            if (next == food) return;
            food = next;
            Changed?.Invoke();
        }

        public void SetWater(int value)
        {
            int next = Mathf.Clamp(value, 0, 100);
            if (next == water) return;
            water = next;
            Changed?.Invoke();
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0) return;

            int absorbed = Mathf.Min(armor, amount);
            armor -= absorbed;
            amount -= absorbed;

            if (amount > 0)
                health = Mathf.Max(0, health - amount);

            Changed?.Invoke();
        }

        public void RestoreHealth(int amount)
        {
            if (amount <= 0) return;
            SetHealth(health + amount);
        }
    }
}
