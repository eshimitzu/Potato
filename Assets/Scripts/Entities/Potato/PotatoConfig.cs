using UnityEngine;
using Potato.Currencies;

namespace Potato.Entities.Potato
{
    [CreateAssetMenu(menuName = "Potato/PotatoConfig", fileName = "PotatoConfig")]
    public class PotatoConfig : ScriptableObject
    {
        public int waterTicksToGrow = 3;
        public int maxStage = 3;
        public CurrencyConfig softCurrency;
        public int sellPrice = 10;
    }
}
