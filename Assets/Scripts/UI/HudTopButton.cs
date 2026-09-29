using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SurvivalGame.UI
{
    [RequireComponent(typeof(Button))]
    public class HudTopButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform visualRoot;
        [SerializeField] private float pressedScale = 0.95f;

        private Vector3 normalScale = Vector3.one;

        private void Awake()
        {
            if (!visualRoot) visualRoot = transform as RectTransform;
            if (visualRoot) normalScale = visualRoot.localScale;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (visualRoot) visualRoot.localScale = normalScale * pressedScale;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (visualRoot) visualRoot.localScale = normalScale;
        }

        private void OnDisable()
        {
            if (visualRoot) visualRoot.localScale = normalScale;
        }
    }
}
