using System;
using UnityEngine;

namespace FirstGame.LevelManager
{
    public class DayNightCycle : MonoBehaviour
    {
        public static DayNightCycle Instance { get; private set; }

        [SerializeField] private float _dayDuration = 90f;
        [SerializeField] private float _nightDuration = 45f;
        [SerializeField] private float _sunsetWarningTime = 10f;

        private float _timeRemaining;
        private bool _isNight;
        private bool _isRunning;
        private bool _isPaused;

        public event Action DayStarted;
        public event Action SunsetWarning;
        public event Action NightStarted;
        public event Action SunriseStarted;

        public float TimeRemaining => _timeRemaining;
        public bool IsNight => _isNight;
        public bool IsRunning => _isRunning;
        public float DayDuration => _dayDuration;
        public float NightDuration => _nightDuration;

        private bool _warningFired;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartDay()
        {
            _isNight = false;
            _isRunning = true;
            _isPaused = false;
            _warningFired = false;
            _timeRemaining = _dayDuration;
            DayStarted?.Invoke();
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }

        public void Stop()
        {
            _isRunning = false;
        }

        private void Update()
        {
            if (!_isRunning || _isPaused) return;

            _timeRemaining -= Time.deltaTime;

            if (!_isNight && !_warningFired && _timeRemaining <= _sunsetWarningTime)
            {
                _warningFired = true;
                SunsetWarning?.Invoke();
            }

            if (_timeRemaining <= 0f)
            {
                if (_isNight) StartDay();
                else StartNight();
            }
        }

        private void StartNight()
        {
            _isNight = true;
            _warningFired = false;
            _timeRemaining = _nightDuration;
            NightStarted?.Invoke();
        }
    }
}