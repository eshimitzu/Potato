using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Potato.Interactions
{
    [RequireComponent(typeof(Collider))]
    public abstract class StandingPlacement : MonoBehaviour
    {
        [SerializeField] private Transform _visual;
        [SerializeField] private SpriteRenderer _icon;
        [SerializeField] protected TMP_Text Counter;
        [SerializeField] private float _enterDelay = 0.5f;
        [SerializeField] private float _interval = 0.02f;
        [SerializeField] private string _playerTag = "Player";

        private Coroutine _tickCoroutine;
        private Vector3 _baseScale;

        protected bool PlayerInside { get; private set; }

        private void Awake()
        {
            if (_visual != null) _baseScale = _visual.localScale;
        }

        private void Start() => OnSetup();

        protected abstract void OnSetup();
        protected abstract void OnTick();

        protected void SetIcon(Sprite sprite)
        {
            if (_icon != null) _icon.sprite = sprite;
        }

        protected void SetCounter(string text)
        {
            if (Counter != null) Counter.text = text;
        }

        public void SetVisible(bool visible)
        {
            GetComponent<Collider>().enabled = visible;
            if (_visual == null) return;
            _visual.DOKill();
            _visual.DOScale(visible ? _baseScale : Vector3.zero, 0.2f)
                .SetEase(visible ? Ease.OutBack : Ease.InBack)
                .SetLink(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_playerTag)) return;
            PlayerInside = true;
            _visual?.DOScale(_baseScale * 1.2f, 0.2f).SetEase(Ease.OutBack).SetLink(gameObject);
            _tickCoroutine = StartCoroutine(TickLoop());
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(_playerTag)) return;
            PlayerInside = false;
            _visual?.DOScale(_baseScale, 0.15f).SetEase(Ease.InOutSine).SetLink(gameObject);
            if (_tickCoroutine != null) StopCoroutine(_tickCoroutine);
        }

        private IEnumerator TickLoop()
        {
            yield return new WaitForSeconds(_enterDelay);
            while (true)
            {
                yield return new WaitForSeconds(_interval);
                OnTick();
            }
        }
    }
}
