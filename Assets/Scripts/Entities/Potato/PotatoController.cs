using System;
using DG.Tweening;
using UnityEngine;

namespace Potato.Entities.Potato
{
    public class PotatoController : MonoBehaviour
    {
        public enum State { Idle, Growing, Ready }

        [SerializeField] private PotatoConfig _config;
        [SerializeField] private PotatoNeed _need;
        [SerializeField] private Renderer _faceRenderer;
        [SerializeField] private Material _smileMaterial;
        [SerializeField] private Material _sadMaterial;
        [SerializeField] private Transform _squashRoot;

        public State CurrentState { get; private set; } = State.Idle;
        public int Stage { get; private set; }
        public int WaterTicks { get; private set; }

        private int _wormsNearby;
        private bool _needActive;
        private Tween _idleTween;

        private bool IsSad => _wormsNearby > 0 || _needActive;

        public event Action<State> OnStateChanged;
        public event Action<int> OnStageChanged;

        private void Awake() => RefreshFace();

        private void Start()
        {
            if (_need == null) return;
            _need.OnNeedAppeared += OnNeedAppeared;
            _need.OnNeedSatisfied += OnNeedSatisfied;
        }

        private void OnDestroy()
        {
            if (_need == null) return;
            _need.OnNeedAppeared -= OnNeedAppeared;
            _need.OnNeedSatisfied -= OnNeedSatisfied;
        }

        public void OnWormEnter()
        {
            _wormsNearby++;
            RefreshFace();
        }

        public void OnWormExit()
        {
            _wormsNearby = Mathf.Max(0, _wormsNearby - 1);
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

        public void ReceiveWaterTick()
        {
            if (CurrentState == State.Ready || IsSad) return;
            WaterTicks++;
            if (WaterTicks >= _config.waterTicksToGrow)
            {
                WaterTicks = 0;
                GrowOneStage();
            }
        }

        public void RestoreState(int stage, int waterTicks)
        {
            Stage = stage;
            WaterTicks = waterTicks;
            SetState(stage >= _config.maxStage ? State.Ready : State.Idle);
            OnStageChanged?.Invoke(Stage);
        }

        private void GrowOneStage()
        {
            Stage++;
            OnStageChanged?.Invoke(Stage);
            SetState(Stage >= _config.maxStage ? State.Ready : State.Growing);
        }

        private void SetState(State next)
        {
            if (CurrentState == next) return;
            CurrentState = next;
            OnStateChanged?.Invoke(CurrentState);
        }

        private void RefreshFace()
        {
            var materials = _faceRenderer.sharedMaterials;
            materials[1] = IsSad ? _sadMaterial : _smileMaterial;
            _faceRenderer.sharedMaterials = materials;
            if (IsSad) StopIdleSquash();
            else PlayIdleSquash();
        }

        private void PlayIdleSquash()
        {
            _idleTween?.Kill(true);
            _idleTween = _squashRoot
                .DOScale(new Vector3(1.05f, 0.92f, 1.05f), 0.9f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void StopIdleSquash()
        {
            _idleTween?.Kill(true);
            _squashRoot.localScale = Vector3.one;
        }
    }
}
