using FirstGame.LevelManager;
using FirstGame.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FirstGame.PlayerUI
{
    public class TimerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private Image _progressFill;
        [SerializeField] private Image _backgroundImage;

        [SerializeField] private Color _dayColor = new Color(0.95f, 0.65f, 0.15f);
        [SerializeField] private Color _warningColor = new Color(0.95f, 0.25f, 0.15f);
        [SerializeField] private Color _nightColor = new Color(0.35f, 0.25f, 0.75f);

        private DayNightCycle _dayNightCycle;
        private UniversalEventBinder _eventBinder;

        private void Awake()
        {
            _eventBinder = new UniversalEventBinder();

            if (_dayNightCycle == null)
            {
                _dayNightCycle = DayNightCycle.Instance;
            }
        }

        private void OnEnable()
        {
            _eventBinder.Bind(
                callback => _dayNightCycle.DayStarted += callback,
                callback => _dayNightCycle.DayStarted -= callback,
                HandleDayStarted);

            _eventBinder.Bind(
                callback => _dayNightCycle.NightStarted += callback,
                callback => _dayNightCycle.NightStarted -= callback,
                HandleNightStarted);

            _eventBinder.Bind(
                callback => _dayNightCycle.SunsetWarning += callback,
                callback => _dayNightCycle.SunsetWarning -= callback,
                HandleSunsetWarning);
        }

        private void OnDisable()
        {
            _eventBinder.UnbindAll();
        }

        private void Update()
        {
            if (_dayNightCycle == null)
            {
                return;
            }

            float remaining = _dayNightCycle.TimeRemaining;

            int seconds = Mathf.CeilToInt(remaining);
            int minutes = seconds / 60;
            int secondsPart = seconds % 60;

            _timerText.text = $"{minutes:00}:{secondsPart:00}";

            if (_progressFill != null)
            {
                float total = GetTotalDuration();
                float fillAmount = 0f;

                if (total > 0f)
                {
                    fillAmount = (remaining / total) * 0.5f;
                }

                _progressFill.fillAmount = fillAmount;
            }
        }

        private float GetTotalDuration()
        {
            if (_dayNightCycle.IsNight)
            {
                return _dayNightCycle.NightDuration;
            }

            return _dayNightCycle.DayDuration;
        }

        private void HandleDayStarted()
        {
            _backgroundImage.color = _dayColor;
        }

        private void HandleSunsetWarning()
        {
            _backgroundImage.color = _warningColor;
        }

        private void HandleNightStarted()
        {
            _backgroundImage.color = _nightColor;
        }
    }
}
