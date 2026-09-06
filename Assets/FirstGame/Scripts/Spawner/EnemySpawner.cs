using FirstGame.Enemy;
using FirstGame.Spawner;
using UnityEngine;

public class EnemySpawner : BasisSpawner<Enemy>
{
    [SerializeField] private float _startHealth = 100f;
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private Vector2 _spawnAreaRangeX = new Vector2(-15f, 15f);
    [SerializeField] private Vector2 _spawnAreaRangeZ = new Vector2(10f, 25f); // Спавним чуть впереди игрока

    private float _nextSpawnTime;
    private bool _canSpawn;
    private Transform _currentTarget;

    private void Update()
    {
        if (!_canSpawn)
        {
            return;
        }

        if (Time.time >= _nextSpawnTime)
        {
            _nextSpawnTime = Time.time + _spawnInterval;
            TrySpawn();
        }
    }

    public void StartSpawning()
    {
        _canSpawn = true;
        _nextSpawnTime = Time.time + _spawnInterval;
    }

    public void SetPlayerTarget(Transform target)
    {
        _currentTarget = target;
    }

    public void ClearActiveEnemies()
    {
        _canSpawn = false;

        ClearActiveObjects();
    }

    protected override void TrySpawn()
    {
        base.TrySpawn();
    }

    protected override Vector3 GetSpawnPosition()
    {
        if (_currentTarget == null)
        {
            return Vector3.zero;
        }

        float randomX = _currentTarget.position.x + Random.Range(_spawnAreaRangeX.x, _spawnAreaRangeX.y);
        float randomZ = _currentTarget.position.z + Random.Range(_spawnAreaRangeZ.x, _spawnAreaRangeZ.y);

        return new Vector3(randomX, 0f, randomZ);
    }

    protected override void InitializeSpawnedObject(Enemy enemy)
    {
        if (enemy.TryGetComponent<UnityEngine.AI.NavMeshAgent>(out var agent))
        {
            agent.enabled = true;
        }

        if (enemy.TryGetComponent<CharacterController>(out var controller))
        {
            controller.enabled = true;
        }

        enemy.Initialize(_startHealth);

        PlayerDetector detector = enemy.GetComponent<PlayerDetector>();

        if (detector != null && _currentTarget != null)
        {
            detector.SetTarget(_currentTarget);
        }
    }
}
