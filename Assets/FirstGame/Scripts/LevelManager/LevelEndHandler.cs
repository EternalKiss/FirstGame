using FirstGame.PlayerUI;
using FirstGame.Spawner;
using UnityEngine;

namespace FirstGame.LevelManager
{
    public class LevelEndHandler : MonoBehaviour
    {
        [SerializeField] private EndUIViewer _endUIViewer;

        private LevelProgressController _progressController;
        private Restarter _restarter;
        private LevelLoader _levelLoader;

        private void Awake()
        {
            _restarter = GetComponent<Restarter>();
            _levelLoader = GetComponent<LevelLoader>();
        }

        private void OnDestroy()
        {
            if (_progressController != null)
            {
                _progressController.LevelCompleted -= ActivateLevelEndPhase;
            }

            if (_endUIViewer != null)
            {
                _endUIViewer.RestartButtonPressed -= Restart;
                _endUIViewer.ContinueButtonPressed -= LoadNextLevel;
            }
        }

        public void Initialize(LevelProgressController progressController)
        {
            _progressController = progressController;
            _progressController.LevelCompleted += ActivateLevelEndPhase;

            if (_endUIViewer != null)
            {
                _endUIViewer.RestartButtonPressed += Restart;
                _endUIViewer.ContinueButtonPressed += LoadNextLevel;
            }
        }

        private void ActivateLevelEndPhase()
        {
            if (_endUIViewer != null)
            {
                _endUIViewer.ShowEndUI();
            }
        }

        private void Restart()
        {
            if (_restarter != null)
            {
                _restarter.RestartLevel();
            }
        }

        private void LoadNextLevel()
        {

        }
    }
}
