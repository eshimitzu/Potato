using System;
using DG.Tweening;
using UnityEngine;
using Potato.Currencies;
using Potato.Interactions;

namespace Potato.Entities.Worm
{
    public class WormController : MonoBehaviour, IInteractable
    {
        public enum State { Calm, Riled }

        [SerializeField] private GameObject _calmView;
        [SerializeField] private GameObject _riledView;

        private WormConfig _config;

        public State CurrentState { get; private set; }

        public event Action<WormController> OnDied;
        public event Action<State> OnStateChanged;

        public void Initialize(WormConfig config)
        {
            _config = config;
            SetState(State.Calm);
            Appear();
        }

        private void Appear()
        {
            transform.localScale = Vector3.one;
            float targetY = transform.position.y;
            transform.position -= Vector3.up * _config.burrowDepth;
            transform.DOMoveY(targetY, _config.appearDuration).SetEase(Ease.OutBounce);
        }

        public void Interact()
        {
            if (CurrentState == State.Calm)
                SetState(State.Riled);
            else
                Die();
        }

        private void SetState(State state)
        {
            CurrentState = state;
            _calmView.SetActive(state == State.Calm);
            _riledView.SetActive(state == State.Riled);
            OnStateChanged?.Invoke(CurrentState);
        }

        private void Die()
        {
            CurrencySystem.Instance.Add(_config.meatCurrency, _config.meatDropAmount);
            OnDied?.Invoke(this);
            transform.DOScale(Vector3.zero, _config.deathDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() => Destroy(gameObject));
        }
    }
}
