using System;
using System.Collections;
using UnityEngine;
using Potato.Currencies;

namespace Potato.Entities.Potato
{
    public class PotatoNeed : MonoBehaviour
    {
        [Serializable]
        public class Need
        {
            public CurrencyConfig currency;
            public int amount = 1;
        }

        [SerializeField] private Need[] _possibleNeeds;
        [SerializeField] private float _appearInterval = 15f;

        public Need CurrentNeed { get; private set; }

        public event Action<Need> OnNeedAppeared;
        public event Action OnNeedSatisfied;

        private void Start() => StartCoroutine(NeedLoop());

        private IEnumerator NeedLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(_appearInterval);
                if (CurrentNeed == null)
                    ShowRandomNeed();
            }
        }

        private void ShowRandomNeed()
        {
            if (_possibleNeeds.Length == 0) return;
            CurrentNeed = _possibleNeeds[UnityEngine.Random.Range(0, _possibleNeeds.Length)];
            OnNeedAppeared?.Invoke(CurrentNeed);
        }

        public bool TrySatisfy()
        {
            if (CurrentNeed == null) return false;
            if (!CurrencySystem.Instance.TrySpend(CurrentNeed.currency, CurrentNeed.amount)) return false;
            CurrentNeed = null;
            OnNeedSatisfied?.Invoke();
            return true;
        }
    }
}
