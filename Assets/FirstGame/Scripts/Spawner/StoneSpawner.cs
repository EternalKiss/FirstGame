using FirstGame.Environment;
using UnityEngine;

namespace FirstGame.Spawner
{
    public class StoneSpawner : BaseSpawner<Resource>
    {
        [SerializeField] private float _startHealth = 50f;
        [SerializeField] private int _stonesCount = 10;

        [SerializeField] private Vector2 _spawnAreaRangeX = new Vector2(-15f, 15f);
        [SerializeField] private Vector2 _spawnAreaRangeZ = new Vector2(-15f, 15f);

        [SerializeField] private float _minScale = 0.7f;
        [SerializeField] private float _maxScale = 1.3f;

        public int StonesCount => _stonesCount;

        public void SpawnResources()
        {
            TrySpawn();
        }

        protected override void TrySpawn()
        {
            for (int i = 0; i < _stonesCount; i++)
            {
                base.TrySpawn();
            }
        }

        protected override Vector3 GetSpawnPosition()
        {
            float randomX = Random.Range(_spawnAreaRangeX.x, _spawnAreaRangeX.y);
            float randomZ = Random.Range(_spawnAreaRangeZ.x, _spawnAreaRangeZ.y);

            return new Vector3(randomX, 0f, randomZ);
        }

        protected override void InitializeSpawnedObject(Resource resource)
        {
            resource.Initialize(_startHealth);

            float randomScale = Random.Range(_minScale, _maxScale);
            resource.transform.localScale = Vector3.one * randomScale;

            float randomRotationY = Random.Range(0f, 360f);
            resource.transform.rotation = Quaternion.Euler(0f, randomRotationY, 0f);
        }
    }
}
