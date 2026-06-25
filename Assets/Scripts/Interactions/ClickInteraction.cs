using UnityEngine;

namespace Potato.Interactions
{
    // Supports both 3D (Physics.Raycast) and 2D (Physics2D.GetRayIntersection).
    // Set _use2D = true in Inspector if your scene is 2D.
    public class ClickInteraction : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private bool _use2D;

        private void Awake()
        {
            if (_camera == null) _camera = Camera.main;
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            if (_use2D)
                Handle2D();
            else
                Handle3D();
        }

        private void Handle3D()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
                hit.collider.GetComponentInParent<IInteractable>()?.Interact();
        }

        private void Handle2D()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.collider != null)
                hit.collider.GetComponentInParent<IInteractable>()?.Interact();
        }
    }
}
