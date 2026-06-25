using NaughtyAttributes;
using UnityEngine;
using Potato.Entities.Worm;

namespace Potato.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Character : MonoBehaviour
    {
        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int AttackHash = Animator.StringToHash("Attack");

        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerInteractor _interactor;
        [SerializeField] private PlayerConfig _config;
        [SerializeField] private float _animDampTime = 0.1f;

        private PlayerMovement _movement;
        private float _attackTimer;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _movement.Initialize(_config);
            _interactor.Initialize(_config);
        }

        private void Update()
        {
            _animator.SetFloat(SpeedHash, _movement.CurrentSpeed, _animDampTime, Time.deltaTime);

            _attackTimer -= Time.deltaTime;

            if (_interactor.Nearest is WormController worm)
            {
                RotateTowards(worm.transform.position);

                if (_attackTimer <= 0f)
                    Attack();
            }

            if (Input.GetKeyDown(KeyCode.Space))
                Attack();
        }

        private void RotateTowards(Vector3 targetPosition)
        {
            Vector3 dir = targetPosition - transform.position;
            dir.y = 0f;
            if (dir == Vector3.zero) return;
            Quaternion target = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, target, _config.rotationSpeed * Time.deltaTime);
        }

        [Button]
        public void Attack()
        {
            _attackTimer = _config.attackCooldown;
            _animator.ResetTrigger(AttackHash);
            _animator.SetTrigger(AttackHash);
            _interactor.Nearest?.Interact();
        }
    }
}
