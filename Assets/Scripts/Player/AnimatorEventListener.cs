using UnityEngine;

namespace Potato.Player
{
    public class AnimatorEventListener : MonoBehaviour
    {
        [SerializeField] private Character _character;

        public void OnAttackHit() => _character.OnAttackHit();
    }
}
