    using Potato.Currencies;
using UnityEngine;
using Potato.Entities.Potato;

namespace Potato.Interactions
{
    public class NeedsPlacement : MonoBehaviour
    {
        [SerializeField] private BuyPlacement _buyPlacement;
        [SerializeField] private PotatoNeed _potatoNeed;
        [SerializeField] private CurrencyConfig _needCurrency;

        private void Start()
        {
            _buyPlacement.OnPurchased += OnPurchased;
            _potatoNeed.OnNeedAppeared += OnNeedAppeared;
            _potatoNeed.OnNeedSatisfied += OnNeedSatisfied;
            _buyPlacement.SetVisible(_potatoNeed.CurrentNeed != null);
        }

        private void OnDestroy()
        {
            _buyPlacement.OnPurchased -= OnPurchased;
            _potatoNeed.OnNeedAppeared -= OnNeedAppeared;
            _potatoNeed.OnNeedSatisfied -= OnNeedSatisfied;
        }

        private void OnNeedAppeared(PotatoNeed.Need need)
        {
            if(need.currency != _needCurrency)
                return;
            
            _buyPlacement.Refresh();
            _buyPlacement.SetVisible(true);
        }

        private void OnNeedSatisfied()
        {
            _buyPlacement.SetVisible(false);
        }

        private void OnPurchased() => _potatoNeed.Satisfy();
    }
}
