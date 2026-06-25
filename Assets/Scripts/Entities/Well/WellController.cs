using System;
using System.Collections;
using UnityEngine;
using Potato.Currencies;
using Potato.Entities.Potato;

namespace Potato.Entities.Well
{
    public class WellController : MonoBehaviour
    {
        [SerializeField] private WellConfig _config;
        [SerializeField] private PotatoController _potato;

        public bool IsBuilt { get; private set; }

        public event Action OnBuilt;
        public event Action OnWaterTick;

        public bool TryBuild(CurrencySystem currencies)
        {
            if (IsBuilt) return false;
            // Spend meat first, then soft. Roll back meat if soft is insufficient.
            if (!currencies.TrySpend(_config.meatCurrency, _config.meatCost)) return false;
            if (!currencies.TrySpend(_config.softCurrency, _config.softCost))
            {
                currencies.Add(_config.meatCurrency, _config.meatCost);
                return false;
            }
            Build();
            return true;
        }

        public void Build()
        {
            IsBuilt = true;
            gameObject.SetActive(true);
            OnBuilt?.Invoke();
            StartCoroutine(WaterLoop());
        }

        private IEnumerator WaterLoop()
        {
            while (IsBuilt)
            {
                yield return new WaitForSeconds(_config.waterTickInterval);
                _potato.ReceiveWaterTick();
                OnWaterTick?.Invoke();
            }
        }
    }
}
