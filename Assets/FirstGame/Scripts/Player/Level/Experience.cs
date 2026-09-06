using System;
using UnityEngine;

namespace FirstGame.Players.Level
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] private float _baseRequiredXp = 100f;
        [SerializeField] private float _xpMultiplier = 1.2f;

        private int _currentLevel = 1;
        private float _currentXp = 0f;
        private float _requiredXpForNextLevel;

        public event Action<float, float> OnExperienceChanged;
        public event Action<int> OnLevelUp;

        public int CurrentLevel => _currentLevel;
        public float CurrentXp => _currentXp;
        public float RequiredXpForNextLevel => _requiredXpForNextLevel;

        private void Awake()
        {
            _requiredXpForNextLevel = _baseRequiredXp;
        }

        private void Start()
        {
            OnExperienceChanged?.Invoke(_currentXp, _requiredXpForNextLevel);
            OnLevelUp?.Invoke(_currentLevel);
        }

        public void AddExperience(float amount)
        {
            if (amount <= 0f)
            {
                return;
            }

            _currentXp += amount;

            if (_currentXp >= _requiredXpForNextLevel)
            {
                _currentXp -= _requiredXpForNextLevel;
                _currentLevel++;

                _requiredXpForNextLevel = Mathf.RoundToInt(_requiredXpForNextLevel * _xpMultiplier);

                OnLevelUp?.Invoke(_currentLevel);
                Debug.Log(_currentLevel);
            }

            OnExperienceChanged?.Invoke(_currentXp, _requiredXpForNextLevel);
        }
    }
}
