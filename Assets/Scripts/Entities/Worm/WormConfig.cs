using UnityEngine;
using Potato.Currencies;

namespace Potato.Entities.Worm
{
    [CreateAssetMenu(menuName = "Potato/WormConfig", fileName = "WormConfig")]
    public class WormConfig : ScriptableObject
    {
        public int hp = 3;
        public CurrencyConfig meatCurrency;
        public int meatDropAmount = 1;
        public float spawnInterval = 5f;
        public int maxWorms = 3;
        public float appearDuration = 0.6f;
        public float burrowDepth = 1.5f;
        public float deathDuration = 0.3f;
    }
}
