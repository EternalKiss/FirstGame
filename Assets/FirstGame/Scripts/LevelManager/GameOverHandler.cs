using FirstGame.Combat;
using FirstGame.Players;
using FirstGame.PlayerUI;
using UnityEngine;

namespace FirstGame.LevelManager
{
    public class GameOverHandler : MonoBehaviour
    {
        [SerializeField] private EndUIViewer _endUIViewer;
        [SerializeField] private EndUIHandler _endUIHandler;

        private Health _playerHealth;
        private PlayerInputReader _playerInputReader;
        private PlayerReviver _playerReviver;
        private bool _isGameOver;

        public void Initialize(Player player)
        {
            if (player == null) return;

            _playerHealth = player.GetComponent<Health>();
            _playerInputReader = player.GetComponent<PlayerInputReader>();
            _playerReviver = player.GetComponent<PlayerReviver>();

            if (_playerHealth != null)
            {
                _playerHealth.Died += HandlePlayerDied;
            }   
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

        public bool TryRevive()
        {
            if (_isGameOver == false) return false;
            if (_playerReviver == null) return false;
            if (_playerReviver.CanRevive == false) return false;

            if (_playerReviver.TryRevive() == false) return false;

            _isGameOver = false;

            if (_endUIViewer != null)
            {
                _endUIViewer.CloseEndUI();
            }

            Time.timeScale = 1f;
            return true;
        }
    }
}
