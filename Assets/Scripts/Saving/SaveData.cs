using System;
using System.Collections.Generic;

namespace Potato.Saving
{
    [Serializable]
    public class SaveData
    {
        public List<CurrencyEntry> currencies = new();
        public bool wellBuilt;
        public int potatoStage;
        public int waterTicksAccumulated;
        public double lastSaveTimestamp;

        [Serializable]
        public class CurrencyEntry
        {
            public string id;
            public long amount;
        }
    }
}
