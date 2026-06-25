using System;
using UnityEngine;
using Potato.Interactions;

namespace Potato.Player
{
    public class PlayerInteractor : MonoBehaviour
    {
        private PlayerConfig _config;

        public void Initialize(PlayerConfig config) => _config = config;

        public IInteractable Nearest { get; private set; }

        // UI can subscribe to show/hide interaction prompt
        public event Action<IInteractable> OnNearestChanged;

        private readonly Collider[] _buffer = new Collider[16];

        private void Update()
        {
            RefreshNearest();

            if (Nearest != null && Input.GetKeyDown(KeyCode.E))
                Nearest.Interact();
        }

        private void RefreshNearest()
        {
            int count = Physics.OverlapSphereNonAlloc(
                transform.position, _config.interactionRadius, _buffer, _config.interactableLayer);

            IInteractable best = null;
            float bestDist = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                var candidate = _buffer[i].GetComponentInParent<IInteractable>();
                if (candidate == null) continue;

                float dist = (_buffer[i].transform.position - transform.position).sqrMagnitude;
                if (dist < bestDist) { bestDist = dist; best = candidate; }
            }

            if (best == Nearest) return;
            Nearest = best;
            OnNearestChanged?.Invoke(Nearest);
        }

        private void OnDrawGizmosSelected()
        {
            if (_config == null) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _config.interactionRadius);
        }
    }
}
