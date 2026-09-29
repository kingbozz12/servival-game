using UnityEngine;

namespace SurvivalGame.UI
{
    [RequireComponent(typeof(Camera))]
    public class MinimapFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float height = 28f;
        [SerializeField] private bool rotateWithPlayer;

        public void Bind(Transform newTarget)
        {
            target = newTarget;
            Snap();
        }

        private void LateUpdate()
        {
            if (!target) return;

            transform.position = target.position + Vector3.up * height;
            transform.rotation = Quaternion.Euler(90f, rotateWithPlayer ? target.eulerAngles.y : 0f, 0f);
        }

        private void Snap()
        {
            if (!target) return;
            transform.position = target.position + Vector3.up * height;
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}
