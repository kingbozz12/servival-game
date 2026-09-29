using SurvivalGame.Character;
using SurvivalGame.Player;

namespace SurvivalGame.UI
{
    public readonly struct PlayerHudData
    {
        public readonly string Nickname;
        public readonly int Level;
        public readonly int Health;
        public readonly int MaxHealth;
        public readonly int Armor;
        public readonly int MaxArmor;
        public readonly int Food;
        public readonly int Water;
        public readonly int Experience;
        public readonly int ExperienceToNextLevel;

        public PlayerHudData(CharacterProfile profile, PlayerVitals vitals, PlayerProgression progression)
        {
            Nickname = profile?.nickname ?? "Survivor";
            Level = progression ? progression.Level : 1;
            Health = vitals ? vitals.Health : 100;
            MaxHealth = vitals ? vitals.MaxHealth : 100;
            Armor = vitals ? vitals.Armor : 0;
            MaxArmor = vitals ? vitals.MaxArmor : 100;
            Food = vitals ? vitals.Food : 100;
            Water = vitals ? vitals.Water : 100;
            Experience = progression ? progression.Experience : 0;
            ExperienceToNextLevel = progression ? progression.ExperienceToNextLevel : 100;
        }
    }
}
