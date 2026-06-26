using System;
using Potato.Currencies;

namespace Potato.Interactions
{
    public interface ICountSource
    {
        CurrencyConfig Currency { get; }
        int Count { get; }
        event Action OnCountChanged;
        int Take(int amount);
        int TakeAll();
    }
}
