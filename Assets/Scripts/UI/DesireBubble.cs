using DG.Tweening;
using Potato.Entities.Potato;
using UnityEngine;
using UnityEngine.UI;

namespace Potato.UI
{
    public class DesireBubble : MonoBehaviour
    {
        [SerializeField] private PotatoNeed _potatoNeed;
        [SerializeField] private PotatoUpgrader _upgrader;
        [SerializeField] private GameObject _root;
        [SerializeField] private SpriteRenderer _icon;

        [SerializeField] private float _potatoSize;

        private void Awake()
        {
        }

        private void Start()
        {
            _root.SetActive(false);
            _potatoNeed.OnNeedAppeared += Show;
            _potatoNeed.OnNeedSatisfied += Hide;
            if (_upgrader != null)
                _upgrader.OnLevelChanged += OnLevelChanged;
        }

        private void OnDestroy()
        {
            _potatoNeed.OnNeedAppeared -= Show;
            _potatoNeed.OnNeedSatisfied -= Hide;
            if (_upgrader != null)
                _upgrader.OnLevelChanged -= OnLevelChanged;
        }

        private void OnLevelChanged(int _)
        {
            float scale = _upgrader.CurrentData.scale;
            Vector3 pos = transform.localPosition;
            transform.DOLocalMoveY((_potatoSize) * scale - 1, 0.4f).SetEase(Ease.OutBack).SetLink(gameObject);
        }

        private void Show(PotatoNeed.Need need)
        {
            if (_icon != null) _icon.sprite = need.currency.icon;

            _root.SetActive(true);
            _root.transform.localScale = Vector3.zero;
            _root.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).SetLink(gameObject);
        }

        private void Hide()
        {
            _root.transform.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .SetLink(gameObject)
                .OnComplete(() => _root.SetActive(false));
        }
    }
}
