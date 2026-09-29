using UnityEngine;
using UnityEngine.UI;

namespace SurvivalGame.UI
{
    public class HudClock : MonoBehaviour
    {
        [SerializeField] private Text timeText;
        [SerializeField] private float gameMinutesPerRealSecond = 0.15f;
        [SerializeField] private float startHour = 18.4f;

        private float gameMinutes;

        private void Start()
        {
            gameMinutes = startHour * 60f;
            Refresh();
        }

        private void Update()
        {
            gameMinutes = (gameMinutes + gameMinutesPerRealSecond * Time.deltaTime) % 1440f;
            Refresh();
        }

        private void Refresh()
        {
            if (!timeText) return;

            int total = Mathf.FloorToInt(gameMinutes);
            int hour = total / 60;
            int minute = total % 60;
            timeText.text = $"{hour:00}:{minute:00}";
        }
    }
}
