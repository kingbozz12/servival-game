using UnityEngine;
using UnityEngine.SceneManagement;

namespace SurvivalGame.UI
{
    public class SceneTravelButton : MonoBehaviour
    {
        [SerializeField] private string sceneName;

        public void SetScene(string value) => sceneName = value;

        public void Travel()
        {
            if (!string.IsNullOrWhiteSpace(sceneName))
                SceneManager.LoadScene(sceneName);
        }
    }
}
