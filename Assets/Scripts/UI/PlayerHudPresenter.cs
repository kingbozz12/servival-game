using UnityEngine;
using UnityEngine.UI;
using SurvivalGame.Character;
using SurvivalGame.Player;

namespace SurvivalGame.UI
{
    public class PlayerHudPresenter : MonoBehaviour
    {
        [Header("Sources")]
        [SerializeField] private PlayerVitals vitals;
        [SerializeField] private PlayerProgression progression;

        [Header("Top left")]
        [SerializeField] private Text nicknameText;
        [SerializeField] private Text levelText;
        [SerializeField] private Slider healthBar;
        [SerializeField] private Text healthText;
        [SerializeField] private Slider armorBar;
        [SerializeField] private Text armorText;
        [SerializeField] private Text foodText;
        [SerializeField] private Text waterText;

        [Header("Bottom")]
        [SerializeField] private Slider expBar;
        [SerializeField] private Text bottomLevelText;
        [SerializeField] private Text expText;

        private void OnEnable()
        {
            if (vitals) vitals.Changed += Refresh;
            if (progression) progression.Changed += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            if (vitals) vitals.Changed -= Refresh;
            if (progression) progression.Changed -= Refresh;
        }

        public void Bind(PlayerVitals newVitals, PlayerProgression newProgression)
        {
            if (vitals) vitals.Changed -= Refresh;
            if (progression) progression.Changed -= Refresh;

            vitals = newVitals;
            progression = newProgression;

            if (vitals) vitals.Changed += Refresh;
            if (progression) progression.Changed += Refresh;
            Refresh();
        }

        public void Refresh()
        {
            var profile = CharacterProfileStore.Load();

            if (nicknameText) nicknameText.text = profile.nickname;
            if (levelText) levelText.text = progression ? progression.Level.ToString() : "1";

            if (vitals)
            {
                if (healthBar)
                {
                    healthBar.minValue = 0;
                    healthBar.maxValue = vitals.MaxHealth;
                    healthBar.value = vitals.Health;
                }
                if (healthText) healthText.text = $"{vitals.Health} / {vitals.MaxHealth}";

                if (armorBar)
                {
                    armorBar.minValue = 0;
                    armorBar.maxValue = Mathf.Max(1, vitals.MaxArmor);
                    armorBar.value = vitals.Armor;
                }
                if (armorText) armorText.text = $"{vitals.Armor} / {vitals.MaxArmor}";
                if (foodText) foodText.text = vitals.Food.ToString();
                if (waterText) waterText.text = vitals.Water.ToString();
            }

            if (progression)
            {
                if (expBar)
                {
                    expBar.minValue = 0;
                    expBar.maxValue = progression.ExperienceToNextLevel;
                    expBar.value = progression.Experience;
                }
                if (bottomLevelText) bottomLevelText.text = $"Ур. {progression.Level}";
                if (expText) expText.text = $"{progression.Experience} / {progression.ExperienceToNextLevel}";
            }
        }
    }
}
