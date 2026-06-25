using System;
using UnityEngine;
using Potato.Interactions;
using Potato.Entities.Potato;

namespace Potato.Entities.Crate
{
    public class CrateController : MonoBehaviour, IInteractable, ICountSource
    {
        public enum State { Empty, Half, Full }

        [SerializeField] private GameObject _emptyView;
        [SerializeField] private GameObject _halfView;
        [SerializeField] private GameObject _fullView;

        public int Count { get; private set; }
        public State CurrentState { get; private set; }

        private PotatoUpgrader _upgrader;

        private int MaxStorage => _upgrader.CurrentData.maxStorage;
        public bool IsFull => Count >= MaxStorage;

        public event Action<State> OnStateChanged;
        public event Action<int> OnCollected;
        public event Action OnCountChanged;

        public void Initialize(PotatoUpgrader upgrader)
        {
            _upgrader = upgrader;
            ApplyView(State.Empty);
        }

        public void AddPotato()
        {
            if (IsFull) return;
            Count++;
            OnCountChanged?.Invoke();
            UpdateState();
        }

        public void Interact()
        {
            if (Count == 0) return;
            TakeAll();
        }

        public int TakeAll()
        {
            int taken = Count;
            Count = 0;
            OnCountChanged?.Invoke();
            OnCollected?.Invoke(taken);
            UpdateState();
            return taken;
        }

        private void UpdateState()
        {
            State next = Count == 0 ? State.Empty
                : Count >= MaxStorage ? State.Full
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
