using NaughtyAttributes;
using UnityEngine;

namespace Potato.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Character : MonoBehaviour
    {
        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int AttackHash = Animator.StringToHash("Attack");

        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerInteractor _interactor;
        [SerializeField] private float _animDampTime = 0.1f;

        private PlayerMovement _movement;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            _animator.SetFloat(SpeedHash, _movement.CurrentSpeed, _animDampTime, Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.Space))
                Attack();
        }

        [Button]
        public void Attack()
        {
            _animator.SetTrigger(AttackHash);
            _interactor.Nearest?.Interact();
        }
    }
}
