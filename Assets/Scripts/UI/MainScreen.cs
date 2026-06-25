using UnityEngine;

namespace Potato.UI
{
    public class MainScreen : MonoBehaviour
    {
        [SerializeField] private CurrencyCounter[] _currencyCounters;

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}
