using UnityEngine;
using Potato.Entities.Potato;

namespace Potato.Interactions
{
    public class NeedReceiverPlacement : StandingPlacement
    {
        [SerializeField] private PotatoNeed _potatoNeed;

        protected override void OnSetup()
        {
            _potatoNeed.OnNeedAppeared += OnNeedAppeared;
            _potatoNeed.OnNeedSatisfied += OnNeedSatisfied;
            RefreshDisplay();
        }

        private void OnDestroy()
        {
            _potatoNeed.OnNeedAppeared -= OnNeedAppeared;
            _potatoNeed.OnNeedSatisfied -= OnNeedSatisfied;
        }

        private void OnNeedAppeared(PotatoNeed.Need need)
        {
            SetIcon(need.currency.icon);
            SetCounter(need.amount.ToString());
            SetVisible(true);
        }

        private void OnNeedSatisfied()
        {
            SetVisible(false);
        }

        private void RefreshDisplay()
        {
            if (_potatoNeed.CurrentNeed != null)
                OnNeedAppeared(_potatoNeed.CurrentNeed);
            else
                SetVisible(false);
        }

        protected override void OnTick()
        {
            _potatoNeed.TrySatisfy();
        }
    }
}
