using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SurvivalGame.Player;

namespace SurvivalGame.UI
{
    public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform handle;
        [SerializeField] private MobileRunController target;
        [SerializeField, Range(0.3f, 1f)] private float handleLimit = 0.65f;

        private Camera uiCamera;

        private void Awake()
        {
            if (!background) background = transform as RectTransform;
            if (!handle && transform.childCount > 0)
                handle = transform.GetChild(0) as RectTransform;
        }

        public void Bind(MobileRunController controller)
        {
            target = controller;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!background || !handle) return;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    background, eventData.position, uiCamera, out var local))
                return;

            Vector2 radius = background.rect.size * 0.5f;
            Vector2 normalized = new Vector2(
                radius.x > 0f ? local.x / radius.x : 0f,
                radius.y > 0f ? local.y / radius.y : 0f);

            normalized = Vector2.ClampMagnitude(normalized, 1f);
            handle.anchoredPosition = new Vector2(
                normalized.x * radius.x * handleLimit,
                normalized.y * radius.y * handleLimit);

            target?.SetMoveInput(normalized);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (handle) handle.anchoredPosition = Vector2.zero;
            target?.SetMoveInput(Vector2.zero);
        }
    }
}
