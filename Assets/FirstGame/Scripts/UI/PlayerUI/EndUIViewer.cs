using UnityEngine;
using System;
using UnityEngine.UI;

namespace FirstGame.PlayerUI
{
    public class EndUIViewer : MonoBehaviour
    {
        [SerializeField] private GameObject _endUI;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _continueButton;

        public event Action RestartButtonPressed;
        public event Action ContinueButtonPressed;

        private void Awake()
        {
            if (_restartButton != null) _restartButton.onClick.AddListener(OnRestartClick);
            if (_continueButton != null) _continueButton.onClick.AddListener(OnContinueClick);
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

        private void OnDestroy()
        {
            if (_restartButton != null) _restartButton.onClick.RemoveListener(OnRestartClick);
            if (_continueButton != null) _continueButton.onClick.RemoveListener(OnContinueClick);
        }
    }
}
