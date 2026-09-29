using UnityEngine;

namespace SurvivalGame.CameraSystem
{
    [RequireComponent(typeof(Camera))]
    public class IsometricCameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;

        [Header("Top-down / isometric camera")]
        [SerializeField] private Vector3 offset = new Vector3(0f, 14f, -7.5f);
        [SerializeField, Range(55f, 80f)] private float pitch = 64f;
        [SerializeField] private float yaw = 0f;
        [SerializeField] private bool orthographic = true;
        [SerializeField] private float orthographicSize = 8.2f;

        [Header("Follow")]
        [SerializeField] private float smoothness = 12f;

        private Camera gameplayCamera;

        private void Awake()
        {
            gameplayCamera = GetComponent<Camera>();
            ApplyCameraMode();
        }

        public void Bind(Transform newTarget)
        {
            target = newTarget;
            ApplyCameraMode();
            Snap();
        }

        private void Start()
        {
            if (!target)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player) target = player.transform;
            }

            ApplyCameraMode();
            Snap();
        }

        private void LateUpdate()
        {
            if (!target) return;

            Vector3 desired = target.position + offset;
            transform.position = Vector3.Lerp(
                transform.position,
                desired,
                1f - Mathf.Exp(-smoothness * Time.deltaTime));

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        private void ApplyCameraMode()
        {
            if (!gameplayCamera)
                gameplayCamera = GetComponent<Camera>();

            gameplayCamera.orthographic = orthographic;

            if (orthographic)
                gameplayCamera.orthographicSize = orthographicSize;
        }

        private void Snap()
        {
            if (!target) return;

            transform.position = target.position + offset;
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }
}
