using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Potato.Currencies;
using Potato.Interactions;

namespace Potato.Entities.Potato
{
    public class PotatoUpgrader : MonoBehaviour, IInteractable
    {
        [SerializeField] private PotatoLevelConfig _config;

        public PotatoLevelConfig Config => _config;

        public int CurrentLevel { get; private set; }
        public PotatoLevelConfig.LevelData CurrentData => _config.Get(CurrentLevel);
        public bool IsMaxLevel => CurrentLevel >= _config.levels.Length - 1;

        public event Action<int> OnLevelChanged;

        private void Awake() => ApplyScale(animate: false);

        public void Interact() => TryUpgrade(CurrencySystem.Instance);

        public bool TryUpgrade(CurrencySystem currencies)
        {
            if (IsMaxLevel) return false;
            var next = _config.Get(CurrentLevel + 1);
            if (!currencies.TrySpend(next.upgradeCurrency, next.upgradeCost)) return false;
            CurrentLevel++;
            ApplyScale(animate: true);
            OnLevelChanged?.Invoke(CurrentLevel);
            return true;
        }

        private void ApplyScale(bool animate)
        {
            float scale = CurrentData.scale;
            if (animate)
                transform.DOScale(Vector3.one * scale, 0.4f).SetEase(Ease.OutBack);
            else
                transform.localScale = Vector3.one * scale;
        }

        [Button("Upgrade (cheat)")]
        private void CheatUpgrade() => TryUpgrade(CurrencySystem.Instance);
    }
}
