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
        [SerializeField] private Transform _scaleRoot;
        [SerializeField] private BuyPlacement _upgradePlacement;

        public PotatoLevelConfig Config => _config;

        public int CurrentLevel { get; private set; }
        public PotatoLevelConfig.LevelData CurrentData => _config.Get(CurrentLevel);
        public bool IsMaxLevel => CurrentLevel >= _config.levels.Length - 1;

        public event Action<int> OnLevelChanged;

        private void Awake() => ApplyScale(animate: false);

        private void Start()
        {
            if (_upgradePlacement == null) return;
            _upgradePlacement.OnPurchased += OnPlacementPurchased;
            RefreshPlacement();
        }

        private void OnDestroy()
        {
            if (_upgradePlacement != null)
                _upgradePlacement.OnPurchased -= OnPlacementPurchased;
        }

        public void Interact() => TryUpgrade(CurrencySystem.Instance);

        public bool TryUpgrade(CurrencySystem currencies)
        {
            if (IsMaxLevel) return false;
            var next = _config.Get(CurrentLevel + 1);
            if (!currencies.TrySpend(next.upgradeCurrency, next.upgradeCost)) return false;
            ApplyUpgrade();
            return true;
        }

        private void OnPlacementPurchased()
        {
            if (IsMaxLevel) return;
            ApplyUpgrade();
        }

        private void ApplyUpgrade()
        {
            CurrentLevel++;
            ApplyScale(animate: true);
            OnLevelChanged?.Invoke(CurrentLevel);
            RefreshPlacement();
        }

        private void RefreshPlacement()
        {
            if (_upgradePlacement == null) return;
            if (IsMaxLevel)
            {
                _upgradePlacement.SetVisible(false);
                return;
            }
            var next = _config.Get(CurrentLevel + 1);
            _upgradePlacement.SetVisible(true);
            _upgradePlacement.Setup(next.upgradeCurrency, next.upgradeCost);
        }

        private void ApplyScale(bool animate)
        {
            float scale = CurrentData.scale;
            if (animate)
                _scaleRoot.DOScale(Vector3.one * scale, 0.4f).SetEase(Ease.OutBack).SetLink(gameObject);
            else
                _scaleRoot.localScale = Vector3.one * scale;
        }

        [Button("Upgrade (cheat)")]
        private void CheatUpgrade() => TryUpgrade(CurrencySystem.Instance);
    }
}
