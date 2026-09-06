using FirstGame.Players.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.PlayerUI
{
    public class PlayerXPMediator : MonoBehaviour
    {
        [SerializeField] private XpViewer _xpView;
        [SerializeField] private Experience _playerExperience;

        private void OnEnable()
        {
            if (_playerExperience != null)
            {
                _playerExperience.OnExperienceChanged += HandleXpChanged;
                _playerExperience.OnLevelUp += HandleLevelUp;

                HandleXpChanged(_playerExperience.CurrentXp, _playerExperience.RequiredXpForNextLevel);
            }
        }

        private void OnDisable()
        {
            if (_playerExperience != null)
            {
                _playerExperience.OnExperienceChanged -= HandleXpChanged;
                _playerExperience.OnLevelUp -= HandleLevelUp;
            }
        }

        private void HandleXpChanged(float currentXp, float maxXp)
        {
            if (_xpView != null)
            {
                _xpView.UpdateXpBar(currentXp, maxXp);
            }
        }

        private void HandleLevelUp(int currentLevel)
        {
            if (_xpView != null)
            {
                _xpView.UpdateLevelText(currentLevel);
            }
        }
    }
}