using System.Collections.Generic;
using UnityEngine;
using Potato.Currencies;
using Potato.Saving;
using Potato.Entities.Potato;
using Potato.Entities.Well;

namespace Potato.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private CurrencySystem _currencies;
        [SerializeField] private SaveSystem _saveSystem;
        [SerializeField] private PotatoController _potato;
        [SerializeField] private CurrencyConfig[] _allCurrencies;
        // [SerializeField] private WellController _well;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start()
        {
            SaveData data = _saveSystem.Load();
            ApplySaveData(data);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus) _saveSystem.Save(CollectSaveData());
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus) _saveSystem.Save(CollectSaveData());
        }

        public SaveData CollectSaveData()
        {
            var data = new SaveData
            {
                // wellBuilt = _well.IsBuilt,
                // potatoStage = _potato.Stage,
                // waterTicksAccumulated = _potato.WaterTicks,
            };
            foreach (var kvp in _currencies.GetAllBalances())
                data.currencies.Add(new SaveData.CurrencyEntry { id = kvp.Key, amount = kvp.Value });
            return data;
        }

        private void ApplySaveData(SaveData data)
        {
            // var savedIds = new HashSet<string>();
            // foreach (var entry in data.currencies)
            // {
            //     _currencies.Set(entry.id, entry.amount);
            //     savedIds.Add(entry.id);
            // }
            //
            // foreach (var cfg in _allCurrencies)
            //     if (!savedIds.Contains(cfg.id) && cfg.initialAmount > 0)
            //         _currencies.Set(cfg.id, cfg.initialAmount);

            // _potato.RestoreState(data.potatoStage, data.waterTicksAccumulated);

            // if (data.wellBuilt) _well.Build();
        }
    }
}
