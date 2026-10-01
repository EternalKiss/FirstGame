using FirstGame.Common;
using FirstGame.Players;
using FirstGame.PlayerUI;
using FirstGame.Sound;
using System;
using UnityEngine;

namespace FirstGame.LevelManager
{
    public class EndUIHandler : MonoBehaviour
    {
        [SerializeField] private EndUIViewer _endUIViewer;
        [SerializeField] private Restarter _restarter;
        [SerializeField] private GameOverHandler _gameOverHandler;
        [SerializeField] private AudioService _audioService;

        private readonly UniversalEventBinder _eventBinder = new UniversalEventBinder();

        private void OnEnable()
        {
            if (_endUIViewer == null) return;

            _eventBinder.Bind(
                callback => _endUIViewer.RestartButtonPressed += callback,
                callback => _endUIViewer.RestartButtonPressed -= callback,
                HandleRestart);

            _eventBinder.Bind(
                callback => _endUIViewer.ContinueButtonPressed += callback,
                callback => _endUIViewer.ContinueButtonPressed -= callback,
                HandleContinue);

            _eventBinder.Bind(
                callback => _endUIViewer.ExitButtonPressed += callback,
                callback => _endUIViewer.ExitButtonPressed -= callback,
                HandleExit);
        }

        private void OnDisable()
        {
            _eventBinder.UnbindAll();
        }

        private void HandleRestart()
        {
            _audioService.PlayUIClick();

            if (_restarter != null) _restarter.RestartLevel();
        }

        private void HandleContinue()
        {
            if (_gameOverHandler == null) return;

            _audioService.PlayUIClick();

            if (_gameOverHandler.TryRevive())
            {
                return;
            }

            if (_restarter != null) _restarter.RestartLevel();
        }

        private void HandleExit()
        {
            _audioService.PlayUIClick();
            Application.Quit();
        }
    }
}