using UnityEngine;
using SurvivalGame.Core;

namespace SurvivalGame.World
{
    [CreateAssetMenu(menuName = "Survival Game/Location Definition")]
    public class LocationDefinition : ScriptableObject
    {
        public string id;
        public string displayName;
        public string sceneName;
        public LocationType type;
        public int dangerLevel = 1;
        public int energyCost = 1;
        public Sprite mapIcon;
    }
}
