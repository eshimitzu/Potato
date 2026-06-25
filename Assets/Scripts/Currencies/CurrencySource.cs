using System;
using UnityEngine;
using Potato.Interactions;

namespace Potato.Currencies
{
    public class CurrencySource : MonoBehaviour, ICountSource
    {
        [SerializeField] private CurrencyConfig _currency;

        public int Count => (int)CurrencySystem.Instance.Get(_currency);
        public event Action OnCountChanged;

        private void Start()
        {
            CurrencySystem.Instance.OnChanged += OnCurrencyChanged;
        }

        private void OnDestroy()
        {
            if (CurrencySystem.Instance != null)
                CurrencySystem.Instance.OnChanged -= OnCurrencyChanged;
        }

        private void OnCurrencyChanged(string id, long _)
        {
            if (id == _currency.id) OnCountChanged?.Invoke();
        }

        public int TakeAll()
        {
            int amount = Count;
            CurrencySystem.Instance.TrySpend(_currency, amount);
            return amount;
        }
    }
}
