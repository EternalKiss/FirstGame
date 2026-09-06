using FirstGame.ObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.Spawner
{
    public abstract class BasisSpawner<T> : MonoBehaviour where T : Component, IDestructible
    {
        [SerializeField] private GameObjectPool<T> _objectPool;


        private readonly List<T> _activeObjects = new List<T>(32);
        public event Action<T> ObjectSpawned;

        public void InitializePool()
        {
            if (_objectPool != null)
            {
                _objectPool.Initialize();
            }
            else
            {
                Debug.LogError($"[BaseSpawner] Пул объектов не назначен на спавнере {name}!");
            }
        }

        public void ClearActiveObjects()
        {
            int count = _activeObjects.Count;

            for (int i = count - 1; i >= 0; i--)
            {
                T obj = _activeObjects[i];

                if (obj != null)
                {
                    if (obj.gameObject.activeSelf == true)
                    {
                        _objectPool.Release(obj);
                    }
                }
            }

            _activeObjects.Clear();
        }

        public void SpawnAtPosition(Vector3 position)
        {
            T spawnedComponent = _objectPool.Get();

            spawnedComponent.transform.position = position;
            spawnedComponent.transform.rotation = Quaternion.identity;

            _activeObjects.Add(spawnedComponent);

            if (ObjectSpawned != null)
            {
                ObjectSpawned.Invoke(spawnedComponent);
            }

            InitializeSpawnedObject(spawnedComponent);
        }

        protected virtual void TrySpawn()
        {
            T spawnedComponent = _objectPool.Get();

            Vector3 spawnPosition = GetSpawnPosition();
            spawnedComponent.transform.position = spawnPosition;
            spawnedComponent.transform.rotation = Quaternion.identity;

            _activeObjects.Add(spawnedComponent);

            if (ObjectSpawned != null)
            {
                ObjectSpawned.Invoke(spawnedComponent);
            }

            InitializeSpawnedObject(spawnedComponent);
        }

        protected void ReturnToPool(T spawnedComponent)
        {
            if (_activeObjects.Contains(spawnedComponent))
            {
                _activeObjects.Remove(spawnedComponent);
            }
            _objectPool.Release(spawnedComponent);
        }

        protected abstract Vector3 GetSpawnPosition();
        protected abstract void InitializeSpawnedObject(T spawnedComponent);
    }
}