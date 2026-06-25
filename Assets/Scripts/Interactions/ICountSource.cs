using System;

namespace Potato.Interactions
{
    public interface ICountSource
    {
        int Count { get; }
        event Action OnCountChanged;
        int TakeAll();
    }
}
