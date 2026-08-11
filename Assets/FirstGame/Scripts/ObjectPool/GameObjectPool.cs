using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace FirstGame.ObjectPool
{
    public class GameObjectPool<T> : MonoBehaviour where T : Component, IDestructible
    {
        [SerializeField] private T _prefab;
        [SerializeField] private int _defaultCapacity = 10;
        [SerializeField] private int _maxPoolSize = 20;

        private ObjectPool<T> _pool;
        private WaitForSeconds _waitReleaseDelay;

        public void Initialize()
        {
            _pool = new ObjectPool<T>(
                createFunc: OnCreateObject,
                actionOnGet: OnTakeFromPool,
                actionOnRelease: OnReturnToPool,
                actionOnDestroy: OnDestroyObject,
                collectionCheck: true,
                defaultCapacity: _defaultCapacity,
                maxSize: _maxPoolSize
            );
        }

        public T Get()
        {
            return _pool.Get();
        }

        public void Release(T poolObject)
        {
            _pool.Release(poolObject);
        }

        private T OnCreateObject()
        {
            T poolObject = Instantiate(_prefab, transform);

            poolObject.OnReadyToRelease += HandleObjectDeath;
            return poolObject;
        }

        private void OnTakeFromPool(T poolObject)
        {
            poolObject.gameObject.SetActive(true);
        }

        private void OnReturnToPool(T poolObject)
        {   
            poolObject.gameObject.SetActive(false);
        }

        private void OnDestroyObject(T poolObject)
        {
            poolObject.OnReadyToRelease -= HandleObjectDeath;
            Destroy(poolObject);
        }

        private void HandleObjectDeath(IDestructible destructible)
        {
            if (destructible is T obj)
            {
                Release(obj);
            }
        }
    }
}
