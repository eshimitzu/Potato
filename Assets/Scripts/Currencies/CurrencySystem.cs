using System;
using System.Collections.Generic;
using UnityEngine;

namespace Potato.Currencies
{
    public class CurrencySystem : MonoBehaviour
    {
        public static CurrencySystem Instance { get; private set; }

        private readonly Dictionary<string, long> _balances = new();

        public event Action<string, long> OnChanged;
        public event Action<string, long, Vector3, bool> OnAddedFromSource;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public long Get(CurrencyConfig cfg) => _balances.GetValueOrDefault(cfg.id, 0);

        public void Add(CurrencyConfig cfg, long amount, Vector3 sourceWorldPos = default, bool animated = true)
        {
            if (amount <= 0) return;
            _balances.TryGetValue(cfg.id, out long current);
            _balances[cfg.id] = current + amount;
            OnChanged?.Invoke(cfg.id, _balances[cfg.id]);
            if (sourceWorldPos != default)
                OnAddedFromSource?.Invoke(cfg.id, amount, sourceWorldPos, animated);
        }

        public bool TrySpend(CurrencyConfig cfg, long amount)
        {
            if (amount <= 0) return true;
            if (!_balances.TryGetValue(cfg.id, out long current) || current < amount) return false;
            _balances[cfg.id] = current - amount;
            OnChanged?.Invoke(cfg.id, _balances[cfg.id]);
            return true;
        }

        public void Set(string currencyId, long amount)
        {
            _balances[currencyId] = amount;
            OnChanged?.Invoke(currencyId, amount);
        }

        public Dictionary<string, long> GetAllBalances() => new(_balances);
    }
}
