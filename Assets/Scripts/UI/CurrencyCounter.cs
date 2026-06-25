using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Potato.Currencies;

namespace Potato.UI
{
    public class CurrencyCounter : MonoBehaviour
    {
        [SerializeField] private CurrencyConfig _config;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private Image _icon;

        private Canvas _canvas;
        private long _displayedAmount;

        private void Start()
        {
            _canvas = GetComponentInParent<Canvas>();
            if (_icon != null) _icon.sprite = _config.icon;
            _displayedAmount = CurrencySystem.Instance.Get(_config);
            Refresh(_displayedAmount);
            CurrencySystem.Instance.OnChanged += OnCurrencyChanged;
            CurrencySystem.Instance.OnAddedFromSource += OnAddedFromSource;
        }

        private void OnDestroy()
        {
            if (CurrencySystem.Instance == null) return;
            CurrencySystem.Instance.OnChanged -= OnCurrencyChanged;
            CurrencySystem.Instance.OnAddedFromSource -= OnAddedFromSource;
        }

        private void OnCurrencyChanged(string id, long amount)  
        {
            if (id != _config.id) return;
            if (amount > _displayedAmount) return;
            _displayedAmount = amount;
            Refresh(amount);
        }

        private void OnAddedFromSource(string id, long amount, Vector3 worldPos, bool animated)
        {
            if (id != _config.id) return;
            if (!animated || _config.prefab == null || _canvas == null)
            {
                OnFlyComplete();
                return;
            }
            Vector3 screenStart = Camera.main.WorldToScreenPoint(worldPos);
            var go = Instantiate(_config.prefab, _canvas.transform);
            var fly = go.GetComponent<CurrencyFly>();
            fly.SetIcon(_config.icon);
            fly.Play(screenStart, transform.position, OnFlyComplete);
        }

        private void OnFlyComplete()
        {
            _displayedAmount = CurrencySystem.Instance.Get(_config);
            Refresh(_displayedAmount);
        }

        private void Refresh(long amount) => _label.text = amount.ToString();

    }
}
