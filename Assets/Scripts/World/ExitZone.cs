using UnityEngine;

namespace SurvivalGame.World
{
    [RequireComponent(typeof(Collider))]
    public class ExitZone : MonoBehaviour
    {
        [SerializeField] private LocationLoader loader;
        [SerializeField] private string worldMapScene = "WorldMap";
        private bool triggered;

        private void Reset()
        {
            var c = GetComponent<Collider>();
            c.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (triggered || !other.CompareTag("Player")) return;
            triggered = true;

            if (loader)
                loader.LoadWorldMap(worldMapScene);
        }
    }
}
