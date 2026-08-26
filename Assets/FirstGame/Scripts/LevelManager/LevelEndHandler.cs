using UnityEngine;
using FirstGame.Base;
using FirstGame.PlayerUI;

namespace FirstGame.LevelManager
{
    public class LevelEndHandler : MonoBehaviour
    {
        [SerializeField] private EnterTriggerHandler _enterTrigger;
        [SerializeField] private EndUIViewer _endUIViewer;

        private Restarter _restarter;
        private LevelLoader _levelLoader;

        private void Awake()
        {
            _restarter = GetComponent<Restarter>();
        }

        private void OnEnable()
        {
            _enterTrigger.EnteredInBase += ShowEndUI;
            _endUIViewer.RestartButtonPressed += Restart;
            _endUIViewer.ContinueButtonPressed += LoadNextLevel;
        }

        private void OnDisable()
        {
            _enterTrigger.EnteredInBase -= ShowEndUI;
        }

        private void ShowEndUI()
        {
            _endUIViewer.ShowEndUI();
        }

        private void Restart()
        {
            _restarter.RestartLevel();
        }

        private void LoadNextLevel()
        {

        }
    }
}
