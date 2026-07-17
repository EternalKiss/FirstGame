using FirstGame.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace FirstGame.ObjectPool
{
    public class GameObjectPool<T> : MonoBehaviour where T : Component
    {
        [SerializeField] private Resource _prefab;
        [SerializeField] private int _defaultCapacity = 10;
        [SerializeField] private int _maxPoolSize = 20;

        private ObjectPool<Resource> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<Resource>(
                createFunc: OnCreateObject,
                actionOnGet: OnTakeFromPool,
                actionOnRelease: OnReturnToPool,
                actionOnDestroy: OnDestroyObject,
                collectionCheck: false,
                defaultCapacity: _defaultCapacity,
                maxSize: _maxPoolSize
            );
        }

        private Resource OnCreateObject()
        {
            return Instantiate(_prefab, transform);
        }

        private void OnTakeFromPool(Resource resource)
        {
            resource.gameObject.SetActive(true);
        }

        private void OnReturnToPool(Resource resource)
        {
            resource.gameObject.SetActive(false);
        }

        private void OnDestroyObject(Resource resource)
        {
            Destroy(resource);
        }

        public Resource Get()
        {
            return _pool.Get();
        }

        public void Release(Resource resource)
        {
            _pool.Release(resource);
        }
    }
}
