using UnityEngine;

namespace Potato.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        private PlayerConfig _config;

        private CharacterController _cc;
        private Vector3 _velocity;

        public bool IsMoving { get; private set; }
        public float CurrentSpeed { get; private set; }

        public void Initialize(PlayerConfig config) => _config = config;

        private void Awake() => _cc = GetComponent<CharacterController>();

        private void Update()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Move(new Vector2(h, v));
        }

        // Separate method so it can be driven by a virtual joystick too
        public void Move(Vector2 input)
        {
            Vector3 dir = new Vector3(input.x, 0f, input.y).normalized;
            IsMoving = dir != Vector3.zero;

            if (IsMoving)
            {
                Quaternion target = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation, target, _config.rotationSpeed * Time.deltaTime);
            }

            _velocity.x = dir.x * _config.moveSpeed;
            _velocity.z = dir.z * _config.moveSpeed;
            CurrentSpeed = new Vector3(_velocity.x, 0f, _velocity.z).magnitude / _config.moveSpeed;
            _velocity.y = _cc.isGrounded ? -0.5f : _velocity.y + Physics.gravity.y * Time.deltaTime;

            _cc.Move(_velocity * Time.deltaTime);
        }
    }
}
