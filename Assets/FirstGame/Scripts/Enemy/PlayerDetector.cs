using FirstGame.Interfaces;
using FirstGame.Players;
using UnityEngine;

namespace FirstGame.Enemy
{
    public class PlayerDetector : MonoBehaviour
    {
        private Transform _playerTransform;
        public IDamageable PlayerDamageable { get; private set; }

        public void SetTarget(Player player)
        {
            _playerTransform = player.transform;

            PlayerDamageable = player;

            if (PlayerDamageable == null) Debug.LogError("[PlayerDetector] Ошибка! IDamageable игрока равен null.");
        }

        public Vector3 GetPlayerPosition()
        {
            if (_playerTransform != null)
            {
                return _playerTransform.position;
            }

            return transform.position;
        }

        public bool HasTarget => _playerTransform != null;
    }
}