using FirstGame.Combat;
using FirstGame.ObjectPool;
using System;
using UnityEngine;
using EnemyComponent = FirstGame.Enemy.Enemy;

namespace FirstGame.Spawner
{
    public class BossSpawner : MonoBehaviour
    {
        [SerializeField] private EnemyPool _enemyPool;
        [SerializeField] private float _startHealth = 300f;
        [SerializeField] private float _damage = 70f;
        [SerializeField] private float _attackRange = 6f;
        [SerializeField] private float _scale = 5f;

        private EnemyComponent _currentBoss;

        public event Action BossDied;

        public void Spawn(float cycleMultiplier, Vector3 spawnPosition)
        {
            _currentBoss = _enemyPool.Get();
            _currentBoss.transform.position = spawnPosition;
            _currentBoss.transform.rotation = Quaternion.identity;

            _currentBoss.Initialize(_startHealth * cycleMultiplier, _attackRange, _damage * cycleMultiplier, _scale);

            Health bossHealth = _currentBoss.GetHealthComponent();

            if (bossHealth != null)
            {
                bossHealth.Died += HandleBossDied;
            }
        }

        private void HandleBossDied()
        {
            if (_currentBoss != null)
            {
                Health bossHealth = _currentBoss.GetHealthComponent();

                if (bossHealth != null)
                {
                    bossHealth.Died -= HandleBossDied;
                }
            }

            _currentBoss = null;
            BossDied?.Invoke();
        }
    }
}