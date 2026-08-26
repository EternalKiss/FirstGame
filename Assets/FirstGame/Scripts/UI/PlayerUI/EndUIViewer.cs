using UnityEngine;
using System;
using UnityEngine.UI;

namespace FirstGame.PlayerUI
{
    public class EndUIViewer : MonoBehaviour
    {
        [SerializeField] private Canvas _endUI;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _exitButton;

        public event Action RestartButtonPressed;
        public event Action ContinueButtonPressed;
        public event Action ExitButtonPressed;

        private void Awake()
        {
            if (_restartButton != null) _restartButton.onClick.AddListener(OnRestartClick);
            if (_continueButton != null) _continueButton.onClick.AddListener(OnContinueClick);
            if (_exitButton != null) _exitButton.onClick.AddListener(OnExitClick);
        }

        public void ShowEndUI()
        {
            if (!_endUI.gameObject.activeSelf)
            {
                _endUI.gameObject.SetActive(true);
            }
        }

        public void CloseEndUI()
        {
            if (_endUI.gameObject.activeSelf)
            {
                _endUI.gameObject.SetActive(false);
            }
        }

        private void OnRestartClick()
        {
            RestartButtonPressed?.Invoke();
        }

        private void OnContinueClick()
        {
            ContinueButtonPressed?.Invoke();
        }

        private void OnExitClick()
        {
            ExitButtonPressed?.Invoke();
        }

        private void OnDestroy()
        {
            if (_restartButton != null) _restartButton.onClick.RemoveListener(OnRestartClick);
            if (_continueButton != null) _continueButton.onClick.RemoveListener(OnContinueClick);
            if (_exitButton != null) _exitButton.onClick.RemoveListener(OnExitClick);
        }
    }
}
