using UnityEngine;

namespace Potato.Player
{
    [CreateAssetMenu(menuName = "Potato/PlayerConfig", fileName = "PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        public float moveSpeed = 4f;
        public float rotationSpeed = 720f;
        public float interactionRadius = 1.5f;
        public LayerMask interactableLayer;
        public float attackCooldown = 0.8f;
    }
}
