using UnityEngine;

namespace SurvivalGame.World
{
    public class SpawnPoint : MonoBehaviour
    {
        public string id = "default";

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (!player || TravelContext.SpawnPointId != id) return;

            player.transform.SetPositionAndRotation(transform.position, transform.rotation);
        }
    }
}
