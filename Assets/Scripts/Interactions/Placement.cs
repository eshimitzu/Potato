using System;
using UnityEngine;

namespace Potato.Interactions
{
    public class Placement : StandingPlacement
    {
        [SerializeField] private GameObject _sourceObject;
        [SerializeField] private GameObject _targetObject;
        [SerializeField] private int _amountPerTick = 1;
        [SerializeField] private float _conversionRate = 1f;
        [SerializeField] private bool _applySourceIcon;

        private ICountSource _source;
        private ICountTarget _target;

        public event Action<int> OnTransferred;

        protected override void OnSetup()
        {
            _source = _sourceObject != null ? _sourceObject.GetComponent<ICountSource>() : null;
            _target = _targetObject != null ? _targetObject.GetComponent<ICountTarget>() : null;

            if (_source != null)
            {
                _source.OnCountChanged += RefreshCounter;
                RefreshIcon();
                RefreshCounter();
            }
            else if (_target != null)
            {
                _target.OnCountChanged += RefreshCounter;
                RefreshIcon();
                RefreshCounter();
            }
        }

        private void OnDestroy()
        {
            if (_source != null) _source.OnCountChanged -= RefreshCounter;
            if (_target != null) _target.OnCountChanged -= RefreshCounter;
        }

        protected override void OnTick()
        {
            if (_source != null && _target != null)
                TransferSourceToTarget();
            else if (_source != null)
                TakeFromSource();
            else if (_target != null)
                GiveToTarget();
        }

        private void TransferSourceToTarget()
        {
            if (_source.Count == 0 || _target.IsFull) return;
            int available = Math.Min(_source.Count, _amountPerTick);
            int produced = Mathf.Max(1, Mathf.RoundToInt(available * _conversionRate));
            int room = _target.Capacity - _target.Count;
            int take = available;
            int give = produced;
            if (give > room)
            {
                give = room;
                take = Mathf.CeilToInt(give / _conversionRate);
            }
            if (take <= 0 || give <= 0) return;
            _source.Take(take);
            _target.Add(give);
            OnTransferred?.Invoke(give);
        }
    
        private void TakeFromSource()
        {
            if (_source.Count == 0) return;
            int taken = _source.TakeAll();
            OnTransferred?.Invoke(taken);
        }

        private void GiveToTarget()
        {
            if (_target.IsFull) return;
            _target.Add(_amountPerTick);
            OnTransferred?.Invoke(_amountPerTick);
        }

        private void RefreshIcon()
        {
            if (_applySourceIcon)
            {
                SetIcon(_source.Currency.icon);
            }
        }

        private void RefreshCounter()
        {
            if (_source != null)
                SetCounter(_source.Count.ToString());
            else if (_target != null)
                SetCounter($"{_target.Count}/{_target.Capacity}");
        }
    }
}
