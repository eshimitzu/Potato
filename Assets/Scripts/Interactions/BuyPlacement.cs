using System;
using UnityEngine;
using Potato.Currencies;

namespace Potato.Interactions
{
    public class BuyPlacement : StandingPlacement
    {
        [SerializeField] private CurrencyConfig _costCurrency;
        [SerializeField] private int _totalCost = 10;
        [SerializeField] private int _costPerTick = 1;

        private int _remaining;
        private bool _purchased = false;
        
        public event Action OnPurchased;

        public CurrencyConfig CostCurrency => _costCurrency;


        public void Setup(CurrencyConfig currency, int totalCost)
        {
            _costCurrency = currency;
            _totalCost = totalCost;
            _remaining = totalCost;
            _purchased = false;
            SetIcon(currency.icon);
            RefreshCounter();
        }

        public void Refresh()
        {
            _remaining = _totalCost;
            _purchased = false;
            RefreshCounter();
        }

        
        protected override void OnSetup()
        {
            SetIcon(CostCurrency.icon);
            _remaining = _totalCost;
            RefreshCounter();
        }

        
        protected override void OnTick()
        {
            int spend = Mathf.Min(_costPerTick, _remaining);
            if (!CurrencySystem.Instance.TrySpend(CostCurrency, spend)) return;
            _remaining -= spend;
            RefreshCounter();
            if (!_purchased && _remaining <= 0)
            {
                _remaining = 0;
                _purchased = true;
                OnPurchased?.Invoke();
                RefreshCounter();
            }
        }
        

        private void RefreshCounter() => SetCounter(_remaining.ToString());
    }
}
