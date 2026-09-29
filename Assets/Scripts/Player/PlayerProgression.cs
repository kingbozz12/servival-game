using System;
using UnityEngine;

namespace SurvivalGame.Player
{
    public class PlayerProgression : MonoBehaviour
    {
        [SerializeField, Min(1)] private int level = 1;
        [SerializeField, Min(0)] private int experience = 0;
        [SerializeField, Min(1)] private int experienceToNextLevel = 100;

        public event Action Changed;

        public int Level => level;
        public int Experience => experience;
        public int ExperienceToNextLevel => experienceToNextLevel;
        public float ExperienceNormalized => experienceToNextLevel <= 0
            ? 0f
            : Mathf.Clamp01(experience / (float)experienceToNextLevel);

        public void AddExperience(int amount)
        {
            if (amount <= 0) return;

            experience += amount;

            while (experience >= experienceToNextLevel)
            {
                experience -= experienceToNextLevel;
                level++;
                experienceToNextLevel = CalculateNextRequirement(level);
            }

            Changed?.Invoke();
        }

        private static int CalculateNextRequirement(int newLevel)
        {
            return Mathf.RoundToInt(100f * Mathf.Pow(newLevel, 1.35f));
        }
    }
}
