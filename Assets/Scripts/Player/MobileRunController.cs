using UnityEngine;
using UnityEngine.InputSystem;

namespace SurvivalGame.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class MobileRunController : MonoBehaviour
    {
        [SerializeField] private float runSpeed = 4.8f;
        [SerializeField] private float rotationSpeed = 14f;
        [SerializeField] private float gravity = -25f;

        private CharacterController controller;
        private Vector2 moveInput;
        private float verticalVelocity;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        public void SetMoveInput(Vector2 input)
        {
            moveInput = Vector2.ClampMagnitude(input, 1f);
        }

        private void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if (moveInput.sqrMagnitude < 0.001f && Keyboard.current != null)
            {
                Vector2 keyboard = Vector2.zero;
                if (Keyboard.current.aKey.isPressed) keyboard.x -= 1f;
                if (Keyboard.current.dKey.isPressed) keyboard.x += 1f;
                if (Keyboard.current.sKey.isPressed) keyboard.y -= 1f;
                if (Keyboard.current.wKey.isPressed) keyboard.y += 1f;
                moveInput = Vector2.ClampMagnitude(keyboard, 1f);
            }
#endif
            Vector3 cameraForward = Camera.main
                ? Vector3.Scale(Camera.main.transform.forward, new Vector3(1f, 0f, 1f)).normalized
                : Vector3.forward;
            Vector3 cameraRight = Camera.main
                ? Vector3.Scale(Camera.main.transform.right, new Vector3(1f, 0f, 1f)).normalized
                : Vector3.right;

            Vector3 move = cameraForward * moveInput.y + cameraRight * moveInput.x;

            if (move.sqrMagnitude > 0.001f)
            {
                move.Normalize();
                var targetRotation = Quaternion.LookRotation(move, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            if (controller.isGrounded && verticalVelocity < 0f)
                verticalVelocity = -2f;
            verticalVelocity += gravity * Time.deltaTime;

            Vector3 velocity = move * runSpeed;
            velocity.y = verticalVelocity;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}
