using System;
using UnityEngine;
using Potato.Interactions;


namespace Potato.Currencies
{
    public class CurrencySource : MonoBehaviour, ICountSource, ICountTarget
    {
        [SerializeField] private CurrencyConfig _currency;
        [SerializeField] private bool _animated = true;

        public CurrencyConfig Currency => _currency;
        public int Count => (int)CurrencySystem.Instance.Get(_currency);
        public int Capacity => int.MaxValue;
        public bool IsFull => false;
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

        public int Take(int amount)
        {
            int actual = Math.Min(amount, Count);
            CurrencySystem.Instance.TrySpend(_currency, actual);
            return actual;
        }

        public int TakeAll() => Take(Count);

        public void Add(int amount) => CurrencySystem.Instance.Add(_currency, amount, transform.position, _animated);
    }
}
