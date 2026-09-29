using UnityEngine;

namespace SurvivalGame.Character
{
    public static class CharacterProfileStore
    {
        private const string Key = "character_profile_v1";

        public static void Save(CharacterProfile profile)
        {
            PlayerPrefs.SetString(Key, JsonUtility.ToJson(profile));
            PlayerPrefs.Save();
        }

        public static CharacterProfile Load()
        {
            if (!PlayerPrefs.HasKey(Key))
                return new CharacterProfile();

            var json = PlayerPrefs.GetString(Key);
            return JsonUtility.FromJson<CharacterProfile>(json) ?? new CharacterProfile();
        }

        public static void ResetProfile()
        {
            PlayerPrefs.DeleteKey(Key);
        }
    }
}
