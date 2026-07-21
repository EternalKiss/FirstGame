using FirstGame.ObjectPool;
using UnityEngine;

namespace FirstGame.Spawner
{
    public abstract class BaseSpawner<T> : MonoBehaviour where T : Component, IDestructible
    {
        [SerializeField] private GameObjectPool<T> _objectPool;
        [SerializeField] private float _spawnInterval = 5f;

        private float _nextSpawnTime;

        protected virtual void Update()
        {
            if (Time.time >= _nextSpawnTime)
            {
                _nextSpawnTime = Time.time + _spawnInterval;
                TrySpawn();
            }
        }

        public void SpawnAtPosition(Vector3 position)
        {
            T spawnedComponent = _objectPool.Get();

            spawnedComponent.transform.position = position;
            spawnedComponent.transform.rotation = Quaternion.identity;

            InitializeSpawnedObject(spawnedComponent);
        }

        private void TrySpawn()
        {
            T spawnedComponent = _objectPool.Get();

            Vector3 spawnPosition = GetSpawnPosition();
            spawnedComponent.transform.position = spawnPosition;
            spawnedComponent.transform.rotation = Quaternion.identity;

            InitializeSpawnedObject(spawnedComponent);
        }

        protected void ReturnToPool(T spawnedComponent)
        {
            _objectPool.Release(spawnedComponent);
        }

        protected abstract Vector3 GetSpawnPosition();
        protected abstract void InitializeSpawnedObject(T spawnedComponent);
    }
}
