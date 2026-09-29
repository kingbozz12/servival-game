using UnityEngine;

namespace SurvivalGame.CameraSystem
{
    public class IsometricCameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 11.5f, -9.5f);
        [SerializeField] private float smoothness = 10f;

        public void Bind(Transform newTarget)
        {
            target = newTarget;
            Snap();
        }

        private void Start()
        {
            if (!target)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player) target = player.transform;
            }

            Snap();
        }

        private void LateUpdate()
        {
            if (!target) return;

            Vector3 desired = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desired, 1f - Mathf.Exp(-smoothness * Time.deltaTime));
            transform.rotation = Quaternion.Euler(48f, 0f, 0f);
        }

        private void Snap()
        {
            if (!target) return;
            transform.position = target.position + offset;
            transform.rotation = Quaternion.Euler(48f, 0f, 0f);
        }
    }
}
