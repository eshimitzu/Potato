using System;
using UnityEngine;
using Potato.Currencies;
using Potato.Interactions;

namespace Potato.Entities.Worm
{
    public class WormController : MonoBehaviour, IInteractable
    {
        private WormConfig _config;
        private Transform _potatoTarget;
        private int _hp;
        private bool _dead;

        public event Action<WormController> OnDied;

        public void Initialize(WormConfig config, Transform potatoTarget)
        {
            _config = config;
            _potatoTarget = potatoTarget;
            _hp = config.hp;
            _dead = false;
        }

        private void Update()
        {
            if (_dead || _potatoTarget == null) return;
            transform.position = Vector3.MoveTowards(
                transform.position,
                _potatoTarget.position,
                _config.moveSpeed * Time.deltaTime);
        }

        public void TakeDamage(int amount)
        {
            if (_dead) return;
            _hp -= amount;
            if (_hp <= 0) Die();
        }

        // IInteractable: tap worm to deal 1 damage
        public void Interact() => TakeDamage(1);

        private void Die()
        {
            _dead = true;
            CurrencySystem.Instance.Add(_config.meatCurrency, _config.meatDropAmount);
            OnDied?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
