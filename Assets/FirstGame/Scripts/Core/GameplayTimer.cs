using System;

namespace FirstGame.Core
{
    public class GameplayTimer
    {
        private float _duration;
        private float _timeRemaining;
        private bool _isActive;

        public event Action OnTimerFinished;
        public float Progress => _duration > 0 ? (1f - (_timeRemaining / _duration)) : 0f;
        public bool IsActive => _isActive;

        public GameplayTimer(float duration)
        {
            _duration = duration;
        }

        public void Start()
        {
            _timeRemaining = _duration;
            _isActive = true;
        }

        public void Stop()
        {
            _isActive = false;
        }

        public void Tick(float deltaTime)
        {
            if (!_isActive) return;

            _timeRemaining -= deltaTime;

            if (_timeRemaining <= 0)
            {
                _isActive = false;
                OnTimerFinished?.Invoke();
            }
        }

        public void ChangeDuration(float newDuration)
        {
            _duration = newDuration;
        }
    }
}
