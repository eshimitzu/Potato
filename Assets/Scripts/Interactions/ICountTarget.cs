using System;

namespace Potato.Interactions
{
    public interface ICountTarget
    {
        int Count { get; }
        int Capacity { get; }
        bool IsFull { get; }
        event Action OnCountChanged;
        void Add(int amount);
    }
}
