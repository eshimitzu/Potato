using System;
using DG.Tweening;
using UnityEngine;
using Potato.Entities.Worm;

namespace Potato.Entities.Potato
{
    public class PotatoController : MonoBehaviour
    {
        public enum State { Idle, Sad }

        [SerializeField] private WormSpawner _wormSpawner;
        [SerializeField] private PotatoNeed _need;
        [SerializeField] private Renderer _faceRenderer;
        [SerializeField] private Material _smileMaterial;
        [SerializeField] private Material _sadMaterial;
        [SerializeField] private Transform _squashRoot;

        public State CurrentState { get; private set; } = State.Idle;

        private bool _wormsAlive;
        private bool _needActive;
        private Tween _idleTween;

        private bool IsSad => _wormsAlive || _needActive;

        public event Action<State> OnStateChanged;

        private void Awake() => RefreshFace();

        private void Start()
        {
            if (_wormSpawner != null)
            {
                _wormsAlive = _wormSpawner.AnyAlive;
                _wormSpawner.OnAnyAliveChanged += OnAnyAliveChanged;
            }
            if (_need != null)
            {
                _need.OnNeedAppeared += OnNeedAppeared;
                _need.OnNeedSatisfied += OnNeedSatisfied;
            }
        }

        private void OnDestroy()
        {
            if (_wormSpawner != null) _wormSpawner.OnAnyAliveChanged -= OnAnyAliveChanged;
            if (_need != null)
            {
                _need.OnNeedAppeared -= OnNeedAppeared;
                _need.OnNeedSatisfied -= OnNeedSatisfied;
            }
        }

        private void OnAnyAliveChanged(bool alive)
        {
            _wormsAlive = alive;
            RefreshFace();
        }

        private void OnNeedAppeared(PotatoNeed.Need _)
        {
            _needActive = true;
            RefreshFace();
        }

        private void OnNeedSatisfied()
        {
            _needActive = false;
            RefreshFace();
        }

        private void RefreshFace()
        {
            var materials = _faceRenderer.sharedMaterials;
            materials[1] = IsSad ? _sadMaterial : _smileMaterial;
            _faceRenderer.sharedMaterials = materials;

            State next = IsSad ? State.Sad : State.Idle;
            if (next == CurrentState) return;
            CurrentState = next;
            if (IsSad) StopIdleSquash();
            else PlayIdleSquash();
            OnStateChanged?.Invoke(CurrentState);
        }

        private void PlayIdleSquash()
        {
            _idleTween?.Kill(true);
            _idleTween = _squashRoot
                .DOScale(new Vector3(1.05f, 0.92f, 1.05f), 0.9f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);
        }

        private void StopIdleSquash()
        {
            _idleTween?.Kill(true);
            _squashRoot.localScale = Vector3.one;
        }
    }
}
