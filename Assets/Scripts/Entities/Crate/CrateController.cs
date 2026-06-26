using System;
using UnityEngine;
using Potato.Currencies;
using Potato.Interactions;
using Potato.Entities.Potato;

namespace Potato.Entities.Crate
{
    public class CrateController : MonoBehaviour, IInteractable, ICountSource, ICountTarget
    {
        public enum State { Empty, Half, Full }

        [SerializeField] private GameObject _emptyView;
        [SerializeField] private GameObject _halfView;
        [SerializeField] private GameObject _fullView;

        public CurrencyConfig Currency => null;
        public int Count { get; private set; }
        public State CurrentState { get; private set; }

        private PotatoUpgrader _upgrader;

        public int Capacity => _upgrader.CurrentData.maxStorage;
        public bool IsFull => Count >= Capacity;

        public event Action<State> OnStateChanged;
        public event Action<int> OnCollected;
        public event Action OnCountChanged;

        public void Initialize(PotatoUpgrader upgrader)
        {
            _upgrader = upgrader;
            ApplyView(State.Empty);
        }

        public void Add(int amount)
        {
            int toAdd = Math.Min(amount, Capacity - Count);
            if (toAdd <= 0) return;
            Count += toAdd;
            OnCountChanged?.Invoke();
            UpdateState();
        }

        public void AddPotato() => Add(1);

        public void Interact()
        {
            if (Count == 0) return;
            TakeAll();
        }

        public int Take(int amount)
        {
            int taken = Math.Min(amount, Count);
            if (taken <= 0) return 0;
            Count -= taken;
            OnCountChanged?.Invoke();
            OnCollected?.Invoke(taken);
            UpdateState();
            return taken;
        }

        public int TakeAll() => Take(Count);

        private void UpdateState()
        {
            State next = Count == 0 ? State.Empty
                : Count >= Capacity ? State.Full
                : State.Half;

            if (next == CurrentState) return;
            CurrentState = next;
            ApplyView(next);
            OnStateChanged?.Invoke(CurrentState);
        }

        private void ApplyView(State state)
        {
            _emptyView.SetActive(state == State.Empty);
            _halfView.SetActive(state == State.Half);
            _fullView.SetActive(state == State.Full);
        }
    }
}
