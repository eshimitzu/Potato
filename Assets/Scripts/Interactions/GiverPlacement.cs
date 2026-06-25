using System;
using UnityEngine;
using Potato.Currencies;

namespace Potato.Interactions
{
    public class GiverPlacement : StandingPlacement
    {
        [SerializeField] private GameObject _source;
        [SerializeField] private CurrencyConfig _rewardCurrency;
        [SerializeField] private int _rewardPerUnit = 1;

        private ICountSource _countSource;

        public event Action<long> OnGiven;

        protected override void OnSetup()
        {
            _countSource = _source.GetComponent<ICountSource>();
            SetIcon(_rewardCurrency.icon);
            RefreshCounter();
            _countSource.OnCountChanged += RefreshCounter;
        }

        private void OnDestroy() => _countSource.OnCountChanged -= RefreshCounter;

        protected override void OnTick()
        {
            if (_countSource.Count == 0) return;
            int taken = _countSource.TakeAll();
            CurrencySystem.Instance.Add(_rewardCurrency, taken * _rewardPerUnit);
            OnGiven?.Invoke(taken * _rewardPerUnit);
        }

        private void RefreshCounter() => SetCounter(_countSource.Count.ToString());
    }
}
