using FirstGame.Combat;
using FirstGame.Players;
using FirstGame.PlayerUI;
using UnityEngine;

namespace FirstGame.LevelManager
{
    public class GameOverHandler : MonoBehaviour
    {
        [SerializeField] private EndUIViewer _endUIViewer;

        private Health _playerHealth;
        private PlayerInputReader _playerInputReader;
        private bool _isGameOver;

        public void Initialize(Player player)
        {
            if (player == null) return;

            _playerHealth = player.GetComponent<Health>();

            if (_playerHealth != null)
            {
                _playerHealth.Died += HandlePlayerDied;
            }

            Debug.Log("[GameOverHandler] Initialize, health = " + (_playerHealth != null));
        }

        private void OnDestroy()
        {
            if (_playerHealth != null)
            {
                _playerHealth.Died -= HandlePlayerDied;
            }
        }

        private void HandlePlayerDied()
        {
            if (_isGameOver) return;

            _isGameOver = true;

            if (_playerInputReader != null)
            {
                _playerInputReader.enabled = false;
            }

            Time.timeScale = 0f;

            if(_endUIViewer != null)
            {
                _endUIViewer.ShowEndUI();
            }

            Debug.Log("[GameOverHandler] Player died!");
        }
    }
}
