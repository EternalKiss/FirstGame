using FirstGame.Environment;
using UnityEngine;

namespace FirstGame.Spawner
{
    public class ResourceSpawner : BaseSpawner<Resource>
    {
        [SerializeField] private float _startHealth = 50f;
        [SerializeField] private int _objectsCountToSpawn = 10;

        [SerializeField] private Vector2 _spawnAreaRangeX = new Vector2(-15f, 15f);
        [SerializeField] private Vector2 _spawnAreaRangeZ = new Vector2(-15f, 15f);

        public void SpawnResources()
        {
            TrySpawn();
        }

        protected override void TrySpawn()
        {
            for (int i = 0; i < _objectsCountToSpawn; i++)
            {
                base.TrySpawn();
            }
        }

        protected override Vector3 GetSpawnPosition()
        {
            float randomX = Random.Range(_spawnAreaRangeX.x, _spawnAreaRangeX.y);
            float randomZ = Random.Range(_spawnAreaRangeZ.x, _spawnAreaRangeZ.y);

            return new Vector3(randomX, 1f, randomZ);
        }

        protected override void InitializeSpawnedObject(Resource resource)
        {
            resource.Initialize(_startHealth);
        }
    }
}
