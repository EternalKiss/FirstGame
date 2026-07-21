using FirstGame.Enemy;
using FirstGame.Players;
using FirstGame.Spawner;
using UnityEngine;

public class EnemySpawner : BaseSpawner<Enemy>
{
    [SerializeField] private float _startHealth = 100f;

    [SerializeField] private Vector2 _spawnAreaRangeX = new Vector2(-15f, 15f);
    [SerializeField] private Vector2 _spawnAreaRangeZ = new Vector2(-15f, 15f);

    private Player _playerTarget;
    
    public void SetPlayerTarget(Player player)
    {
        _playerTarget = player;
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

        if (detector != null && _playerTarget != null)
        {
            detector.SetTarget(_playerTarget);
        }
    }
}
