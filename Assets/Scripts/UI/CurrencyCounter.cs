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

        private void Start()
        {
            _canvas = GetComponentInParent<Canvas>();
            if (_icon != null) _icon.sprite = _config.icon;
            Refresh(CurrencySystem.Instance.Get(_config));
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
            if (id == _config.id) Refresh(amount);
        }

        private void OnAddedFromSource(string id, long amount, Vector3 worldPos)
        {
            if (id != _config.id || _config.prefab == null || _canvas == null) return;
            Vector3 screenStart = Camera.main.WorldToScreenPoint(worldPos);
            var go = Instantiate(_config.prefab, _canvas.transform);
            go.GetComponent<CurrencyFly>()?.Play(screenStart, transform.position);
        }

        private void Refresh(long amount) => _label.text = amount.ToString();

    }
}
