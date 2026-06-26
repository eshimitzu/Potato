using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Potato.UI
{
    public class CurrencyFly : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private float _duration = 0.6f;
        [SerializeField] private float _scaleUpDuration = 0.15f;
        [SerializeField] private float _arcStrength = 120f;

        public void SetIcon(Sprite sprite)
        {
            if (_icon != null) _icon.sprite = sprite;
        }

        public void Play(Vector3 fromScreenPos, Vector3 toScreenPos, Action onComplete = null)
        {
            transform.position = fromScreenPos;
            transform.localScale = Vector3.zero;

            Vector3 mid = (fromScreenPos + toScreenPos) * 0.5f;
            Vector3 perp = Vector3.Cross((toScreenPos - fromScreenPos).normalized, Vector3.forward);
            mid += perp * Random.Range(-_arcStrength, _arcStrength);
            mid += Vector3.up * Random.Range(0f, _arcStrength);

            var path = new Vector3[] { mid, toScreenPos };

            transform.DOScale(Vector3.one, _scaleUpDuration)
                .SetEase(Ease.OutBack)
                .SetLink(gameObject)
                .OnComplete(() =>
                    transform.DOPath(path, _duration, PathType.CatmullRom)
                        .SetEase(Ease.InQuart)
                        .SetLink(gameObject)
                        .OnComplete(() =>
                        {
                            onComplete?.Invoke();
                            Destroy(gameObject);
                        }));
        }
    }
}
