using System;
using DG.Tweening;
using UnityEngine;
using Potato.Currencies;

namespace Potato.Entities.Potato
{
    public class PotatoController : MonoBehaviour
    {
        public enum State { Idle, Sad, Growing, Ready }

        [SerializeField] private PotatoConfig _config;
        [SerializeField] private Renderer _faceRenderer;
        [SerializeField] private Material _smileMaterial;
        [SerializeField] private Material _sadMaterial;

        public State CurrentState { get; private set; } = State.Idle;
        public int Stage { get; private set; }
        public int WaterTicks { get; private set; }

        private int _wormsNearby;
        private Tween _idleTween;

        public event Action<State> OnStateChanged;
        public event Action<int> OnStageChanged;

        private void Awake() => PlayIdleSquash();

        public void OnWormEnter()
        {
            _wormsNearby++;
            SetState(State.Sad);
        }

        public void OnWormExit()
        {
            _wormsNearby = Mathf.Max(0, _wormsNearby - 1);
            if (_wormsNearby == 0 && CurrentState == State.Sad)
                SetState(State.Idle);
        }

        public void ReceiveWaterTick()
        {
            if (CurrentState == State.Ready || CurrentState == State.Sad) return;
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
            _faceRenderer.material = next == State.Sad ? _sadMaterial : _smileMaterial;

            if (next == State.Idle)
                PlayIdleSquash();
            else
                StopIdleSquash();

            OnStateChanged?.Invoke(CurrentState);
        }

        private void PlayIdleSquash()                                                                     
        {                                                                                                 
            _idleTween?.Kill(true);                                                                       
            _idleTween = transform                                                                        
                .DOScale(new Vector3(1.05f, 0.92f, 1.05f), 0.9f)                                          
                .SetEase(Ease.InOutSine)                                                                  
                .SetLoops(-1, LoopType.Yoyo);                                                             
        }                                  

        private void StopIdleSquash()
        {
            _idleTween?.Kill(true);
            transform.localScale = Vector3.one;
        }
    }
}
