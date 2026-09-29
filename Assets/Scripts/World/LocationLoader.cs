using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SurvivalGame.World
{
    public class LocationLoader : MonoBehaviour
    {
        public void Load(LocationDefinition location, string spawnPointId = "default")
        {
            if (!location || string.IsNullOrWhiteSpace(location.sceneName)) return;
            TravelContext.SetTarget(location.id, spawnPointId);
            StartCoroutine(LoadScene(location.sceneName));
        }

        public void LoadWorldMap(string sceneName = "WorldMap")
        {
            TravelContext.SetTarget("world_map");
            StartCoroutine(LoadScene(sceneName));
        }

        private static IEnumerator LoadScene(string sceneName)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName);
            while (operation != null && !operation.isDone)
                yield return null;
        }
    }
}
