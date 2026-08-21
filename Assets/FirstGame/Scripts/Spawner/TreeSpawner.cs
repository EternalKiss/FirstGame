using FirstGame.Environment;
using UnityEngine;

namespace FirstGame.Spawner
{
    public class TreeSpawner : BaseSpawner<Resource>
    {
        [SerializeField] private float _treeHealth = 100f;
        [SerializeField] private int _treesCount = 20;

        [SerializeField] private Vector2 _spawnAreaRangeX = new Vector2(-15f, 15f);
        [SerializeField] private Vector2 _spawnAreaRangeZ = new Vector2(-15f, 15f);

        [SerializeField] private float _minScale = 0.8f;
        [SerializeField] private float _maxScale = 1.2f;

        public int TreesCount => _treesCount;

        public void SpawnTrees()
        {
            for (int i = 0; i < _treesCount; i++)
            {
                TrySpawn();
            }
        }

        protected override Vector3 GetSpawnPosition()
        {
            float randomX = Random.Range(_spawnAreaRangeX.x, _spawnAreaRangeX.y);
            float randomZ = Random.Range(_spawnAreaRangeZ.x, _spawnAreaRangeZ.y);
            return new Vector3(randomX, 1.5f, randomZ);
        }

        protected override void InitializeSpawnedObject(Resource resource)
        {
            resource.Initialize(_treeHealth);

            float randomScale = Random.Range(_minScale, _maxScale);
            resource.transform.localScale = Vector3.one * randomScale;
        }
    }
}
