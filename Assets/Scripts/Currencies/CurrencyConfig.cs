using NaughtyAttributes;
using UnityEngine;

namespace Potato.Currencies
{
    [CreateAssetMenu(menuName = "Potato/Currency", fileName = "Currency_New")]
    public class CurrencyConfig : ScriptableObject
    {
        public string id;
        public string displayName;
        public Sprite icon;
        public GameObject prefab;
        public int initialAmount;

        [Button("+ 100")]
        private void CheatAdd100() => CurrencySystem.Instance.Add(this, 100, Vector3.zero, false);
    }
}
