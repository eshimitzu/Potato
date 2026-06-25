using DG.Tweening;
using UnityEngine;

namespace Potato.UI
{
    public class CurrencyFly : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.6f;
        [SerializeField] private float _scaleUpDuration = 0.15f;

        public void Play(Vector3 fromScreenPos, Vector3 toScreenPos)
        {
            transform.position = fromScreenPos;
            transform.localScale = Vector3.zero;

            transform.DOScale(Vector3.one, _scaleUpDuration)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                    transform.DOMove(toScreenPos, _duration)
                        .SetEase(Ease.InQuart)
                        .OnComplete(() => Destroy(gameObject)));
        }
    }
}
