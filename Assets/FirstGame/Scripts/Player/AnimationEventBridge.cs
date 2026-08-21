using UnityEngine;

namespace FirstGame.Players
{
    public class AnimationEventBridge : MonoBehaviour
    {
        private Player _playerRoot;

        private void Awake()
        {
            _playerRoot = GetComponentInParent<Player>();
        }

        public void OnAttackHitEvent()
        {
            if (_playerRoot != null)
            {
                _playerRoot.OnAttackHitEvent();
            }
        }
    }
}
