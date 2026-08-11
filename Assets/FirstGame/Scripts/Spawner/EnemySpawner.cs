using FirstGame.Enemy;
using FirstGame.Spawner;
using UnityEngine;

public class EnemySpawner : BaseSpawner<Enemy>
{
    [SerializeField] private float _startHealth = 100f;
    [SerializeField] private float _spawnInterval = 5f;

    private float _nextSpawnTime;
    private bool _canSpawn;

    [SerializeField] private Vector2 _spawnAreaRangeX = new Vector2(-15f, 15f);
    [SerializeField] private Vector2 _spawnAreaRangeZ = new Vector2(-15f, 15f);

    private Transform _currentTarget;

    public void StartSpawning()
    {
        _canSpawn = true;
        _nextSpawnTime = Time.time + _spawnInterval;
    }

    private void Update()
    {
        if (!_canSpawn) return;

        if (Time.time >= _nextSpawnTime)
        {
            _nextSpawnTime = Time.time + _spawnInterval;
            TrySpawn();
        }
    }

    public void SetPlayerTarget(Transform target)
    {
        _currentTarget = target;
    }

    protected override void TrySpawn()
    {
        base.TrySpawn();
    }

    protected override Vector3 GetSpawnPosition()
    {
        float randomX = Random.Range(_spawnAreaRangeX.x, _spawnAreaRangeX.y);
        float randomZ = Random.Range(_spawnAreaRangeZ.x, _spawnAreaRangeZ.y);

        return new Vector3(randomX, 1f, randomZ);
    }

    protected override void InitializeSpawnedObject(Enemy enemy)
    {
        enemy.Initialize(_startHealth);

        PlayerDetector detector = enemy.GetComponent<PlayerDetector>();

        if (detector != null && _currentTarget != null)
        {
            detector.SetTarget(_currentTarget);
        }
    }
}
