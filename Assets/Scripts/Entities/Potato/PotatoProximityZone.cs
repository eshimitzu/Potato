using UnityEngine;
using Potato.Entities.Worm;

namespace Potato.Entities.Potato
{
    public class PotatoProximityZone : MonoBehaviour
    {
        [SerializeField] private PotatoController _potato;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<WormController>() != null)
                _potato.OnWormEnter();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponentInParent<WormController>() != null)
                _potato.OnWormExit();
        }
    }
}
