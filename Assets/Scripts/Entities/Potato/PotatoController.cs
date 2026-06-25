using System;
using UnityEngine;
using Potato.Currencies;

namespace Potato.Entities.Potato
{
    public class PotatoController : MonoBehaviour
    {
        public enum State { Idle, Sad, Growing, Ready }

        [SerializeField] private PotatoConfig _config;

        public State CurrentState { get; private set; } = State.Idle;
        public int Stage { get; private set; }
        public int WaterTicks { get; private set; }

        private int _wormsNearby;

        public event Action<State> OnStateChanged;
        public event Action<int> OnStageChanged;

        public void OnWormEnter()
        {
            _wormsNearby++;
            SetState(State.Sad);
        }

        public void OnWormExit()
        {
            _wormsNearby = Mathf.Max(0, _wormsNearby - 1);
            if (_wormsNearby == 0 && CurrentState == State.Sad)
                SetState(State.Idle);
        }

        public void ReceiveWaterTick()
        {
            if (CurrentState == State.Ready || CurrentState == State.Sad) return;
            WaterTicks++;
            if (WaterTicks >= _config.waterTicksToGrow)
            {
                WaterTicks = 0;
                GrowOneStage();
            }
        }

        public bool TryHarvest(CurrencySystem currencies)
        {
            if (CurrentState != State.Ready) return false;
            currencies.Add(_config.softCurrency, _config.sellPrice);
            Stage = 0;
            WaterTicks = 0;
            SetState(State.Idle);
            return true;
        }

        public void RestoreState(int stage, int waterTicks)
        {
            Stage = stage;
            WaterTicks = waterTicks;
            SetState(stage >= _config.maxStage ? State.Ready : State.Idle);
            OnStageChanged?.Invoke(Stage);
        }

        private void GrowOneStage()
        {
            Stage++;
            OnStageChanged?.Invoke(Stage);
            SetState(Stage >= _config.maxStage ? State.Ready : State.Growing);
        }

        private void SetState(State next)
        {
            if (CurrentState == next) return;
            CurrentState = next;
            OnStateChanged?.Invoke(CurrentState);
        }
    }
}
