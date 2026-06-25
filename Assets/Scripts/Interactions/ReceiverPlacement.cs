using System;
using UnityEngine;
using Potato.Currencies;

namespace Potato.Interactions
{
    public class ReceiverPlacement : StandingPlacement
    {
        [SerializeField] private CurrencyConfig _currency;
        [SerializeField] private int _amountPerTick = 1;

        public event Action<bool> OnReceived;

        protected override void OnSetup()
        {
            SetIcon(_currency.icon);
            RefreshCounter();
            CurrencySystem.Instance.OnChanged += OnCurrencyChanged;
        }

        private void OnDestroy()
        {
            if (CurrencySystem.Instance != null)
                CurrencySystem.Instance.OnChanged -= OnCurrencyChanged;
        }

        private void OnCurrencyChanged(string id, long _)
        {
            if (id == _currency.id) RefreshCounter();
        }

        private void RefreshCounter() =>
            SetCounter(CurrencySystem.Instance.Get(_currency).ToString());

        protected override void OnTick()
        {
            bool success = CurrencySystem.Instance.TrySpend(_currency, _amountPerTick);
            OnReceived?.Invoke(success);
        }
    }
}
