using UnityEngine;

namespace Potato.Core
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _smoothSpeed = 8f;

        private Vector3 _offset;

        private void Awake()
        {
            if (_target != null)
                _offset = transform.position - _target.position;
        }

        private void LateUpdate()
        {
            if (_target == null) return;
            Vector3 desired = _target.position + _offset;
            transform.position = Vector3.Lerp(transform.position, desired, _smoothSpeed * Time.deltaTime);
        }
    }
}
