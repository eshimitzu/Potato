using UnityEngine;
using Potato.Currencies;

namespace Potato.Entities.Worm
{
    [CreateAssetMenu(menuName = "Potato/WormConfig", fileName = "WormConfig")]
    public class WormConfig : ScriptableObject
    {
        public int hp = 3;
        public float moveSpeed = 1f;
        public CurrencyConfig meatCurrency;
        public int meatDropAmount = 1;
        public float spawnInterval = 5f;
        public int spawnCount = 3;
    }
}
