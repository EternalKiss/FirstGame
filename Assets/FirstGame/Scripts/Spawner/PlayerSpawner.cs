using FirstGame.Camera;
using FirstGame.Players;
using UnityEngine;

namespace FirstGame.Spawner
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private Vector3 _spawnPosition;
        [SerializeField] private Follower _cameraFollow;

        public Player Spawn()
        {
            Player player = Instantiate(_playerPrefab, _spawnPosition, Quaternion.identity);
            player.Initialize();
            _cameraFollow.Initialize(player);

            return player;
        }
    }
}