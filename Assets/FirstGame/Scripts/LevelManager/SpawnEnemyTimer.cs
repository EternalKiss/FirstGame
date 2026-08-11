using System;
using UnityEngine;

namespace FirstGame.LevelManager
{
    public class SpawnEnemyTimer : MonoBehaviour
    {
        [SerializeField] private float _duration = 5f;

        private float _timeRemaining;
        private bool _isActive;

        public event Action OnTimerFinished;

        public void StartTimer()
        {
            _timeRemaining = _duration;
            _isActive = true;
        }

        private void Update()
        {
            if (!_isActive) return;

            _timeRemaining -= Time.deltaTime;

            if (_timeRemaining <= 0)
            {
                _isActive = false;
                OnTimerFinished?.Invoke();
            }
        }
    }
}