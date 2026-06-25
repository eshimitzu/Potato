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

        private void Start()
        {
            if (_icon != null) _icon.sprite = _config.icon;
            Refresh(CurrencySystem.Instance.Get(_config));
            CurrencySystem.Instance.OnChanged += OnCurrencyChanged;
        }

        private void OnDestroy()
        {
            if (CurrencySystem.Instance != null)
                CurrencySystem.Instance.OnChanged -= OnCurrencyChanged;
        }

        private void OnCurrencyChanged(string id, long amount)
        {
            if (id == _config.id) Refresh(amount);
        }

        private void Refresh(long amount) => _label.text = amount.ToString();

    }
}
