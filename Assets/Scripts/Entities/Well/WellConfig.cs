using UnityEngine;
using Potato.Currencies;

namespace Potato.Entities.Well
{
    [CreateAssetMenu(menuName = "Potato/WellConfig", fileName = "WellConfig")]
    public class WellConfig : ScriptableObject
    {
        public CurrencyConfig meatCurrency;
        public int meatCost = 5;
        public CurrencyConfig softCurrency;
        public int softCost = 20;
        public float waterTickInterval = 10f;
        public CurrencyConfig waterCurrency;
        public int waterPerTick = 1;
    }
}
