using System;
using DG.Tweening;
using UnityEngine;
using Potato.Currencies;
using Potato.Interactions;

namespace Potato.Entities.Worm
{
    public class WormController : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject _calmView;
        [SerializeField] private GameObject _riledView;
        [SerializeField] private Transform _leftEye;
        [SerializeField] private Transform _rightEye;
        [SerializeField] private float _punchScale = 0.5f;
        [SerializeField] private float _punchDuration = 0.5f;
        [SerializeField] private float _hitShakeStrength = 0.15f;
        [SerializeField] private float _hitShakeDuration = 0.25f;

        private WormConfig _config;
        private int _hp;

        public event Action<WormController> OnDied;

        public void Initialize(WormConfig config)
        {
            _config = config;
            _hp = config.hp;
            _calmView.SetActive(true);
            _riledView.SetActive(false);
            Appear();
        }

        public void Interact()
        {
            if (_hp <= 0) return;
            _hp--;
            BulgeEyes();
            transform.DOKill();
            transform.DOShakePosition(_hitShakeDuration, _hitShakeStrength, 20, 90, false, false)
                .SetLink(gameObject);
            if (_hp <= 0) Die();
        }

        private void BulgeEyes()
        {
            BulgeEye(_leftEye);
            BulgeEye(_rightEye);
        }

        private void BulgeEye(Transform eye)
        {
            if (eye == null) return;
            eye.DOKill();
            eye.localScale = Vector3.one;
            eye.DOPunchScale(Vector3.one * _punchScale, _punchDuration, 1, 0.1f).SetLink(gameObject);
        }

        private void Appear()
        {
            transform.localScale = Vector3.one;
            float targetY = transform.position.y;
            transform.position -= Vector3.up * _config.burrowDepth;
            transform.DOMoveY(targetY, _config.appearDuration).SetEase(Ease.OutBounce).SetLink(gameObject);
        }

        private void Die()
        {
            _hp = 0;
            _calmView.SetActive(false);
            _riledView.SetActive(true);
            CurrencySystem.Instance.Add(_config.meatCurrency, _config.meatDropAmount, transform.position);
            OnDied?.Invoke(this);
            transform.DOScale(Vector3.zero, _config.deathDuration)
                .SetEase(Ease.InBack)
                .SetLink(gameObject)
                .OnComplete(() => Destroy(gameObject));
        }
    }
}
