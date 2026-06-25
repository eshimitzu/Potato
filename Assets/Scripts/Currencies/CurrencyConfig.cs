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
    }
}
