using UnityEngine;

namespace FirstGame.Enemy
{
    public class EnemyAnimationEventBridge : MonoBehaviour
    {
        private Enemy _enemyRoot;

        private void Awake()
        {
            _enemyRoot = GetComponentInParent<Enemy>();

            if (_enemyRoot == null)
            {
                Debug.LogError($"[EnemyAnimationEventBridge] Не найден скрипт Enemy в родителях у {name}!");
            }
        }

        public void OnAttackHitEvent()
        {
            if (_enemyRoot != null)
            {
                _enemyRoot.OnAttackHitEvent();
            }
        }

    }
}
