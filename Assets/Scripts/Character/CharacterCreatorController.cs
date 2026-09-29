using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SurvivalGame.Core;

namespace SurvivalGame.Character
{
    public class CharacterCreatorController : MonoBehaviour
    {
        [SerializeField] private InputField nicknameInput;
        [SerializeField] private Text summaryText;

        private CharacterProfile profile = new CharacterProfile();

        private void Start()
        {
            if (nicknameInput)
                nicknameInput.text = profile.nickname;
            RefreshSummary();
        }

        public void SetMale()
        {
            profile.gender = CharacterGender.Male;
            RefreshSummary();
        }

        public void SetFemale()
        {
            profile.gender = CharacterGender.Female;
            profile.hasBeard = false;
            RefreshSummary();
        }

        public void NextSkin()
        {
            profile.skinTone = (profile.skinTone + 1) % 3;
            RefreshSummary();
        }

        public void NextHair()
        {
            profile.hairstyle = (profile.hairstyle + 1) % 5;
            RefreshSummary();
        }

        public void NextHairColor()
        {
            profile.hairColor = (profile.hairColor + 1) % 5;
            RefreshSummary();
        }

        public void ToggleBeard()
        {
            if (profile.gender == CharacterGender.Male)
                profile.hasBeard = !profile.hasBeard;
            else
                profile.hasBeard = false;

            RefreshSummary();
        }

        public void CreateCharacter()
        {
            profile.nickname = nicknameInput && !string.IsNullOrWhiteSpace(nicknameInput.text)
                ? nicknameInput.text.Trim()
                : "Survivor";

            CharacterProfileStore.Save(profile);
            PlayerPrefs.DeleteKey("starter_loadout_granted_v1");
            SceneManager.LoadScene("WorldMap");
        }

        private void RefreshSummary()
        {
            if (!summaryText) return;

            string beard = profile.gender == CharacterGender.Male
                ? (profile.hasBeard ? "Да" : "Нет")
                : "—";

            summaryText.text =
                $"Пол: {(profile.gender == CharacterGender.Male ? "Мужчина" : "Женщина")}\n" +
                $"Кожа: {profile.skinTone + 1}/3\n" +
                $"Причёска: {profile.hairstyle + 1}/5\n" +
                $"Цвет волос: {profile.hairColor + 1}/5\n" +
                $"Борода: {beard}";
        }
    }
}
