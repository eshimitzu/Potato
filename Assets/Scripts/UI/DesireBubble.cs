using DG.Tweening;
using Potato.Entities.Potato;
using UnityEngine;
using UnityEngine.UI;

namespace Potato.UI
{
    public class DesireBubble : MonoBehaviour
    {
        [SerializeField] private PotatoNeed _potatoNeed;
        [SerializeField] private GameObject _root;
        [SerializeField] private SpriteRenderer _icon;

        private void Start()
        {
            _root.SetActive(false);
            _potatoNeed.OnNeedAppeared += Show;
            _potatoNeed.OnNeedSatisfied += Hide;
        }

        private void OnDestroy()
        {
            _potatoNeed.OnNeedAppeared -= Show;
            _potatoNeed.OnNeedSatisfied -= Hide;
        }

        private void Show(PotatoNeed.Need need)
        {
            if (_icon != null) _icon.sprite = need.currency.icon;

            _root.SetActive(true);
            _root.transform.localScale = Vector3.zero;
            _root.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        }

        private void Hide()
        {
            _root.transform.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(() => _root.SetActive(false));
        }
    }
}
