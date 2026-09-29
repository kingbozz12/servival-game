using System;
using SurvivalGame.Core;

namespace SurvivalGame.Character
{
    [Serializable]
    public class CharacterProfile
    {
        public string nickname = "Survivor";
        public CharacterGender gender = CharacterGender.Male;
        public int skinTone = 0;      // 0..2
        public int hairstyle = 0;     // 0..4, 0 can be bald
        public int hairColor = 0;     // 0..4
        public bool hasBeard = false;

        public string headItem;
        public string torsoItem;
        public string jacketItem;
        public string legsItem;
        public string bootsItem;
        public string glovesItem;
        public string backpackItem;
        public string weaponItem;
    }
}
