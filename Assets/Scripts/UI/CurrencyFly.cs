using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Potato.UI
{
    public class CurrencyFly : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private float _duration = 0.6f;
        [SerializeField] private float _scaleUpDuration = 0.15f;

        public void SetIcon(Sprite sprite)
        {
            if (_icon != null) _icon.sprite = sprite;
        }

        public void Play(Vector3 fromScreenPos, Vector3 toScreenPos, Action onComplete = null)
        {
            transform.position = fromScreenPos;
            transform.localScale = Vector3.zero;

            transform.DOScale(Vector3.one, _scaleUpDuration)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                    transform.DOMove(toScreenPos, _duration)
                        .SetEase(Ease.InQuart)
                        .OnComplete(() =>
                        {
                            onComplete?.Invoke();
                            Destroy(gameObject);
                        }));
        }
    }
}
