using System;
using UnityEngine;
using Potato.Currencies;

namespace Potato.Entities.Potato
{
    [CreateAssetMenu(menuName = "Potato/PotatoLevelConfig", fileName = "PotatoLevelConfig")]
    public class PotatoLevelConfig : ScriptableObject
    {
        [Serializable]
        public class LevelData
        {
            public CurrencyConfig upgradeCurrency;
            public int upgradeCost;
            public float scale = 1f;
            public float potatoesPerMinute = 10f;
            public int maxStorage = 10;
        }

        public LevelData[] levels;

        public float flyDuration = 0.8f;
        public float flyJumpPower = 2f;
        public GameObject potatoPrefab;

        public LevelData Get(int level) => levels[Mathf.Clamp(level, 0, levels.Length - 1)];
    }
}
