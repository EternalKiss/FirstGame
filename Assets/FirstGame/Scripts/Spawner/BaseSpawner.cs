using FirstGame.Environment;
using FirstGame.ObjectPool;
using UnityEngine;

namespace FirstGame.Spawner
{
    public abstract class BaseSpawner<T> : MonoBehaviour where T : Component
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

        private void TrySpawn()
        {
            Resource spawnedResource = _objectPool.Get();

            Vector3 spawnPosition = GetSpawnPosition();
            spawnedResource.transform.position = spawnPosition;
            spawnedResource.transform.rotation = Quaternion.identity;

            InitializeSpawnedObject(spawnedResource);
        }

        protected void ReturnToPool(Resource resource)
        {
            _objectPool.Release(resource);
        }

        protected abstract Vector3 GetSpawnPosition();
        protected abstract void InitializeSpawnedObject(Resource resource);
    }
}
